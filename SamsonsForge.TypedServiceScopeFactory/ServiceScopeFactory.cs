using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace SamsonsForge.TypedServiceScopeFactory
{
    /// <inheritdoc cref="IServiceScopeFactory" />
    public interface IServiceScopeFactory<out T> where T : class
    {
        /// <inheritdoc cref="IServiceScope" />
        IServiceScope<T> CreateScope();
    }

    /// <inheritdoc cref="IServiceScope" />
    public interface IServiceScope<out T> : IDisposable where T : class
    {
        /// <inheritdoc cref="ServiceProviderServiceExtensions.GetRequiredService{T}(IServiceProvider)" />
        T GetRequiredService();
        /// <inheritdoc cref="ServiceProviderServiceExtensions.GetService{T}(IServiceProvider)" />
        T GetService();
        /// <inheritdoc cref="ServiceProviderServiceExtensions.GetServices{T}(IServiceProvider)" />
        IEnumerable<T> GetServices();
    }

    /// <inheritdoc cref="IServiceScopeFactory" />
    public class ServiceScopeFactory<T> : IServiceScopeFactory<T> where T : class
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        /// <inheritdoc cref="IServiceScopeFactory" />
        public ServiceScopeFactory(IServiceScopeFactory serviceScopeFactory) => _serviceScopeFactory = serviceScopeFactory;

        /// <inheritdoc cref="ServiceProviderServiceExtensions.CreateScope(IServiceProvider)" />
        public IServiceScope<T> CreateScope() => new ServiceScope<T>(_serviceScopeFactory.CreateScope());
    }

    /// <inheritdoc cref="IServiceScope" />
    public class ServiceScope<T> : IServiceScope<T> where T : class
    {
        private readonly IServiceScope _scope;

        /// <inheritdoc cref="IServiceScopeFactory.CreateScope" />
        public ServiceScope(IServiceScope scope) => _scope = scope;

        /// <inheritdoc />
        public T GetRequiredService() => _scope.ServiceProvider.GetRequiredService<T>();

        /// <inheritdoc />
        public T GetService() => _scope.ServiceProvider.GetService<T>();

        /// <inheritdoc />
        public IEnumerable<T> GetServices() => _scope.ServiceProvider.GetServices<T>();

        private bool _disposed = false;

        /// <inheritdoc cref="IDisposable.Dispose" />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        protected virtual void Dispose(bool calledFromCodeNotTheGarbageCollector)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
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

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        ~ServiceScope()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            Dispose(false);
        }
    }
}
