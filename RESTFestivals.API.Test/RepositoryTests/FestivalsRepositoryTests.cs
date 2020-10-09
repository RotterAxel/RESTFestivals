using System;
using System.Linq;
using RESTFestivals.API.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace RESTFestivals.API.Test.RepositoryTests
{
    public class FestivalsRepositoryTests : FestivalsRepositoryTestBase
    {
        [Fact]
        public void GetFestivalsCollection_ReturnedFestivalsDontStartInThePast()
        {
            //Arrange - Add 2 Festivals in Today or in the Future and one in Starting in the past
            ContextBase.Addresses.AddRange(
                new Address
                {
                    Street = "Test Address",
                    Number = "1",
                    PostalCode = "28303",
                    State = "Bremen",
                    Country = "Germany"
                });

            ContextBase.SaveChanges();

            
            ContextBase.Festivals.AddRange(
                new Festival
                {
                    Title = "Mittelalterfest 1",
                    Description = "",
                    StartDate = DateTimeMock.Object.Now.AddDays(5),
                    EndDate = DateTimeMock.Object.Now.AddDays(8),
                    AddressId = ContextBase.Addresses.First(a => a.Street == "Test Address").Id
                },
                new Festival
                {
                    Title = "Mittelalterfest 1",
                    Description = "",
                    StartDate = DateTimeMock.Object.Now.AddDays(-4),
                    EndDate = DateTimeMock.Object.Now.AddDays(8),
                    AddressId = ContextBase.Addresses.First(a => a.Street == "Test Address").Id
                },
                new Festival
                {
                    Title = "Mittelalterfest 1",
                    Description = "",
                    StartDate = DateTimeMock.Object.Now.AddHours(1),
                    EndDate = DateTimeMock.Object.Now.AddDays(8),
                    AddressId = ContextBase.Addresses.First(a => a.Street == "Test Address").Id
                });

            ContextBase.SaveChanges();

            //Act
            var festivals = MedievalFestivalsRepositoryBase.GetFestivals(FestivalsResourceParametersBase);

            //Assert
            foreach(var festival in festivals.ToList())
            {
                Assert.True(festival.StartDate >= DateTimeMock.Object.Now);
            }
            Assert.True(festivals.Count() == 2);
        }

        [Fact]
        public void GetFestivalsCollection_NoPageSizeDefined_Return10PagesByDefault()
        {
            //Arrange - Add 11 Festivals
            ContextBase.Addresses.AddRange(
               new Address
               {
                   Street = "Test Address",
                   Number = "1",
                   PostalCode = "28303",
                   State = "Bremen",
                   Country = "Germany"
               });

            ContextBase.SaveChanges();

            for (int i = 0; i < 11; i++)
            {
                ContextBase.Festivals.Add(
                 new Festival
                 {
                     Title = "Mittelalterfest 1",
                     Description = "",
                     StartDate = DateTimeMock.Object.Now.AddDays(5),
                     EndDate = DateTimeMock.Object.Now.AddDays(8),
                     AddressId = ContextBase.Addresses.First(a => a.Street == "Test Address").Id
                 });
            }

            ContextBase.SaveChanges();

            //Act
            var festivals = MedievalFestivalsRepositoryBase.GetFestivals(FestivalsResourceParametersBase);

            //Assert
            Assert.True(festivals.Count() == 10);
            
        }

        [Fact]
        public void GetFestivalsCollection_NoResourceParameters_OrderByDateByDefault()
        {
            //Arrange - Add 3 Festivals that most days until it starts first
            ContextBase.Addresses.AddRange(
                new Address
                {
                    Street = "Test Address",
                    Number = "1",
                    PostalCode = "28303",
                    State = "Bremen",
                    Country = "Germany"
                });

            ContextBase.SaveChanges();


            ContextBase.Festivals.AddRange(
                new Festival
                {
                    Title = "Mittelalterfest 3",
                    Description = "",
                    StartDate = DateTimeMock.Object.Now.AddDays(5),
                    EndDate = DateTimeMock.Object.Now.AddDays(8),
                    AddressId = ContextBase.Addresses.First(a => a.Street == "Test Address").Id
                },
                new Festival
                {
                    Title = "Mittelalterfest 2",
                    Description = "",
                    StartDate = DateTimeMock.Object.Now.AddDays(3),
                    EndDate = DateTimeMock.Object.Now.AddDays(8),
                    AddressId = ContextBase.Addresses.First(a => a.Street == "Test Address").Id
                },
                new Festival
                {
                    Title = "Mittelalterfest 1",
                    Description = "",
                    StartDate = DateTimeMock.Object.Now.AddHours(1),
                    EndDate = DateTimeMock.Object.Now.AddDays(8),
                    AddressId = ContextBase.Addresses.First(a => a.Street == "Test Address").Id
                });

            ContextBase.SaveChanges();

            //Act
            var festivals = MedievalFestivalsRepositoryBase.GetFestivals(FestivalsResourceParametersBase);

            var festivalsOrderedByDate = festivals.OrderBy(f => f.StartDate).ToList();

            //Assert
            Assert.True(festivals.Count() > 2 && festivalsOrderedByDate.Count() > 2);

            for(int i = 0; i < festivals.Count(); ++i)
            {
                Assert.True(festivals[i].StartDate == festivalsOrderedByDate[i].StartDate);
            }
        }

        [Fact]
        public void GetFestivalsCollection_PageSize49_ReturnMaximum48Pages()
        {
            //Arrange - Create 49 Festivals
            ContextBase.Addresses.AddRange(
                new Address
                {
                    Street = "Test Address",
                    Number = "1",
                    PostalCode = "28303",
                    State = "Bremen",
                    Country = "Germany"
                });

            ContextBase.SaveChanges();

            for (int i = 0; i < 49; i++)
            {
                ContextBase.Festivals.Add(
                 new Festival
                 {
                     Title = "Mittelalterfest 1",
                     Description = "",
                     StartDate = DateTimeMock.Object.Now.AddDays(5),
                     EndDate = DateTimeMock.Object.Now.AddDays(8),
                     AddressId = ContextBase.Addresses.First(a => a.Street == "Test Address").Id
                 });
            }

            ContextBase.SaveChanges();

            FestivalsResourceParametersBase.PageSize = 49;

            //Act
            var festivals = MedievalFestivalsRepositoryBase.GetFestivals(FestivalsResourceParametersBase);

            //Assert
            Assert.True(festivals.Count() == 48);
        }

        [Fact]
        public void GetFestivalsCollection_SearchQueryByFestivalName_ReturnsAllFestivalsThatContainSeachTerm()
        {
            //Arrange
            ContextBase.Addresses.AddRange(
                new Address
                {
                    Street = "Test Address",
                    Number = "1",
                    PostalCode = "28303",
                    State = "Bremen",
                    Country = "Germany"
                });

            ContextBase.SaveChanges();

            ContextBase.Festivals.AddRange(
                new Festival
                {
                    Title = "Hellfest",
                    Description = "",
                    StartDate = DateTimeMock.Object.Now.AddDays(5),
                    EndDate = DateTimeMock.Object.Now.AddDays(8),
                    AddressId = ContextBase.Addresses.First(a => a.Street == "Test Address").Id
                },
                new Festival
                {
                    Title = "Hellbronner festival",
                    Description = "",
                    StartDate = DateTimeMock.Object.Now.AddDays(5),
                    EndDate = DateTimeMock.Object.Now.AddDays(8),
                    AddressId = ContextBase.Addresses.First(a => a.Street == "Test Address").Id
                },
                new Festival
                {
                    Title = "Incandescent festival",
                    Description = "",
                    StartDate = DateTimeMock.Object.Now.AddDays(5),
                    EndDate = DateTimeMock.Object.Now.AddDays(8),
                    AddressId = ContextBase.Addresses.First(a => a.Street == "Test Address").Id
                });

            ContextBase.SaveChanges();

            FestivalsResourceParametersBase.SearchQuery = "Hell";

            var festivalCollectionContainingHell = ContextBase.Festivals.Where(f => f.Title.Contains("Hell")).ToList();

            //Act
            var festivals = MedievalFestivalsRepositoryBase.GetFestivals(FestivalsResourceParametersBase);

            //Assert
            Assert.True(festivals.Count() == festivalCollectionContainingHell.Count());
            Assert.Contains(festivals, f => f.Title == festivalCollectionContainingHell.FirstOrDefault(fc => f.Title == fc.Title).Title);
            
        }

        [Fact]
        public void GetFestivalFull_FestivalId_ReturnsAFestivalWithAddress()
        {
            //Arrange
            ContextBase.Addresses.AddRange(
                new Address
                {
                    Street = "Test Address",
                    Number = "1",
                    PostalCode = "28303",
                    State = "Bremen",
                    Country = "Germany"
                });

            ContextBase.SaveChanges();

            ContextBase.Festivals.AddRange(
                new Festival
                {
                    Id = 14,
                    Title = "Hellfest",
                    Description = "",
                    StartDate = DateTimeMock.Object.Now.AddDays(5),
                    EndDate = DateTimeMock.Object.Now.AddDays(8),
                    AddressId = ContextBase.Addresses.First(a => a.Street == "Test Address").Id
                });

            ContextBase.SaveChanges();

            //Act
            Festival festivalFromRepository = MedievalFestivalsRepositoryBase.GetFestivalFull(14);
            
            //Assert
            Assert.True(festivalFromRepository.Address != null);
        }

        [Fact]
        public void GetFestivalFull_NonExistingFestivalId_ResturnsNull()
        {
            //Act
            Festival festivalFromRepository = MedievalFestivalsRepositoryBase.GetFestivalFull(14);

            //Assert
            Assert.True(festivalFromRepository == null);
        }

        [Fact]
        public void CreateFestival_FestivalForCreation_CreatesANewFestival()
        {
            //Arrange
            Address addressEntity = new Address
                {
                    Street = "Test Address",
                    Number = "1",
                    PostalCode = "28303",
                    State = "Bremen",
                    Country = "Germany"
                };

            Festival festivalEntity = new Festival
            {
                Title = "Hellfest",
                Description = "",
                StartDate = DateTimeMock.Object.Now.AddDays(5),
                EndDate = DateTimeMock.Object.Now.AddDays(8),
                Address = addressEntity
            };

            //Act
            MedievalFestivalsRepositoryBase.AddFestival(festivalEntity);
            ContextBase.SaveChanges();

            //Assert
            Assert.True(festivalEntity != null);
            Assert.True(festivalEntity.AddressId != 0);
            Assert.Equal(festivalEntity, ContextBase.Festivals.FirstOrDefault(f => f.Title == "Hellfest"));
        }

        [Fact]
        public void CreateFestival_FestivalForCreationNull_ThrowsArgumentNullException()
        { 
            //Assert
            Assert.Throws<ArgumentNullException>(() => MedievalFestivalsRepositoryBase.AddFestival(null));
        }

        [Fact]
        public void CreateFestival_FestivalForCreationAddressNull_ThrowsDbUpdateExceptionMissingForeignKey()
        {
            //Arrange
            Festival festivalEntity = new Festival
            {
                Title = "Hellfest",
                Description = "",
                StartDate = DateTimeMock.Object.Now.AddDays(5),
                EndDate = DateTimeMock.Object.Now.AddDays(8)
            };

            //Act
            MedievalFestivalsRepositoryBase.AddFestival(festivalEntity);

            //Assert
            Assert.Throws<DbUpdateException>(() => ContextBase.SaveChanges());
        }
    }
}
