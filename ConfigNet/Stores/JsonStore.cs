using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ConfigNet.Stores
{
    public class JsonStore : IStore
    {
        public Type configType { get; set; }

        string _jsonPath;
        JObject _root;

        public JsonStore(string jsonPath)
        {
            _jsonPath = jsonPath;
            if (File.Exists(jsonPath))
                _root = JObject.Parse(File.ReadAllText(jsonPath));
            else
                _root = JObject.FromObject(new { });
        }


        public object ReadValue(string key, Type returnType)
        {
            if(_root.TryGetValue(key, out JToken value))
                return value.ToObject(returnType);
            return null;
        }

        public void WriteValue(string key, object value, bool writeIfPersistentNow)
        {
            if (_root.ContainsKey(key))
                _root[key] = value == null ? null : JToken.FromObject(value);
            else
                _root.Add(key, value == null ? null : JToken.FromObject(value));

            if(writeIfPersistentNow)
                WriteFile();
        }

        private void WriteFile()
        {
            var diff = _root.Properties().Select(p => p.Name).Except(configType.GetProperties().Select(p => p.Name)).ToList();
            foreach (var key in diff)
            {
                if (_root.ContainsKey(key))
                    _root.Remove(key);
            }
            File.WriteAllText(_jsonPath, _root.ToString(Formatting.Indented));
        }
    }
}
