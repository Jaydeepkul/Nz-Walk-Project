using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using NZWalks.Data;
using NZWalks.Models.Domain;

namespace NZWalks.Repositories
{
    public class SQLRegionRepository : IRegionRepository
    {
        private readonly NZWalksDbContext dbContext;

        public SQLRegionRepository(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Region> Create(Region region)
        {
            await dbContext.Regions.AddAsync(region);
            await dbContext.SaveChangesAsync();
            return region;
        }

        

        public async Task<List<Region>> GetAllAsync()
        {
          return await dbContext.Regions.ToListAsync();
        }

        public async Task<Region?> GetById(Guid id)
        {
          return await dbContext.Regions.FirstOrDefaultAsync(x=>x.Id==id);
        }

        public async Task<Region?> Update(Guid id, Region region)
        {
            var existingRegion = await dbContext.Regions.FirstOrDefaultAsync(x=>x.Id==id);
            if (existingRegion!=null)
            {
                return null;
            }
            existingRegion.Code = region.Code;
            existingRegion.Name = region.Name;
            existingRegion.RegionImgUrl = region.RegionImgUrl;

            await dbContext.SaveChangesAsync();
            return existingRegion;
        }

        public async Task<Region?> Delete(Guid id)
        {
            var result = await dbContext.Regions.FirstOrDefaultAsync(x=>x.Id == id);
            if (result==null)
            {
                return null;
            }
            dbContext.Regions.Remove(result);
            await dbContext.SaveChangesAsync();
            return result;
        }

    }
}
