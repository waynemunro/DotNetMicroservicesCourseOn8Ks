using PlatformService.Models;

namespace PlatformService.Data
{
    public interface IPlatformRepo
    {
        bool SaveChanges();

        AppDbContext Context { get; }

        IEnumerable<Platform>? GetAllPlatforms();

        Platform? GetPlatform(int platformId);

        void AddPlatform(Platform platform);

        void UpdatePlatform(Platform platform);

        void DeletePlatform(int platformId);

        IEnumerable<Platform>? GetPlatforms(IEnumerable<int> platformIds);

        IEnumerable<Platform>? GetPlatforms(IEnumerable<string> platformNames);        
    }
}