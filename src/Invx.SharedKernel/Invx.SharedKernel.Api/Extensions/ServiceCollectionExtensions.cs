//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using System.Reflection;

//namespace Invx.SharedKernel.Api.Extensions;
//public static class ServiceCollectionExtensions
//{
//    public static IServiceCollection AddSharedKernelApi(this IServiceCollection services, IConfiguration configuration)
//    {
//        services.AddControllers(options =>
//        {
//            options.Filters.Add<ValidationFilter>();
//        });

//        services.AddEndpointsApiExplorer();
//        services.AddSwaggerGen(c =>
//        {
//            c.SwaggerDoc("v1", new OpenApiInfo
//            {
//                Title = "Microservice API",
//                Version = "v1"
//            });

//            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//            {
//                Description = "JWT Authorization header using the Bearer scheme",
//                Name = "Authorization",
//                In = ParameterLocation.Header,
//                Type = SecuritySchemeType.ApiKey,
//                Scheme = "Bearer"
//            });

//            c.AddSecurityRequirement(new OpenApiSecurityRequirement
//            {
//                {
//                    new OpenApiSecurityScheme
//                    {
//                        Reference = new OpenApiReference
//                        {
//                            Type = ReferenceType.SecurityScheme,
//                            Id = "Bearer"
//                        }
//                    },
//                    Array.Empty<string>()
//                }
//            });
//        });

//        services.AddHealthChecks();
//        services.AddCors();

//        return services;
//    }

//    public static IServiceCollection AddFluentValidation(this IServiceCollection services, params Assembly[] assemblies)
//    {
//        services.AddValidatorsFromAssemblies(assemblies);
//        return services;
//    }

//    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
//    {
//        var jwtSettings = configuration.GetSection("JwtSettings");
//        var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey is required");

//        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//            .AddJwtBearer(options =>
//            {
//                options.TokenValidationParameters = new TokenValidationParameters
//                {
//                    ValidateIssuer = true,
//                    ValidateAudience = true,
//                    ValidateLifetime = true,
//                    ValidateIssuerSigningKey = true,
//                    ValidIssuer = jwtSettings["Issuer"],
//                    ValidAudience = jwtSettings["Audience"],
//                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
//                };
//            });

//        return services;
//    }
//}