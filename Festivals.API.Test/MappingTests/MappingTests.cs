using AutoMapper;
using System;
using Festivals.API.Infrastructure.Entities;
using Festivals.API.Mapping_Profiles;
using Festivals.API.Models;
using Xunit;

namespace Festivals.API.Test.MappingTests
{
    public class MappingTests
    {

        private readonly IConfigurationProvider _configuration;
        private readonly IMapper _mapper;

        public MappingTests()
        {
            _configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AddressProfile>();
                cfg.AddProfile<FestivalsProfile>();
            });

            _mapper = _configuration.CreateMapper();
        }

        [Fact]
        public void ShouldHaveValidConfiguration()
        {
            _configuration.AssertConfigurationIsValid();
        }

        [Theory]
        [InlineData(typeof(Address), typeof(AddressDto))]
        [InlineData(typeof(AddressDto), typeof(Address))]
        [InlineData(typeof(AddressForCreationDto), typeof(Address))]
        [InlineData(typeof(AddressForUpdateDto), typeof(Address))]
        [InlineData(typeof(Address), typeof(AddressForUpdateDto))]

        [InlineData(typeof(Festival), typeof(FestivalDto))]
        [InlineData(typeof(FestivalDto), typeof(Festival))]
        [InlineData(typeof(Festival), typeof(FestivalFullDto))]
        [InlineData(typeof(FestivalForCreationDto), typeof(Festival))]
        [InlineData(typeof(FestivalForUpdateDto), typeof(Festival))]
        [InlineData(typeof(Festival), typeof(FestivalForUpdateDto))]
        public void ShouldSupportMappingFromSourceToDestination(Type source, Type destination)
        {
            var instance = Activator.CreateInstance(source);

            _mapper.Map(instance, source, destination);
        }
    }

}
