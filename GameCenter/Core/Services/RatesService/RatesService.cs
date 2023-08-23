using GameCenter.Data;
using GameCenter.Data.UnitOfWork;
using GameCenter.Dtos.RateDto;
using Microsoft.AspNetCore.Identity;

namespace GameCenter.Core.Services.RatesService
{
    public class RatesService : IRatesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<GameCenterUser> _userManager;

        public RatesService(IUnitOfWork unitOfWork, UserManager<GameCenterUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<RateDto?> GetAvarageRate(Guid gameId)
        {
            var game = await _unitOfWork.Games.GetById(gameId);
            if (game == null)
            {
                return null;
            }

            int rateSum = 0;
            foreach (var rate in game.GameRates)
            {
                rateSum += rate.GameRate;
            }

            return new RateDto
            {
                AmountOfRates = game.GameRates.Count(),
                AvarageRate = rateSum / game.GameRates.Count()
            };

        }

        public async Task<int?> GetUserRate(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return null;
            }

            var rate = await _unitOfWork.Rates.GetUserRate(user.Id);
            if (rate == null)
            {
                return 0;
            }

            return rate.GameRate;
        }
    }
}
