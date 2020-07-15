using Castle.DynamicProxy;
using ConfigNet.Stores;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConfigNet
{
    internal class ConfigInterceptor : IInterceptor
    {
        private List<IStore> _stores;

        public ConfigInterceptor(List<IStore> stores)
        {
            _stores = stores;
        }

        public void Intercept(IInvocation invocation)
        {
            var key = invocation.Method.Name;

            // Setter
            if (key.StartsWith("set_", StringComparison.Ordinal))
            {
                foreach(var store in _stores)
                {
                    store.WriteValue(key.Replace("set_", string.Empty), invocation.Arguments[0]);
                }
            }
            // Getter
            else if (key.StartsWith("get_", StringComparison.Ordinal))
            {
                // Since returning multiple values doesn't make sense, only read from the first store in the list
                invocation.ReturnValue = _stores[0].ReadValue(key.Replace("get_", string.Empty), invocation.Method.ReturnType);
            }
            // Custom Method
            else
            {
                throw new NotImplementedException("Custom Methods are not implemented yet");
            }
        }
    }
}
