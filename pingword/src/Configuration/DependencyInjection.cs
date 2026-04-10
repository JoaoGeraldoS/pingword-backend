using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using pingword.src.Data;
using pingword.src.DTOs.FeedBacks;
using pingword.src.DTOs.Users;
using pingword.src.Interfaces.FeedBacks;
using pingword.src.Interfaces.Notifications;
using pingword.src.Interfaces.StudyState;
using pingword.src.Interfaces.Users;
using pingword.src.Interfaces.Words;
using pingword.src.Models.Users;
using pingword.src.Repositories.FeedBacks;
using pingword.src.Repositories.Notifications;
using pingword.src.Repositories.StudyStates;
using pingword.src.Repositories.Users;
using pingword.src.Repositories.Words;
using pingword.src.Services.Billing;
using pingword.src.Services.FeedBacks;
using pingword.src.Services.Notifications;
using pingword.src.Services.StudyState;
using pingword.src.Services.Users;
using pingword.src.Services.Words;
using pingword.src.Workers;

namespace pingword.src.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHostedService<StudyStateWorker>();

            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();


            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<ITokenService, TokenService>();
            
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<INotificationRepository, NotificationRepository>();

            services.AddScoped<IStudyStateRepository, StudyStateRepository>();
            services.AddScoped<IStudyStateService, StudyStateService>();

            services.AddScoped<IFeedBackRepository, FeedBackRepository>();
            services.AddScoped<IFeedBackService, FeedBackService>();

            services.AddScoped<IWordRepository, WordRepository>();
            services.AddScoped<IWordService, WordService>();


            services.AddValidatorsFromAssemblyContaining<FeedBackRequestDto>();
            services.AddValidatorsFromAssemblyContaining<UserRegisterRequestDto>();

            services.AddScoped<IntegrityService>();
            services.AddScoped<GooglePlayService>();


            services.Configure<JwtOptions>(configuration.GetSection("JWT"));
            var jwtOptions = configuration.GetSection("JWT").Get<JwtOptions>() ?? throw new InvalidOperationException("JWT settings not Found");

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtOptions.ValidIssuer,
                    ValidAudience = jwtOptions.ValidAudience,
                    IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),
                    ClockSkew = TimeSpan.FromMinutes(2)
                };
            });

      

            return services;
        }
    }
}
