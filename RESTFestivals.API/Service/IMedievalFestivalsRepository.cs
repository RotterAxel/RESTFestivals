using RESTFestivals.API.Helpers;
using RESTFestivals.API.Infrastructure.Entities;
using RESTFestivals.API.ResourceParameters;

namespace RESTFestivals.API.Service
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