
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;

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
                client.BaseAddress = new Uri("https://localhost:7028/");
            });

            builder.Services.AddHttpClient("ExchangeClient", client =>
            {
                client.BaseAddress = new Uri("https://localhost:7140/");
            });

            builder.Services.AddHttpClient("HotelClient", client =>
            {
                client.BaseAddress = new Uri("https://localhost:7077/");
            });

            builder.Services.AddHttpClient("FlightClient", client =>
            {
                client.BaseAddress = new Uri("https://localhost:7044/");
            });

            builder.Services.AddHttpClient("InformationClient", client =>
            {
                client.BaseAddress = new Uri("https://localhost:7163/");
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


            var app = builder.Build();

            app.UseAuthentication();
            app.UseAuthorization();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.MapControllers();

            app.Run();
        }
    }
}
