
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Http.Resilience;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Polly;
using Polly.Timeout;
using System.Text;
using System.Threading.RateLimiting;
using TravelingApplication.Configuration;

namespace TravelingApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            //builder.Services.AddSingleton<IAuthorizationMiddlewareResultHandler, AuthorizationMiddlewareResultHandler>();
            
    

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();


            
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

                
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Enter JWT Bearer token only",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                           Array.Empty<string>()
                         }
                     });
                });

            var key = Encoding.ASCII.GetBytes(builder.Configuration["JwtSettings:SecretKey"]);

            builder.Services
            .AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();


            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "traveling",
                    ValidAudience = "users",
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            
                options.Events = new JwtBearerEvents
                {
                    OnChallenge = async context =>
                    {
                        context.HandleResponse();
            
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";
            
                        var result = new
                        {
                            message = "You are not authorized. Please login first."
                        };
            
                        await context.Response.WriteAsJsonAsync(result);
                    }
                };
            });

            builder.Services.AddAuthorization();
            builder.Services.AddHttpClient("WeatherClient", client =>
            {
                client.BaseAddress = new Uri(builder.Configuration["ExternalServices:Weather:BaseAddress"]!);
            })
            .AddResilienceHandler("WeatherResilience", resilienceBuilder =>
            {
                // 1. Rate Limiter
                resilienceBuilder.AddRateLimiter(new HttpRateLimiterStrategyOptions
                {
                    DefaultRateLimiterOptions = new ConcurrencyLimiterOptions
                    {
                        PermitLimit = 50,
                        QueueLimit = 0
                    }
                });

                // 2. Total Request Timeout
                resilienceBuilder.AddTimeout(new TimeoutStrategyOptions
                {
                    Timeout = TimeSpan.FromSeconds(30)
                });

                // 3. Retry
                resilienceBuilder.AddRetry(new HttpRetryStrategyOptions
                {
                    MaxRetryAttempts = 3,
                    Delay = TimeSpan.FromSeconds(1),
                    BackoffType = DelayBackoffType.Exponential,
                    UseJitter = true

                    //I don't have a ShouldHandler here, I'm using default to retry transient HTTP failures.
                });

                // 4. Circuit Breaker
                resilienceBuilder.AddCircuitBreaker(new HttpCircuitBreakerStrategyOptions
                {
                    FailureRatio = 0.5,
                    SamplingDuration = TimeSpan.FromSeconds(30),
                    MinimumThroughput = 10,
                    BreakDuration = TimeSpan.FromSeconds(10)
                });

                // 5. Attempt Timeout
                resilienceBuilder.AddTimeout(new TimeoutStrategyOptions
                {
                    Timeout = TimeSpan.FromSeconds(5)
                });
            });


            builder.Services.AddHttpClient("ExchangeClient", client =>
            {
                client.BaseAddress = new Uri(builder.Configuration["ExternalServices:Exchange:BaseAddress"]!);
            })
            .AddResilienceHandler("ExchangeResilience", resilienceBuilder =>
            {
                // 1. Rate Limiter
                resilienceBuilder.AddRateLimiter(new HttpRateLimiterStrategyOptions
                {
                    DefaultRateLimiterOptions = new ConcurrencyLimiterOptions
                    {
                        PermitLimit = 50,
                        QueueLimit = 0
                    }
                });

                // 2. Total Request Timeout
                resilienceBuilder.AddTimeout(new TimeoutStrategyOptions
                {
                    Timeout = TimeSpan.FromSeconds(30)
                });

                // 3. Retry
                resilienceBuilder.AddRetry(new HttpRetryStrategyOptions
                {
                    MaxRetryAttempts = 3,
                    Delay = TimeSpan.FromSeconds(1),
                    BackoffType = DelayBackoffType.Exponential,
                    UseJitter = true

                    //I don't have a ShouldHandler here, I'm using default to retry transient HTTP failures.
                });

                // 4. Circuit Breaker
                resilienceBuilder.AddCircuitBreaker(new HttpCircuitBreakerStrategyOptions
                {
                    FailureRatio = 0.5,
                    SamplingDuration = TimeSpan.FromSeconds(30),
                    MinimumThroughput = 10,
                    BreakDuration = TimeSpan.FromSeconds(10)
                });

                // 5. Attempt Timeout
                resilienceBuilder.AddTimeout(new TimeoutStrategyOptions
                {
                    Timeout = TimeSpan.FromSeconds(5)
                });
            });


            builder.Services.AddHttpClient("HotelClient", client =>
            {
                client.BaseAddress = new Uri(builder.Configuration["ExternalServices:Hotel:BaseAddress"]!);
            })
            .AddResilienceHandler("HotelResilience", resilienceBuilder =>
            {
                // 1. Rate Limiter
                resilienceBuilder.AddRateLimiter(new HttpRateLimiterStrategyOptions
                {
                    DefaultRateLimiterOptions = new ConcurrencyLimiterOptions
                    {
                        PermitLimit = 50,
                        QueueLimit = 0
                    }
                });

                // 2. Total Request Timeout
                resilienceBuilder.AddTimeout(new TimeoutStrategyOptions
                {
                    Timeout = TimeSpan.FromSeconds(30)
                });

                // 3. Retry
                resilienceBuilder.AddRetry(new HttpRetryStrategyOptions
                {
                    MaxRetryAttempts = 3,
                    Delay = TimeSpan.FromSeconds(1),
                    BackoffType = DelayBackoffType.Exponential,
                    UseJitter = true

                    //I don't have a ShouldHandler here, I'm using default to retry transient HTTP failures.
                });

                // 4. Circuit Breaker
                resilienceBuilder.AddCircuitBreaker(new HttpCircuitBreakerStrategyOptions
                {
                    FailureRatio = 0.5,
                    SamplingDuration = TimeSpan.FromSeconds(30),
                    MinimumThroughput = 10,
                    BreakDuration = TimeSpan.FromSeconds(10)
                });

                // 5. Attempt Timeout
                resilienceBuilder.AddTimeout(new TimeoutStrategyOptions
                {
                    Timeout = TimeSpan.FromSeconds(5)
                });
            });


            builder.Services.AddHttpClient("FlightClient", client =>
            {
                client.BaseAddress = new Uri(builder.Configuration["ExternalServices:Flight:BaseAddress"]!);
            })
            .AddResilienceHandler("FlightResilience", resilienceBuilder =>
            {
                // 1. Rate Limiter
                resilienceBuilder.AddRateLimiter(new HttpRateLimiterStrategyOptions
                {
                    DefaultRateLimiterOptions = new ConcurrencyLimiterOptions
                    {
                        PermitLimit = 50,
                        QueueLimit = 0
                    }
                });

                // 2. Total Request Timeout
                resilienceBuilder.AddTimeout(new TimeoutStrategyOptions
                {
                    Timeout = TimeSpan.FromSeconds(30)
                });

                // 3. Retry
                resilienceBuilder.AddRetry(new HttpRetryStrategyOptions
                {
                    MaxRetryAttempts = 3,
                    Delay = TimeSpan.FromSeconds(1),
                    BackoffType = DelayBackoffType.Exponential,
                    UseJitter = true

                    //I don't have a ShouldHandler here, I'm using default to retry transient HTTP failures.
                });

                // 4. Circuit Breaker
                resilienceBuilder.AddCircuitBreaker(new HttpCircuitBreakerStrategyOptions
                {
                    FailureRatio = 0.5,
                    SamplingDuration = TimeSpan.FromSeconds(30),
                    MinimumThroughput = 10,
                    BreakDuration = TimeSpan.FromSeconds(10)
                });

                // 5. Attempt Timeout
                resilienceBuilder.AddTimeout(new TimeoutStrategyOptions
                {
                    Timeout = TimeSpan.FromSeconds(5)
                });
            });


            builder.Services.AddHttpClient("InformationClient", client =>
            {
                client.BaseAddress = new Uri(builder.Configuration["ExternalServices:Information:BaseAddress"]!);
            })
            .AddResilienceHandler("InformationResilience", resilienceBuilder =>
            {
                // 1. Rate Limiter
                resilienceBuilder.AddRateLimiter(new HttpRateLimiterStrategyOptions
                {
                    DefaultRateLimiterOptions = new ConcurrencyLimiterOptions
                    {
                        PermitLimit = 50,
                        QueueLimit = 0
                    }
                });

                // 2. Total Request Timeout
                resilienceBuilder.AddTimeout(new TimeoutStrategyOptions
                {
                    Timeout = TimeSpan.FromSeconds(30)
                });

                // 3. Retry
                resilienceBuilder.AddRetry(new HttpRetryStrategyOptions
                {
                    MaxRetryAttempts = 3,
                    Delay = TimeSpan.FromSeconds(1),
                    BackoffType = DelayBackoffType.Exponential,
                    UseJitter = true

                    //I don't have a ShouldHandler here, I'm using default to retry transient HTTP failures.
                });

                // 4. Circuit Breaker
                resilienceBuilder.AddCircuitBreaker(new HttpCircuitBreakerStrategyOptions
                {
                    FailureRatio = 0.5,
                    SamplingDuration = TimeSpan.FromSeconds(30),
                    MinimumThroughput = 10,
                    BreakDuration = TimeSpan.FromSeconds(10)
                });

                // 5. Attempt Timeout
                resilienceBuilder.AddTimeout(new TimeoutStrategyOptions
                {
                    Timeout = TimeSpan.FromSeconds(5)
                });
            });


            builder.Services.AddHttpClient("FoodInformationClient", client =>
            {
                client.BaseAddress = new Uri(builder.Configuration["ExternalServices:FoodInformation:BaseAddress"]!);
            })
            .AddResilienceHandler("FoodInformationResilience", resilienceBuilder =>
            {
                // 1. Rate Limiter
                resilienceBuilder.AddRateLimiter(new HttpRateLimiterStrategyOptions
                {
                    DefaultRateLimiterOptions = new ConcurrencyLimiterOptions
                    {
                        PermitLimit = 50,
                        QueueLimit = 0
                    }
                });

                // 2. Total Request Timeout
                resilienceBuilder.AddTimeout(new TimeoutStrategyOptions
                {
                    Timeout = TimeSpan.FromSeconds(30)
                });

                // 3. Retry
                resilienceBuilder.AddRetry(new HttpRetryStrategyOptions
                {
                    MaxRetryAttempts = 3,
                    Delay = TimeSpan.FromSeconds(1),
                    BackoffType = DelayBackoffType.Exponential,
                    UseJitter = true

                    //I don't have a ShouldHandler here, I'm using default to retry transient HTTP failures.
                });

                // 4. Circuit Breaker
                resilienceBuilder.AddCircuitBreaker(new HttpCircuitBreakerStrategyOptions
                {
                    FailureRatio = 0.5,
                    SamplingDuration = TimeSpan.FromSeconds(30),
                    MinimumThroughput = 10,
                    BreakDuration = TimeSpan.FromSeconds(10)
                });

                // 5. Attempt Timeout
                resilienceBuilder.AddTimeout(new TimeoutStrategyOptions
                {
                    Timeout = TimeSpan.FromSeconds(5)
                });
            });

            builder.Services.Configure<JwtSettings>(
            builder.Configuration.GetSection("JwtSettings"));

            builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(
            builder.Configuration.GetConnectionString("DefaultConnection")));


            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddFluentValidationClientsideAdapters();
            builder.Services.AddValidatorsFromAssemblyContaining<GetWeatherRequestModelValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<GetInformationRequestModelValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<GetHotelBookingRequestModelValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<GetFlightBookingRequestModelValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<GetExchangeRequestModelValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<GetUserRegisterRequestModelValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<GetUserLoginRequestModelValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<GetFoodInformationRequestModelValidator>();

            builder.Services.AddScoped<BookingRequestService>();

            var app = builder.Build();

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.MapControllers();

            app.Run();
        }
    }
}
