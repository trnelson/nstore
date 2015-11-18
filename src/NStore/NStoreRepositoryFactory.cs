using System.Collections.Generic;
using System.Linq;
using NStore.Contract.Storage;

namespace NStore
{
    internal static class NStoreRepositoryFactory<T> where T : NStoreRepository, new()
    {
        internal static NStoreRepository Create(IEnumerable<NStoreRepositorySettings> plugins)
        {
            var repo = new T();
            repo.Settings = plugins.FirstOrDefault(p => p.PluginType == repo.RepositoryType);
            repo.Init();
            return repo;
        }
    }
}