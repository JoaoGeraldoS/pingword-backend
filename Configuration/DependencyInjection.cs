using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using pingword.Data;
using pingword.Interfaces.Generic;
using pingword.Interfaces.Notification;
using pingword.Interfaces.Users;
using pingword.Models.Users;
using pingword.Repositories.Generic;
using pingword.Repositories.Users;
using pingword.Services.Notifications;
using pingword.Services.Users;

namespace pingword.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("Db")));

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services.AddDbContext<AppDbContext>(options => options.UseSqlite(configuration.GetConnectionString("Db")));

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();
            
            services.AddScoped<INotificationService, NotificationService>();

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            return services;
        }
    }
}
