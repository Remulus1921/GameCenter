using GameCenter.Data.UnitOfWork;
using GameCenter.Dtos.CommentDtos;
using GameCenter.Dtos.FileDtos;
using GameCenter.Dtos.GameDto;
using GameCenter.Models;

namespace GameCenter.Core.Services.GameService
{
    public class GamesService : IGamesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly string _uploadsFolder;

        public GamesService(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _uploadsFolder = Path.Combine(webHostEnvironment.ContentRootPath, "Resources/Images");
        }

        public async Task<bool> AddGame(GameAddUpdateDto game)
        {
            string uniqueName = this.AddFile(game.Image);

            var gamePlatforms = new List<Platform>();

            if (game.Platforms != null)
            {
                foreach (var platformName in game.Platforms)
                {
                    var platform = await _unitOfWork.Platforms.GetByName(platformName);
                    if (platform == null)
                        continue;
                    gamePlatforms.Add(platform);
                }
            }

            Game newGame = new Game
            {
                Name = game.Name,
                GameType = game.GameType,
                Description = game.Description,
                Studio = game.Studio,
                Rating = game.Rating,
                Capacity = game.Capacity,
                ImageName = uniqueName,
                Platforms = gamePlatforms,
            };

            await _unitOfWork.Games.Add(newGame);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        public async Task<bool> DeleteGame(Guid id)
        {
            var game = await _unitOfWork.Games.GetById(id);
            if (game == null)
            {
                return false;
            }

            this.DeleteFile(game.ImageName);

            await _unitOfWork.Games.Delete(game);
            await _unitOfWork.CompleteAsync();

            return true;
        }


        public async Task<GameDto?> GetGameById(Guid id)
        {
            var game = await _unitOfWork.Games.GetById(id);
            if (game == null)
            {
                return null;
            }

            FileDto file = this.FindFile(game.ImageName);

            var gameDto = new GameDto
            {
                Id = game.Id,
                Name = game.Name,
                GameType = game.GameType,
                Description = game.Description,
                Studio = game.Studio,
                Capacity = game.Capacity,
                Image = file,
                Rating = game.Rating,
                PlatformsName = game.Platforms.Select(p => p.PlatformName).ToList(),
                GameRates = game.GameRates?.Select(r => r.GameRate).ToList(),
                Comments = game.GameComments?.Select(c => new CommentDto
                {
                    Id = c.Id,
                    UserName = c.User.UserName,
                    UserEmail = c.User.Email,
                    CreationDate = c.CreationDate,
                    ModificationDate = c.ModificationDate,
                    CommentContent = c.CommentContent,
                    ParentId = c.ParentId
                }).ToList(),
            };

            return gameDto;
        }

        public async Task<List<GameSmallDto>> GetGames()
        {
            var games = await _unitOfWork.Games.All();
            var gamesDtoList = new List<GameSmallDto>();

            foreach (var game in games)
            {
                FileDto file = this.FindFile(game.ImageName);

                gamesDtoList.Add(new GameSmallDto
                {
                    Id = game.Id,
                    Name = game.Name,
                    GameType = game.GameType,
                    Rating = game.Rating,
                    Image = file,
                });
            }

            return gamesDtoList;
        }

        public async Task<bool> UpdateGame(GameAddUpdateDto game, Guid gameId)
        {
            var gameExists = await _unitOfWork.Games.GetById(gameId);

            if (gameExists == null)
            {
                return false;
            }

            var gamePlatforms = new List<Platform>();

            foreach (var platformName in game.Platforms)
            {
                var platform = await _unitOfWork.Platforms.GetByName(platformName);
                if (platform == null)
                    continue;
                gamePlatforms.Add(platform);
            }

            this.DeleteFile(gameExists.ImageName);
            string uniqueName = this.AddFile(game.Image);

            gameExists.Name = game.Name;
            gameExists.GameType = game.GameType;
            gameExists.Rating = game.Rating;
            gameExists.Description = game.Description;
            gameExists.Studio = game.Studio;
            gameExists.Capacity = game.Capacity;
            gameExists.ImageName = uniqueName;
            gameExists.Platforms = gamePlatforms;

            await _unitOfWork.Games.Update(gameExists);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        private void DeleteFile(string fileName)
        {
            string[] matchingFiles = Directory.GetFiles(_uploadsFolder, fileName + ".jpg");

            if (matchingFiles.Any())
            {
                File.Delete(matchingFiles.First());
            }
            else
            {
                throw new Exception($"File with provided file name: \"{fileName}\" doesnt exist");
            }
        }

        private string AddFile(IFormFile file)
        {
            string uniqueName = Guid.NewGuid().ToString();

            string path = Path.Combine(_uploadsFolder, uniqueName + Path.GetExtension(file.FileName));

            using (var stream = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return uniqueName;
        }

        private FileDto FindFile(string fileName)
        {
            string[] matchingFile = Directory.GetFiles(_uploadsFolder, fileName + ".jpg");

            if (matchingFile.Any())
            {
                byte[] fileContent = File.ReadAllBytes(matchingFile.First());
                string content = Convert.ToBase64String(fileContent);

                FileDto file = new FileDto()
                {
                    FileContent = content,
                    FileName = fileName,
                    FileType = "image/jpeg",
                };
                return file;
            }
            else
            {
                throw new Exception($"File with provided file name: \"{fileName}\" doesnt exist");
            }
        }
    }
}
