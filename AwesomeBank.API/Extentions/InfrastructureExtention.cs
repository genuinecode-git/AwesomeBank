

namespace AwesomeBank.API.Extentions
{
    public static class InfrastructureExtention
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            //Register MediatR
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            services.AddSingleton<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}
