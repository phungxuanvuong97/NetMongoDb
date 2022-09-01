using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public delegate void Registrator(Type serviceType, Func<IServiceProvider, object> implementationFactory);

        public sealed class InterfaceRegistrator<T> where T : notnull
        {
            private readonly Registrator registerRequired;
            private readonly Registrator registerOptional;

            public InterfaceRegistrator(Registrator registerRequired, Registrator registerOptional)
            {
                this.registerRequired = registerRequired;
                this.registerOptional = registerOptional;
            }

            public InterfaceRegistrator<T> AsSelf()
            {
                return this;
            }

            public InterfaceRegistrator<T> AsOptional<TInterface>()
            {
                return AsOptional(typeof(TInterface));
            }

            public InterfaceRegistrator<T> As<TInterface>()
            {
                return As(typeof(TInterface));
            }

            public InterfaceRegistrator<T> AsOptional(Type type)
            {
                if (type != typeof(T))
                {
                    registerOptional(type, c => c.GetRequiredService<T>());
                }

                return this;
            }

            public InterfaceRegistrator<T> As(Type type)
            {
                if (type != typeof(T))
                {
                    registerRequired(type, c => c.GetRequiredService<T>());
                }

                return this;
            }
        }

        public static InterfaceRegistrator<T> AddScopedAs<T>(this IServiceCollection services, Func<IServiceProvider, T> factory) where T : class
        {
            services.AddTransient(typeof(T), factory);

            return new InterfaceRegistrator<T>((t, f) => services.AddScoped(t, f), services.TryAddScoped);
        }

        public static InterfaceRegistrator<T> AddScopedAs<T>(this IServiceCollection services) where T : class
        {
            services.AddTransient<T, T>();

            return new InterfaceRegistrator<T>((t, f) => services.AddScoped(t, f), services.TryAddScoped);
        }

        public static InterfaceRegistrator<T> AddTransientAs<T>(this IServiceCollection services, Func<IServiceProvider, T> factory) where T : class
        {
            services.AddTransient(typeof(T), factory);

            return new InterfaceRegistrator<T>((t, f) => services.AddTransient(t, f), services.TryAddTransient);
        }

        public static InterfaceRegistrator<T> AddTransientAs<T>(this IServiceCollection services) where T : class
        {
            services.AddTransient<T, T>();

            return new InterfaceRegistrator<T>((t, f) => services.AddTransient(t, f), services.TryAddTransient);
        }

        public static InterfaceRegistrator<T> AddSingletonAs<T>(this IServiceCollection services, Func<IServiceProvider, T> factory) where T : class
        {
            services.AddSingleton(typeof(T), factory);

            return new InterfaceRegistrator<T>((t, f) => services.AddSingleton(t, f), services.TryAddSingleton);
        }

        public static InterfaceRegistrator<T> AddSingletonAs<T>(this IServiceCollection services) where T : class
        {
            services.AddSingleton<T, T>();

            return new InterfaceRegistrator<T>((t, f) => services.AddSingleton(t, f), services.TryAddSingleton);
        }
    }
}
