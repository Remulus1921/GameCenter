using GameCenter.Data.UnitOfWork;
using GameCenter.Dtos.PlatformDto;
using GameCenter.Models;

namespace GameCenter.Core.Services.PlatformService
{

    public class PlatformsService : IPlatformsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PlatformsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<PlatformDto>?> GetPlatforms()
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

            if (platformDtoList.Count > 0)
            {
                return platformDtoList;
            }
            return null;
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

            await _unitOfWork.Platforms.Delete(platform);
            await _unitOfWork.CompleteAsync();

            return true;
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
}
