using System;
using RESTFestivals.API.Infrastructure.DbContexts;
using RESTFestivals.API.ResourceParameters;
using RESTFestivals.API.Service;
using Moq;

namespace RESTFestivals.API.Test.RepositoryTests
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
