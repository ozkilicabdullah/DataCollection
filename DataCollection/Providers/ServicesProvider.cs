using System;

namespace DataCollection.Providers
{
    public class ServicesProvider<TInterface> : IServicesProvider<TInterface>
    {
        private IServiceProvider _serviceProvider;

        public ServicesProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public TInterface GetInstance(string key)
        {
            var func = GetService();
            return func(key);
        }

        private Func<string, TInterface> GetService()
        {
            return (Func<string, TInterface>)_serviceProvider
                .GetService(typeof(Func<string, TInterface>));          
        }
    }

}
