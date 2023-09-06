using GameCenter.Core.Repositories.GenericRepository;
using GameCenter.Data;
using GameCenter.Models;
using Microsoft.EntityFrameworkCore;

namespace GameCenter.Core.Repositories.PlatformRepository
{

    public class PlatformsRepository : GenericRepository<Platform>, IPlatformsRepository
    {
        public PlatformsRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Platform>> All()
        {
            try
            {
                return await _context.Platforms.ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Platform?> GetByName(string name)
        {
            try
            {
                return await _context.Platforms.Where(p => p.PlatformName == name).FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
