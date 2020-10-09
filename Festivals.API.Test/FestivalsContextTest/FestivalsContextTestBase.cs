using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using Festivals.API.Infrastructure.DbContexts;
using Festivals.API.Service;

namespace Festivals.API.Test.FestivalsContextTest
{
    public class FestivalsContextTestBase : IDisposable
    {
        public FestivalsContextTestBase()
        {
            var options = new DbContextOptionsBuilder<FestivalsContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString())
               .Options;

            DateTimeMock = new Mock<IDateTimeService>();
            DateTimeMock.Setup(m => m.Now).Returns(new DateTime(3001, 1, 1));

            var userInfoServiceMock = new Mock<IUserInfoService>();
            userInfoServiceMock.Setup(m => m.UserId)
                .Returns("00000000-0000-0000-0000-000000000000");
            
            ContextBase = new FestivalsContext(options, new DateTimeService(), userInfoServiceMock.Object);
        }

        public FestivalsContext ContextBase { get; }

        public Mock<IDateTimeService> DateTimeMock { get; }

        public void Dispose()
        {
            ContextBase.Database.EnsureDeleted();

            ContextBase.Dispose();
        }
    }
}
