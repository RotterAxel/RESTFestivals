using System;
using Festivals.API.Infrastructure.DbContexts;
using Festivals.API.Service;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Festivals.API.Test.RepositoryTests
{
    public static class FestivalsDbContextFactory
    {
        public static FestivalsContext Create()
        {
            var connectionStringBuilder =
                new SqliteConnectionStringBuilder { DataSource = ":memory:" };
            var connection = new SqliteConnection(connectionStringBuilder.ToString());

            var options = new DbContextOptionsBuilder<FestivalsContext>()
                .UseSqlite(connection)
                .Options;

            var dateTimeMock = new Mock<IDateTimeService>();
            dateTimeMock.Setup(m => m.Now)
                .Returns(DateTime.Now);

            var userInfoServiceMock = new Mock<IUserInfoService>();
            userInfoServiceMock.Setup(m => m.UserId)
                .Returns("00000000-0000-0000-0000-000000000000");

            var context = new FestivalsContext(
                options,
                dateTimeMock.Object, 
                userInfoServiceMock.Object);

            context.Database.OpenConnection();
            context.Database.EnsureCreated();

            return context;
        }
        
        public static void Destroy(FestivalsContext context)
        {
            context.Database.EnsureDeleted();

            context.Dispose();
        }
    }
}
