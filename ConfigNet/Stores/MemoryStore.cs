using Castle.Core.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace ConfigNet.Stores
{
    public class MemoryStore : IStore
    {
        public Type configType { get; set; }

        private Dictionary<string, object> _dict = new Dictionary<string, object>();

        public object ReadValue(string key, Type returnType)
        {
            if (_dict.TryGetValue(key, out object val))
                return val;
            else
                return null;
        }

        public void WriteValue(string key, object value, bool writeIfPersistentNow) => _dict[key] = value;
    }
}
