using GameCenter.Data.UnitOfWork;
using GameCenter.Dtos.PlatformDto;
using GameCenter.Models;

namespace GameCenter.Core.Services.PlatformService;

public class PlatformService : IPlatformService
{
    private readonly IUnitOfWork _unitOfWork;

    public PlatformService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<PlatformDto>> GetPlatforms()
    {
        var platforms = await _unitOfWork.Platforms.All();
        var platformDtoList = new List<PlatformDto>();

        foreach (var platform in platforms)
        {
            platformDtoList.Add(new PlatformDto
            {
                Name = platform.PlatformName
            });
        }

        return platformDtoList;
    }

    public async Task<bool> AddPlatform(PlatformDto platformDto)
    {
        var platform = await _unitOfWork.Platforms.GetByName(platformDto.Name);

        if (platform != null)
        {
            return false;
        }

        Platform newPlatform = new Platform
        {
            PlatformName = platformDto.Name
        };

        await _unitOfWork.Platforms.Add(newPlatform);
        await _unitOfWork.CompleteAsync();

        return true;
    }

    public async Task<bool> DeletePlatform(PlatformDto platformDto)
    {
        var platform = await _unitOfWork.Platforms.GetByName(platformDto.Name);

        if (platform == null)
        {
            return false;
        }

        return await _unitOfWork.Platforms.Delete(platform);
    }

    public async Task<bool> UpdatePlatform(PlatformDto platformDto, string newName)
    {
        var platform = await _unitOfWork.Platforms.GetByName(platformDto.Name);

        if (platform == null)
        {
            return false;
        }

        platform.PlatformName = newName;

        await _unitOfWork.Platforms.Update(platform);
        await _unitOfWork.CompleteAsync();

        return true;
    }
}
