using System.Threading.Tasks;
using RESTFestivals.API.Infrastructure.Entities;
using Xunit;

namespace RESTFestivals.API.Test.FestivalsContextTest
{
    //Test SaveChangesAsync & SaveChanges 
    public class FestivalContextTests : FestivalsContextTestBase
    {
        
        #region SaveChanges

        [Fact]
        public void SaveChanges_GivenNewAddress_ShouldSetCreatedByProperty()
        {
            //Arrange - Add Address
            Address address = new Address
            {
                Street = "Test Address",
                Number = "1",
                PostalCode = "28303",
                State = "Bremen",
                Country = "Germany"
            };

            ContextBase.Addresses.Add(address);

            //Act
            ContextBase.SaveChanges();

            //Assert
            Assert.True(address.CreatedBy == "00000000-0000-0000-0000-000000000000");
            Assert.True(address.CreatedBy != null);
            Assert.True(address.ModifiedBy == null);
        }

        [Fact]
        public void SaveChanges_GivenNewAddress_ShouldSetCreatedOnProperty()
        {
            //Arrange - Add Address
            Address address = new Address
            {
                Street = "Test Address",
                Number = "1",
                PostalCode = "28303",
                State = "Bremen",
                Country = "Germany"
            };

            ContextBase.Addresses.Add(address);

            //Act
            ContextBase.SaveChanges();

            //Assert
            Assert.True(address.CreatedOn != null);
        }
        
        [Fact]
        public void SaveChanges_UpdateAddress_ShouldSetModifiedBy()
        {
            //Arrange - Add Address and Update the Address
            Address address = new Address
            {
                Street = "Test Address",
                Number = "1",
                PostalCode = "28303",
                State = "Bremen",
                Country = "Germany"
            };

            ContextBase.Addresses.Add(address);

            ContextBase.SaveChanges();

            address.Street = "Example";

            //Act
            ContextBase.SaveChanges();

            //Assert
            Assert.True(address.ModifiedBy == "00000000-0000-0000-0000-000000000000");
            Assert.True(address.ModifiedBy != null);
        }
        
        [Fact]
        public void SaveChanges_UpdateAdress_RowVersionNewerThanCreatedOn()
        {
            //Arrange - Add Address and Update the Address
            Address address = new Address
            {
                Street = "Test Address",
                Number = "1",
                PostalCode = "28303",
                State = "Bremen",
                Country = "Germany"
            };

            ContextBase.Addresses.Add(address);

            ContextBase.SaveChanges();

            address.Street = "Example";

            //Act
            ContextBase.SaveChanges();

            //Assert
            Assert.True(address.RowVersion > address.CreatedOn);
        }
        
        #endregion
        
        #region SaveChangesAsync

        [Fact]
        public async Task SaveChangesAsync_GivenNewAddress_ShouldSetCreatedOnProperty()
        {
            //Arrange - Add Address
            Address address = new Address
            {
                Street = "Test Address",
                Number = "1",
                PostalCode = "28303",
                State = "Bremen",
                Country = "Germany"
            };

            ContextBase.Addresses.Add(address);

            //Act
            await ContextBase.SaveChangesAsync();

            //Assert
            Assert.True(address.CreatedOn != null);
        }
        
        [Fact]
        public async Task SaveChangesAsync_GivenNewAddress_ShouldSetCreatedByProperty()
        {
            //Arrange - Add Address
            Address address = new Address
            {
                Street = "Test Address",
                Number = "1",
                PostalCode = "28303",
                State = "Bremen",
                Country = "Germany"
            };

            ContextBase.Addresses.Add(address);

            //Act
            await ContextBase.SaveChangesAsync();

            //Assert
            Assert.True(address.CreatedBy == "00000000-0000-0000-0000-000000000000");
            Assert.True(address.CreatedBy != null);
            Assert.True(address.ModifiedBy == null);
        }
        
        [Fact]
        public async Task SaveChangesAsync_UpdateAdress_ShouldSetModifiedByProperty()
        {
            //Arrange - Add Address and Update the Address
            Address address = new Address
            {
                Street = "Test Address",
                Number = "1",
                PostalCode = "28303",
                State = "Bremen",
                Country = "Germany"
            };

            ContextBase.Addresses.Add(address);

            await ContextBase.SaveChangesAsync();

            address.Street = "Example";

            //Act
            await ContextBase.SaveChangesAsync();

            //Assert
            Assert.True(address.ModifiedBy == "00000000-0000-0000-0000-000000000000");
            Assert.True(address.ModifiedBy != null);
        }
        
        [Fact]
        public async Task SaveChangesAsync_UpdateAddress_RowVersionNewerThanCreatedOn()
        {
            //Arrange - Add Address and Update the Address
            Address address = new Address
            {
                Street = "Test Address",
                Number = "1",
                PostalCode = "28303",
                State = "Bremen",
                Country = "Germany"
            };

            ContextBase.Addresses.Add(address);

            await ContextBase.SaveChangesAsync();

            address.Street = "Example";

            //Act
            await ContextBase.SaveChangesAsync();

            //Assert
            Assert.True(address.RowVersion > address.CreatedOn);
        }

        #endregion
        
        
        

        
    }
}
