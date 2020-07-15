using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace ConfigNet.Stores
{
    public interface IStore
    {
        Type configType { get; set; }
        void InitializeDefaults<T>()
        {
            // Start of local function
            object GetDefaultValue(Type t)
            {
                if (t.IsValueType)
                    return Activator.CreateInstance(t);

                return null;
            }
            // End of local function

            configType = typeof(T);

            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                var key = property.Name;

                // Skip already initialized properties
                if (ReadValue(key, property.PropertyType) != null)
                    continue;

                // Get default value attribute's value, if that attribute exists
                var value = ((DefaultValueAttribute)property.GetCustomAttributes(false).SingleOrDefault(a => a.GetType() == typeof(DefaultValueAttribute)))?.Value;

                // Otherwise, get type default
                if (value == null)
                {
                    value = GetDefaultValue(property.PropertyType);
                }

                bool isLast = property.Name == properties.Last().Name;

                WriteValue(key, value, isLast);
            }
        }

        void WriteValue(string key, object value, bool writeIfPersistentNow = true);
        void WriteValues(Dictionary<string, object> dict, bool writeIfPersistentNow = true)
        {
            foreach (var pair in dict) 
            {
                bool isLast = pair.Key == dict.Last().Key;
                WriteValue(pair.Key, pair.Value, isLast ? writeIfPersistentNow : false);
            }
        }
        object ReadValue(string key, Type returnType);
    }
}
