using System;
using Festivals.API.Infrastructure.DbContexts;
using Festivals.API.ResourceParameters;
using Festivals.API.Service;
using Moq;

namespace Festivals.API.Test.RepositoryTests
{
    public class FestivalsRepositoryTestBase : IDisposable
    {
        public FestivalsRepositoryTestBase()
        {
            ContextBase = FestivalsDbContextFactory.Create();
            PropertyMappingServiceBase = new PropertyMappingService();

            MedievalFestivalsRepositoryBase = new FestivalsRepository(ContextBase, PropertyMappingServiceBase);

            FestivalsResourceParametersBase = new FestivalsResourceParameters();

            DateTimeMock = new Mock<IDateTimeService>();
            DateTimeMock.Setup(m => m.Now).Returns(DateTime.Now);
        }

        public FestivalsContext ContextBase { get; }

        public IPropertyMappingService PropertyMappingServiceBase { get; }

        public IMedievalFestivalsRepository MedievalFestivalsRepositoryBase { get; }

        public FestivalsResourceParameters FestivalsResourceParametersBase { get; }

        public Mock<IDateTimeService> DateTimeMock { get; }

        public void Dispose()
        {
            FestivalsDbContextFactory.Destroy(ContextBase);
        }
    }
}
