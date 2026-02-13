using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using pingword.src.Data;
using pingword.src.Interfaces.Generic;
using pingword.src.Interfaces.Notifications;
using pingword.src.Interfaces.Users;
using pingword.src.Models.Users;
using pingword.src.Repositories.Generic;
using pingword.src.Repositories.Users;
using pingword.src.Services.Notifications;
using pingword.src.Services.Users;

namespace pingword.src.Configuration
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
