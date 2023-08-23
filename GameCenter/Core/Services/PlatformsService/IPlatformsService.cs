using GameCenter.Dtos.PlatformDto;

namespace GameCenter.Core.Services.PlatformService;

public interface IPlatformsService
{
    Task<List<PlatformDto>> GetPlatforms();
    Task<bool> AddPlatform(PlatformDto platformDto);
    Task<bool> DeletePlatform(PlatformDto platformDto);
    Task<bool> UpdatePlatform(PlatformDto platformDto, string newName);
}
