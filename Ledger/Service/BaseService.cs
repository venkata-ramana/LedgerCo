using Ledger.Factories;
using Ledger.Repository;

namespace Ledger.Service
{
    public abstract class BaseService
    {
        public readonly IDataStore DataStore;

        public BaseService()
        {
            DataStore = DataStoreFactory.GetDataStore(Constants.DataStoreType.InMemoryStore);
        }
    }
}
