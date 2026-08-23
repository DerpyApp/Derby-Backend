using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using PadelBooking.BLL.DTOs.PaymentDTOs;
using PadelBooking.BLL.Exceptions;
using PadelBooking.BLL.Options;
using PadelBooking.DAL.Enums;
using PadelBooking.DAL.Models;
using PadelBooking.DAL.Repositiory.Booking;
using PadelBooking.DAL.Repositiory.PaymentRepo;
using PadelBooking.DAL.Repositiory.UserRepo;

namespace PadelBooking.BLL.Services.Payment
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepo _paymentRepo;
        private readonly IBookingRepo _bookingRepo;
        private readonly IUserRepo _userRepo;
        private readonly IPaymobService _paymobService;
        private readonly PaymobOptions _paymobOptions;

        public PaymentService(
            IPaymentRepo paymentRepo,
            IBookingRepo bookingRepo,
            IUserRepo userRepo,
            IPaymobService paymobService,
            IOptions<PaymobOptions> paymobOptions)
        {
            _paymentRepo = paymentRepo;
            _bookingRepo = bookingRepo;
            _userRepo = userRepo;
            _paymobService = paymobService;
            _paymobOptions = paymobOptions.Value;
        }

        public async Task<PaymentIntentResponseDto> CreatePaymentIntentAsync(int bookingId, int userId)
        {
            var booking = await _bookingRepo.GetBookingWithDetailsAsync(bookingId);
            if (booking == null)
            {
                throw new NotFoundException($"Booking with ID {bookingId} not found.");
            }

            if (booking.UserId != userId)
            {
                throw new BadRequestException("You are not authorized to create a payment for this booking.");
            }

            if (booking.Status == BookingStatus.Cancelled)
            {
                throw new BadRequestException("Cannot process payment for a cancelled booking.");
            }

            if (booking.PaymentStatus == PaymentStatus.Paid)
            {
                throw new BadRequestException("This booking has already been paid.");
            }

            var payment = await _paymentRepo.GetPaymentByBookingAsync(bookingId);
            if (payment == null)
            {
                // Deposit is half of total price
                var depositAmount = booking.TotalPrice / 2;
                payment = new DAL.Models.Payment
                {
                    BookingId = booking.Id,
                    UserId = userId,
                    Amount = depositAmount,
                    Method = PaymentMethod.OnlinePayment,
                    Status = PaymentStatus.Pending,
                    CreatedAt = DateTime.UtcNow
                };

                await _paymentRepo.AddAsync(payment);
                await _paymentRepo.SaveChangesAsync();
            }

            var user = await _userRepo.GetByIdAsync(userId);
            string firstName = "Customer";
            string lastName = "User";
            string email = user?.Email ?? "customer@example.com";
            string phone = user?.PhoneNumber ?? "+201000000000";

            if (user != null && !string.IsNullOrWhiteSpace(user.FullName))
            {
                var nameParts = user.FullName.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
                firstName = nameParts[0];
                if (nameParts.Length > 1)
                {
                    lastName = nameParts[1];
                }
            }

            string specialReference = $"BOOKING_{booking.Id}";

            var (clientSecret, intentionId, checkoutUrl) = await _paymobService.CreateIntentionAsync(
                payment.Amount,
                "EGP",
                specialReference,
                firstName,
                lastName,
                email,
                phone);

            if (!string.IsNullOrEmpty(intentionId))
            {
                payment.TransactionId = intentionId;
                await _paymentRepo.SaveChangesAsync();
            }

            return new PaymentIntentResponseDto
            {
                PaymentId = payment.Id,
                BookingId = booking.Id,
                Amount = payment.Amount,
                Currency = "EGP",
                ClientSecret = clientSecret,
                PublicKey = _paymobOptions.PublicKey,
                IntentionId = intentionId,
                CheckoutUrl = checkoutUrl
            };
        }

        public async Task<PaymentDetailsDto> GetPaymentDetailsAsync(int paymentId, int userId)
        {
            var payment = await _paymentRepo.GetByIdAsync(paymentId);
            if (payment == null)
            {
                throw new NotFoundException($"Payment with ID {paymentId} not found.");
            }

            if (payment.UserId != userId)
            {
                throw new BadRequestException("You are not authorized to view this payment.");
            }

            return new PaymentDetailsDto
            {
                Id = payment.Id,
                BookingId = payment.BookingId,
                UserId = payment.UserId,
                Amount = payment.Amount,
                Method = payment.Method.ToString(),
                Status = payment.Status.ToString(),
                TransactionId = payment.TransactionId,
                PaidAt = payment.PaidAt,
                CreatedAt = payment.CreatedAt
            };
        }

        public async Task<bool> ProcessWebhookAsync(PaymobWebhookPayloadDto payload, string hmac)
        {
            if (payload?.Obj == null)
            {
                throw new BadRequestException("Invalid webhook payload.");
            }

            bool isValid = _paymobService.VerifyHmacSignature(payload.Obj, hmac);
            if (!isValid)
            {
                throw new BadRequestException("Invalid HMAC signature.");
            }

            // Extract special reference to locate booking/payment
            string? specialRef = payload.Obj.SpecialReference 
                ?? payload.Obj.Order?.SpecialReference 
                ?? payload.Obj.Order?.MerchantOrderId;

            int bookingId = 0;
            if (!string.IsNullOrWhiteSpace(specialRef) && specialRef.StartsWith("BOOKING_", StringComparison.OrdinalIgnoreCase))
            {
                int.TryParse(specialRef.Substring(8), out bookingId);
            }

            DAL.Models.Payment? payment = null;
            if (bookingId > 0)
            {
                payment = await _paymentRepo.GetPaymentByBookingAsync(bookingId);
            }

            if (payment == null && payload.Obj.Id > 0)
            {
                string transId = payload.Obj.Id.ToString();
                payment = await _paymentRepo.GetByIdAsync((int)payload.Obj.Id); // or by transaction id
            }

            if (payment == null && bookingId > 0)
            {
                var bookingToUpdate = await _bookingRepo.GetByIdAsync(bookingId);
                if (bookingToUpdate != null)
                {
                    if (payload.Obj.Success)
                    {
                        bookingToUpdate.Status = BookingStatus.Confirmed;
                        bookingToUpdate.PaymentStatus = PaymentStatus.Paid;
                    }
                    else
                    {
                        bookingToUpdate.Status = BookingStatus.Cancelled;
                        bookingToUpdate.PaymentStatus = PaymentStatus.Failed;
                    }
                    await _bookingRepo.SaveChangesAsync();
                    return true;
                }
            }

            if (payment != null)
            {
                var booking = await _bookingRepo.GetByIdAsync(payment.BookingId);
                if (payload.Obj.Success)
                {
                    payment.Status = PaymentStatus.Paid;
                    payment.TransactionId = payload.Obj.Id.ToString();
                    payment.PaidAt = DateTime.UtcNow;

                    if (booking != null)
                    {
                        booking.Status = BookingStatus.Confirmed;
                        booking.PaymentStatus = PaymentStatus.Paid;
                    }
                }
                else
                {
                    payment.Status = PaymentStatus.Failed;
                    if (booking != null)
                    {
                        booking.Status = BookingStatus.Cancelled;
                        booking.PaymentStatus = PaymentStatus.Failed;
                    }
                }

                await _paymentRepo.SaveChangesAsync();
                await _bookingRepo.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}
