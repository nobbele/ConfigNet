using Castle.DynamicProxy;
using ConfigNet.Stores;
using System;
using System.Collections.Generic;

namespace ConfigNet
{
    public class ConfigurationBuilder<T> where T : class
    {
        private List<IStore> _stores = new List<IStore>();

        private ProxyGenerator _proxyGenerator = new ProxyGenerator();

        public ConfigurationBuilder()
        {
            if (!typeof(T).IsInterface)
                throw new Exception("Can only create a configuration from interfaces");
        }

        public ConfigurationBuilder<T> AddStore(IStore store)
        {
            store.InitializeDefaults<T>();
            _stores.Add(store);
            return this;
        }

        public T Build()
        {
            if (_stores.Count <= 0)
                throw new Exception("Can't build a configuration without any stores");

            var proxy = _proxyGenerator.CreateInterfaceProxyWithoutTarget<T>(new ConfigInterceptor(_stores));

            return proxy;
        }
    }
}
