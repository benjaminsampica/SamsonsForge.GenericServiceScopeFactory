using Microsoft.Extensions.DependencyInjection;

namespace SamsonsForge.TypedServiceScopeFactory
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddTypedServiceScopeFactory(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton(typeof(IServiceScopeFactory<>), typeof(ServiceScopeFactory<>));

            return serviceCollection;
        }
    }
}
