using Festivals.API.Helpers;
using Festivals.API.Infrastructure.Entities;
using Festivals.API.ResourceParameters;

namespace Festivals.API.Service
{
    public interface IMedievalFestivalsRepository
    {
        PagedList<Festival> GetFestivals(
            FestivalsResourceParameters festivalsResourceParameters);

        Festival GetFestivalFull(int festivalId);

        void AddFestival(Festival festivalEntity);
        void UpdateFestival(Festival festivalFromRepo);
        bool FesitvalExists(int festivalId);

        bool AdressExists(int adressId);

        bool Save();

        void Dispose();
        
    }
}