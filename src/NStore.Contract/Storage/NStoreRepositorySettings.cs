using System;
using System.Collections.Generic;
using System.Linq;

namespace NStore.Contract.Storage
{
    public class NStoreRepositorySettings
    {
        private readonly Dictionary<string, string> _settings;

        public NStoreRepositorySettings(Dictionary<string, string> settings)
        {
            _settings = settings;
        }

        public string PluginType { get; set; }

        public TType Setting<TType>(string key)
        {
            var setting = _settings.FirstOrDefault(s => s.Key == key).Value;
            return (TType) Convert.ChangeType(setting, typeof (TType));
        }
    }
}