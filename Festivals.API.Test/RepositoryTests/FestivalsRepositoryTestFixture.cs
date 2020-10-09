using System;
using Festivals.API.Infrastructure.DbContexts;
using Xunit;

namespace Festivals.API.Test.RepositoryTests
{
    //Refer to: https://xunit.net/docs/shared-context#constructor 
    //to know when to use a base class, a fixture or a collection fixture for testing
    public sealed class FestivalsRepositoryTestFixture : IDisposable
    {
        public FestivalsRepositoryTestFixture()
        {
            Context = FestivalsDbContextFactory.Create();

            //var configurationProvider = new MapperConfiguration(cfg =>
            //{
            //    cfg.AddProfile<MappingProfile>();
            //});

            //Mapper = configurationProvider.CreateMapper();
        }
        public FestivalsContext Context { get; }

        //public IMapper Mapper { get; }

        public void Dispose()
        {
            FestivalsDbContextFactory.Destroy(Context);
        }
    }

    [CollectionDefinition("FestivalsRepositoryTests")]
    public class FestivalsRepositoryCollection : ICollectionFixture<FestivalsRepositoryTestFixture> { }
}
