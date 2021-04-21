using System;
using Microsoft.Extensions.DependencyInjection;

namespace Contoso.Domain
{
    public sealed class GetRepo
    {
        private readonly IServiceProvider provider;
        private static IServiceProvider instance;
        public GetRepo(IServiceProvider p = null) => provider = p ?? instance;
        public static void SetProvider(IServiceProvider p) => instance = p;
        public dynamic Instance(Type t) =>
            provider?.CreateScope()?.ServiceProvider?.GetService(t);
        public dynamic Instance<TService>() => Instance(typeof(TService));

    }
}

