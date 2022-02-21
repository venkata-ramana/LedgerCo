using Ledger.Constants;
using Ledger.Repository;

namespace Ledger.Factories
{
    public static class DataStoreFactory
    {
        public static IDataStore GetDataStore(DataStoreType dataStoreType)
        {
            IDataStore dataStore = null;
            switch (dataStoreType)
            {
                case DataStoreType.InMemoryStore:
                default:
                    dataStore = new InMemoryDataStore();
                    break;
            }
            return dataStore;
        }
    }
}
