using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using pingword.src.Data;
using pingword.src.DTOs.FeedBacks;
using pingword.src.Interfaces.FeedBacks;
using pingword.src.Interfaces.Notifications;
using pingword.src.Interfaces.StudyState;
using pingword.src.Interfaces.Users;
using pingword.src.Models.Users;
using pingword.src.Repositories.FeedBacks;
using pingword.src.Repositories.Notifications;
using pingword.src.Repositories.StudyStates;
using pingword.src.Repositories.Users;
using pingword.src.Services.FeedBacks;
using pingword.src.Services.Notifications;
using pingword.src.Services.StudyState;
using pingword.src.Services.Users;
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

            services.AddDbContext<AppDbContext>(options => options.UseSqlite(configuration.GetConnectionString("Db")));

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();
            
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<INotificationRepository, NotificationRepository>();

            services.AddScoped<IStudyStateRepository, StudyStateRepository>();
            services.AddScoped<IStudyStateService, StudyStateService>();

            services.AddScoped<IFeedBackRepository, FeedBackRepository>();
            services.AddScoped<IFeedBackService, FeedBackService>();

            services.AddValidatorsFromAssemblyContaining<FeedBackRequestDto>();
            

            return services;
        }
    }
}
