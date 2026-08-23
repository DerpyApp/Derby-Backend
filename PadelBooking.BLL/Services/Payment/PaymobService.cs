using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using PadelBooking.BLL.DTOs.PaymentDTOs;
using PadelBooking.BLL.Exceptions;
using PadelBooking.BLL.Options;

namespace PadelBooking.BLL.Services.Payment
{
    public class PaymobService : IPaymobService
    {
        private readonly HttpClient _httpClient;
        private readonly PaymobOptions _options;

        public PaymobService(HttpClient httpClient, IOptions<PaymobOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;
        }

        public async Task<(string ClientSecret, string IntentionId, string CheckoutUrl)> CreateIntentionAsync(
            decimal amount,
            string currency,
            string specialReference,
            string firstName,
            string lastName,
            string email,
            string phoneNumber)
        {
            var baseUrl = _options.BaseUrl.TrimEnd('/');
            var endpoint = _options.Endpoints.Intention.TrimStart('/');
            var url = $"{baseUrl}/{endpoint}";

            long amountInCents = (long)Math.Round(amount * 100, 0);

            var requestPayload = new
            {
                amount = amountInCents,
                currency = string.IsNullOrWhiteSpace(currency) ? "EGP" : currency,
                payment_methods = new object[] { },
                items = new[]
                {
                    new
                    {
                        name = "Booking Payment",
                        amount = amountInCents,
                        description = $"Payment for {specialReference}",
                        quantity = 1
                    }
                },
                billing_data = new
                {
                    first_name = string.IsNullOrWhiteSpace(firstName) ? "Customer" : firstName,
                    last_name = string.IsNullOrWhiteSpace(lastName) ? "User" : lastName,
                    phone_number = string.IsNullOrWhiteSpace(phoneNumber) ? "+201000000000" : phoneNumber,
                    email = string.IsNullOrWhiteSpace(email) ? "customer@example.com" : email
                },
                special_reference = specialReference
            };

            var requestJson = JsonSerializer.Serialize(requestPayload);
            using var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Content = new StringContent(requestJson, Encoding.UTF8, "application/json");

            // Paymob Intention API authentication header
            string secretKey = _options.SecretKey;
            request.Headers.TryAddWithoutValidation("Authorization", $"Token {secretKey}");

            var response = await _httpClient.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new BadRequestException($"Paymob Intention API failed with status {response.StatusCode}: {responseContent}");
            }

            using var doc = JsonDocument.Parse(responseContent);
            var root = doc.RootElement;

            string? clientSecret = null;
            if (root.TryGetProperty("client_secret", out var csProp) && csProp.ValueKind == JsonValueKind.String)
            {
                clientSecret = csProp.GetString();
            }
            else if (root.TryGetProperty("cs", out var csProp2) && csProp2.ValueKind == JsonValueKind.String)
            {
                clientSecret = csProp2.GetString();
            }

            string intentionId = string.Empty;
            if (root.TryGetProperty("id", out var idProp))
            {
                intentionId = idProp.ToString();
            }

            if (string.IsNullOrEmpty(clientSecret))
            {
                throw new BadRequestException($"Paymob response did not contain client_secret: {responseContent}");
            }

            string checkoutUrl = $"{baseUrl}/unifiedcheckout/?publicKey={_options.PublicKey}&clientSecret={clientSecret}";

            return (clientSecret, intentionId, checkoutUrl);
        }

        public bool VerifyHmacSignature(PaymobTransactionObjectDto obj, string receivedHmac)
        {
            if (obj == null || string.IsNullOrWhiteSpace(receivedHmac))
            {
                return false;
            }

            string hmacKey = _options.HMAC;
            if (string.IsNullOrWhiteSpace(hmacKey))
            {
                return false;
            }

            // Concatenation order as specified by Paymob:
            // amount_cents + created_at + currency + error_occured + has_parent_transaction + id + integration_id + is_3d_secure + is_auth + is_capture + is_refunded + is_standalone_payment + is_voided + order.id + owner + pending + source_data.pan + source_data.sub_type + source_data.type + success
            var concatenatedString = new StringBuilder()
                .Append(obj.AmountCents)
                .Append(obj.CreatedAt ?? "")
                .Append(obj.Currency ?? "")
                .Append(obj.ErrorOccured.ToString().ToLowerInvariant())
                .Append(obj.HasParentTransaction.ToString().ToLowerInvariant())
                .Append(obj.Id)
                .Append(obj.IntegrationId)
                .Append(obj.Is3dSecure.ToString().ToLowerInvariant())
                .Append(obj.IsAuth.ToString().ToLowerInvariant())
                .Append(obj.IsCapture.ToString().ToLowerInvariant())
                .Append(obj.IsRefunded.ToString().ToLowerInvariant())
                .Append(obj.IsStandalonePayment.ToString().ToLowerInvariant())
                .Append(obj.IsVoided.ToString().ToLowerInvariant())
                .Append(obj.Order?.Id ?? 0)
                .Append(obj.Owner)
                .Append(obj.Pending.ToString().ToLowerInvariant())
                .Append(obj.SourceData?.Pan ?? "")
                .Append(obj.SourceData?.SubType ?? "")
                .Append(obj.SourceData?.Type ?? "")
                .Append(obj.Success.ToString().ToLowerInvariant())
                .ToString();

            using var hmacSha512 = new HMACSHA512(Encoding.UTF8.GetBytes(hmacKey));
            var hashBytes = hmacSha512.ComputeHash(Encoding.UTF8.GetBytes(concatenatedString));
            var computedHmacHex = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();

            return string.Equals(computedHmacHex, receivedHmac.Trim(), StringComparison.OrdinalIgnoreCase);
        }
    }
}
