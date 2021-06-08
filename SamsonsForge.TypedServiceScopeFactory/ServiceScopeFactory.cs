using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace SamsonsForge.TypedServiceScopeFactory
{
    public interface IServiceScopeFactory<out T> where T : class
    {
        /// <inheritdoc cref="IServiceScope" />
        IServiceScope<T> CreateScope();
    }

    public interface IServiceScope<out T> : IDisposable where T : class
    {
        /// <inheritdoc cref="ServiceProviderServiceExtensions.GetRequiredService{T}(IServiceProvider)" />
        T GetRequiredService();
        /// <inheritdoc cref="ServiceProviderServiceExtensions.GetService{T}(IServiceProvider)" />
        T GetService();
        /// <inheritdoc cref="ServiceProviderServiceExtensions.GetServices{T}(IServiceProvider)" />
        IEnumerable<T> GetServices();
    }

    public class ServiceScopeFactory<T> : IServiceScopeFactory<T> where T : class
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ServiceScopeFactory(IServiceScopeFactory serviceScopeFactory) => _serviceScopeFactory = serviceScopeFactory;

        public IServiceScope<T> CreateScope() => new ServiceScope<T>(_serviceScopeFactory.CreateScope());
    }

    public class ServiceScope<T> : IServiceScope<T> where T : class
    {
        private readonly IServiceScope _scope;

        public ServiceScope(IServiceScope scope) => _scope = scope;

        /// <inheritdoc />
        public T GetRequiredService() => _scope.ServiceProvider.GetRequiredService<T>();

        /// <inheritdoc />
        public T GetService() => _scope.ServiceProvider.GetService<T>();

        /// <inheritdoc />
        public IEnumerable<T> GetServices() => _scope.ServiceProvider.GetServices<T>();

        private bool _disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool calledFromCodeNotTheGarbageCollector)
        {
            if (_disposed)
            {
                return;
            }

            if (calledFromCodeNotTheGarbageCollector)
            {
                _scope?.Dispose();
            }

            _disposed = true;
        }

        ~ServiceScope()
        {
            Dispose(false);
        }
    }
}
