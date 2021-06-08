using Microsoft.Extensions.DependencyInjection;

namespace SamsonsForge.TypedServiceScopeFactory
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public static class DependencyInjectionExtensions
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
        /// <summary>
        ///     Adds the <see cref="IServiceScopeFactory{T}"/> as a singleton to the service container.
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        public static IServiceCollection AddTypedServiceScopeFactory(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton(typeof(IServiceScopeFactory<>), typeof(ServiceScopeFactory<>));

            return serviceCollection;
        }
    }
}
