using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PadelBooking.API.Helpers;
using PadelBooking.API.Hubs;
using PadelBooking.API.Middleware;
using PadelBooking.BLL.Options;
using PadelBooking.BLL.Services.Admin;
using PadelBooking.BLL.Services.Booking;
using PadelBooking.BLL.Services.Club;
using PadelBooking.BLL.Services.Notification;
using PadelBooking.BLL.Services.Owner;
using PadelBooking.BLL.Services.Payment;
using PadelBooking.BLL.Services.Token;
using PadelBooking.BLL.Services.User;
using PadelBooking.DAL.Data;
using PadelBooking.DAL.Repositiory.Booking;
using PadelBooking.DAL.Repositiory.ClubRepo;
using PadelBooking.DAL.Repositiory.CourtBlockRepo;
using PadelBooking.DAL.Repositiory.CourtRepo;
using PadelBooking.DAL.Repositiory.CourtScheduleRepo;
using PadelBooking.DAL.Repositiory.NotificationRepo;
using PadelBooking.DAL.Repositiory.OfferRepo;
using PadelBooking.DAL.Repositiory.PaymentRepo;
using PadelBooking.DAL.Repositiory.RoleRepo;
using PadelBooking.DAL.Repositiory.UserRepo;
using System.Text;

namespace PadelBooking.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSignalR();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "PadelBooking API", Version = "v1" });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "ادخلي التوكن فقط (بدون كلمة Bearer) في الحقل أدناه."
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            builder.Services.AddScoped<IUserRepo, UserRepo>();
            builder.Services.AddScoped<IRoleRepo, RoleRepo>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

            builder.Services.AddScoped<IClubRepo, ClubRepo>();
            builder.Services.AddScoped<ICourtRepo, CourtRepo>();
            builder.Services.AddScoped<ICourtScheduleRepo, CourtScheduleRepo>();
            builder.Services.AddScoped<ICourtBlockRepo, CourtBlockRepo>();
            builder.Services.AddScoped<IBookingRepo, BookingRepo>();
            builder.Services.AddScoped<IPaymentRepo, PaymentRepo>();
            builder.Services.AddScoped<INotificationRepo, NotificationRepo>();

            builder.Services.AddScoped<IClubService, ClubService>();
            builder.Services.AddScoped<INotificationService, NotificationService>();
            builder.Services.AddScoped<INotififcationService, NotificationService>();
            builder.Services.AddScoped<IBookingService, BookingService>();
            builder.Services.AddScoped<IPaymentService, PaymentService>();

            builder.Services.AddHttpClient<IPaymobService, PaymobService>();
            builder.Services.AddScoped<IOwnerClubService, OwnerClubService>();
            builder.Services.AddScoped<IOwnerCourtService, OwnerCourtService>();
            builder.Services.AddScoped<IOwnerReservationService, OwnerReservationService>();
            builder.Services.AddScoped<IOwnerReportService, OwnerReportService>();
            builder.Services.AddScoped<INotificationRepo, NotificationRepo>();
            builder.Services.AddScoped<INotififcationService ,  NotificationService>();
            builder.Services.AddScoped<IAdminService, AdminService>();
            builder.Services.AddScoped<IOfferRepo, OfferRepo>();

            // Configure Paymob options
            builder.Services.Configure<PaymobOptions>(builder.Configuration.GetSection("PaymentGateway:Paymob"));

            // DbContext
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    sqlOptions => sqlOptions.MigrationsAssembly("PadelBooking.DAL")
                ));

            builder.Services.AddIdentity<PadelBooking.DAL.Models.User, PadelBooking.DAL.Models.Role>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Configure JWT Settings
            builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JWT"));

            // Configure JWT Authentication & SignalR WebSocket support
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = builder.Configuration["JWT:Issuer"],
                    ValidAudience = builder.Configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]!)),
                    ClockSkew = TimeSpan.Zero
                };
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        Console.WriteLine($"JWT Authentication failed: {context.Exception.Message}");
                        return Task.CompletedTask;
                    },
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs/notifications"))
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            var app = builder.Build();

            // Global exception handling
            app.UseMiddleware<GlobalExceptionMiddleware>();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseCors("AllowAll");

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.MapHub<NotificationHub>("/hubs/notifications");

            using (var scope = app.Services.CreateScope())
            {
                await DatabaseInitializer.InitializeDatabaseAsync(scope.ServiceProvider);
            }

            app.Run();
        }
    }
}