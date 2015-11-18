using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using NStore.Contract.Storage;

namespace NStore
{
    public static class NStore
    {
        private static IEnumerable<NStoreRepositorySettings> _repositorySettings;
        private static XElement _config;

        public static void Init(string configXml)
        {
            _config = XElement.Parse(configXml);
            _repositorySettings = LoadPlugins(_config);
        }

        public static NStoreRepository GetRepository<TRepositoryType>() where TRepositoryType : NStoreRepository, new()
        {
            return NStoreRepositoryFactory<TRepositoryType>.Create(_repositorySettings);
        }

        private static IEnumerable<NStoreRepositorySettings> LoadPlugins(XContainer config)
        {
            var pluginXml = config.Element("plugins").Elements("plugin");
            return
                pluginXml.Select(
                    p =>
                        new NStoreRepositorySettings(
                            p.Elements().ToDictionary(pkey => pkey.Name.LocalName, pval => pval.Value))
                        {
                            PluginType = p.Attribute("type").Value
                        });
        }
    }
}