using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PadelBooking.DAL.Data;
using PadelBooking.API.Helpers;
using PadelBooking.BLL.Services.Club;
using PadelBooking.BLL.Services.Token;
using PadelBooking.BLL.Services.User;
using PadelBooking.DAL.Repositiory.Booking;
using PadelBooking.DAL.Repositiory.ClubRepo;
using PadelBooking.DAL.Repositiory.CourtRepo;
using PadelBooking.DAL.Repositiory.CourtScheduleRepo;
using PadelBooking.DAL.Repositiory.RoleRepo;
using PadelBooking.DAL.Repositiory.UserRepo;

namespace PadelBooking.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "PadelBooking API", Version = "v1" });

                // إضافة إعدادات الـ JWT داخل Swagger
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "ادخلي التوكن بالصيغة التالية: Bearer YOUR_TOKEN"
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
            builder.Services.AddScoped<IBookingRepo, BookingRepo>();
            builder.Services.AddScoped<IClubService, ClubService>();

            //// 1. إضافة DbContext
            //builder.Services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // 1. إضافة DbContext
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
             builder.Configuration.GetConnectionString("DefaultConnection"),
            sqlOptions => sqlOptions.MigrationsAssembly("PadelBooking.DAL")
            ));

            // 3. ربط إعدادات الـ JWT من appsettings.json
            builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JWT"));

            // 4. إعداد الـ Authentication للـ JWT
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
            });

           

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            // 6. تفعيل الـ Authentication (مهم جداً أن تكون قبل UseAuthorization)
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}