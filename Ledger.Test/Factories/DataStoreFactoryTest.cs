using FluentAssertions;
using Ledger.Constants;
using Ledger.Factories;
using Ledger.Repository;
using Xunit;

namespace Ledger.Test.Factories
{
    public class DataStoreFactoryTest
    {
        [Fact]
        public void ShouldReturn_InMemoryDataStore_DefaultStore()
        {
            var dataStore = DataStoreFactory.GetDataStore(0);

            dataStore.Should().NotBeNull();
            dataStore.GetType().Should().Be(typeof(InMemoryDataStore));
        }

        [Fact]
        public void ShouldReturn_InMemoryDataStore_When_InMemoryStore_TypeProvided()
        {
            var dataStore = DataStoreFactory.GetDataStore(DataStoreType.InMemoryStore);

            dataStore.Should().NotBeNull();
            dataStore.GetType().Should().Be(typeof(InMemoryDataStore));
        }
    }
}
