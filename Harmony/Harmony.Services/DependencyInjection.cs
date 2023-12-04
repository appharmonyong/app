using Harmony.Bussiness.Services.Contracts;
using Harmony.Bussiness.Services.UserCases;
using Microsoft.Extensions.DependencyInjection;

namespace Harmony.Bussiness.Services
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<IUserServices, UserServices>();

          
            return services;
        }
    }

}
