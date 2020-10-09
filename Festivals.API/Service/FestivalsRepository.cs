using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Festivals.API.Helpers;
using Festivals.API.Infrastructure.DbContexts;
using Festivals.API.Infrastructure.Entities;
using Festivals.API.Models;
using Festivals.API.ResourceParameters;

namespace Festivals.API.Service
{
    public class FestivalsRepository : IMedievalFestivalsRepository
    {
        private readonly FestivalsContext _festivalsContext;
        private readonly IPropertyMappingService _propertyMappingService;

        public FestivalsRepository(
            FestivalsContext festivalsContext,
            IPropertyMappingService propertyMappingService)
        {
            _festivalsContext = festivalsContext ?? throw new ArgumentNullException(nameof(festivalsContext));
            this._propertyMappingService = propertyMappingService;
        }

        public PagedList<Festival> GetFestivals(
            FestivalsResourceParameters resourceParameters)
        {
            if(resourceParameters == null)
            {
                throw new ArgumentNullException(nameof(resourceParameters));
            }

            var festivals = _festivalsContext.Festivals as IQueryable<Festival>;

            if (resourceParameters.TodayOrLater)
            {
                 festivals = festivals
                    .Where(f => f.StartDate >= DateTime.Now);
            }

            if (!string.IsNullOrWhiteSpace(resourceParameters.SearchQuery))
            {

                var searchQuery = resourceParameters.SearchQuery.Trim();
                festivals = festivals.Where(a => a.Title.Contains(searchQuery)
                    /*|| a.FirstName.Contains(searchQuery)*/);
            }

            if (!string.IsNullOrWhiteSpace(resourceParameters.OrderBy))
            {
                // get property mapping dictionary
                var authorPropertyMappingDictionary =
                    _propertyMappingService.GetPropertyMapping<FestivalDto, Festival>();

                festivals = festivals.ApplySort(resourceParameters.OrderBy,
                    authorPropertyMappingDictionary);
            }

            return PagedList<Festival>.Create(festivals,
                resourceParameters.PageNumber,
                resourceParameters.PageSize);
        }

        public Festival GetFestivalFull(int festivalId)
        {
            var festivalFromRepo = _festivalsContext.Festivals
                .Include(f => f.Address)
                .FirstOrDefault(f => f.Id == festivalId);

            return festivalFromRepo;
        }

        public void AddFestival(Festival festivalEntity)
        {
            if(festivalEntity == null)
            {
                throw new ArgumentNullException(nameof(festivalEntity));
            }

            _festivalsContext.Festivals.Add(festivalEntity);
        }

        public void UpdateFestival(Festival festivalFromRepo)
        {
            //Deliberately empty code
        }

        public bool FesitvalExists(int festivalId)
        {
            return _festivalsContext.Festivals.Any(f => f.Id == festivalId);
        }

        public bool AdressExists(int adressId)
        {
            return _festivalsContext.Addresses.Any(a => a.Id == adressId);
        }

        public bool Save()
        {
            return (_festivalsContext.SaveChanges() >= 0);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose resources when needed
            }
        }
    }
}
