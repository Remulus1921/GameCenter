using GameCenter.Data;
using GameCenter.Data.UnitOfWork;
using GameCenter.Dtos.RateDto;
using GameCenter.Models;
using Microsoft.AspNetCore.Identity;

namespace GameCenter.Core.Services.RatesService;

public class RatesService : IRatesService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<GameCenterUser> _userManager;

    public RatesService(IUnitOfWork unitOfWork, UserManager<GameCenterUser> userManager)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
    }

    public async Task<(int, string)> AddRate(RateSmallDto rate, Guid gameId, string email)
    {
        var game = await _unitOfWork.Games.GetById(gameId);
        if (game == null)
        {
            return (0, "Game not found");
        }

        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return (-1, "No user with claimed email");
        }

        var rateExists = await _unitOfWork.Rates.GetUserRate(user.Id, game.Id);
        if (rateExists != null)
        {
            return (-1, "User already rated this game");
        }

        var newRate = new Rate { GameRate = (int)rate.GameRate!, Game = game, User = user };
        await _unitOfWork.Rates.Add(newRate);
        await _unitOfWork.CompleteAsync();

        return (1, "Rate Successfully added");
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

    public async Task<RateSmallDto?> GetUserRate(string email, Guid gameId)
    {
        var game = await _unitOfWork.Games.GetById(gameId);
        if (game == null)
        {
            return null;
        }

        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return null;
        }

        var rate = await _unitOfWork.Rates.GetUserRate(user.Id, game.Id);
        if (rate == null)
        {
            return new RateSmallDto { GameRate = null };
        }

        return new RateSmallDto { GameRate = rate.GameRate };
    }

    public async Task<(int, string)> RemoveRate(Guid gameId, string email)
    {
        var game = await _unitOfWork.Games.GetById(gameId);
        if (game == null)
        {
            return (0, "Game not found");
        }

        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return (-1, "No user with claimed email");
        }

        var rateExists = await _unitOfWork.Rates.GetUserRate(user.Id, game.Id);
        if (rateExists == null)
        {
            return (-1, "User never rated this game");
        }

        await _unitOfWork.Rates.Delete(rateExists);
        await _unitOfWork.CompleteAsync();

        return (1, "Rate successfully deleted");
    }

    public async Task<(int, string)> UpdateRate(RateSmallDto rate, Guid gameId, string email)
    {
        var game = await _unitOfWork.Games.GetById(gameId);
        if (game == null)
        {
            return (0, "Game not found");
        }

        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return (-1, "No user with claimed email");
        }

        var rateExists = await _unitOfWork.Rates.GetUserRate(user.Id, game.Id);
        if (rateExists == null)
        {
            return (-1, "User never rated this game");
        }

        rateExists.GameRate = (int)rate.GameRate!;
        await _unitOfWork.Rates.Update(rateExists);
        await _unitOfWork.CompleteAsync();

        return (1, "Rate Successfully updated");
    }

    private async Task<(int, string)> ValidateRate(string email, Guid gameId)
    {
        var game = await _unitOfWork.Games.GetById(gameId);
        if (game == null)
        {
            return (0, "Game not found");
        }

        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return (-1, "No user with claimed email");
        }

        var rateExists = await _unitOfWork.Rates.GetUserRate(user.Id, game.Id);
        if (rateExists != null)
        {
            return (-1, "User already rated this game");
        }

        return (1, "Validation passed");
    }
}
