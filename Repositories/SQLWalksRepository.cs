using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NZWalks.Data;
using NZWalks.Models.Domain;

namespace NZWalks.Repositories
{
    public class SQLWalksRepository : IWalksRepository
    {
        private readonly NZWalksDbContext dbContext;

        public SQLWalksRepository(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Walk> CreateAsync(Walk walk)
        {
            await dbContext.Walks.AddAsync(walk);
            await dbContext.SaveChangesAsync();
            return walk;

        }

        

        public async Task<List<Walk>> GetAllWalks(string? fiterOn = null, string? fiterQuery = null, string? sortBy=null, bool Isassending=true, int Pagesize = 1000, int PageNumber = 1)
        {
            var walks = dbContext.Walks.Include("Difficulty").Include("Region").AsQueryable();
            //filtering
            if (string.IsNullOrWhiteSpace(fiterOn)==false && string.IsNullOrWhiteSpace(fiterQuery)==false)
            {
                if(fiterOn.Equals("Name",StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where(x => x.Name.Contains(fiterQuery));

                }
            }

            //Sorting
            if (string.IsNullOrWhiteSpace(sortBy) == false)
            {
                if(sortBy.Equals("Name",StringComparison.OrdinalIgnoreCase))
                {
                    walks = Isassending ? walks.OrderBy(x => x.Name) : walks.OrderByDescending(x => x.Name);
                }
                else if (sortBy.Equals("LengthInKm", StringComparison.OrdinalIgnoreCase))
                {
                    walks = Isassending ? walks.OrderBy(x => x.LengthInKm) : walks.OrderByDescending(x => x.LengthInKm);
                }
            }

            //pagination
            var skipPage = (PageNumber - 1) * Pagesize;
            return await walks.Skip(skipPage).Take(Pagesize).ToListAsync();

            //return await dbContext.Walks.Include("Difficulty").Include("Region").ToListAsync();
        }

        public async Task<Walk?> GetById(Guid id)
        {
            return await dbContext.Walks.Include("Difficulty").Include("Region").FirstOrDefaultAsync(x => x.Id == id);
        }


        public async Task<Walk?> Update(Guid id, Walk walk)
        {
            var checkExistWalk = await dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if (checkExistWalk == null)
            {
                return null;
            }
            checkExistWalk.Name = walk.Name;
            checkExistWalk.Description = walk.Description;
            checkExistWalk.Region = walk.Region;
            checkExistWalk.Description = walk.Description;
            checkExistWalk.LengthInKm = walk.LengthInKm;
            checkExistWalk.WalkImgUrl = walk.WalkImgUrl;

            await dbContext.SaveChangesAsync();
            return checkExistWalk;

        }
        public async Task<Walk> Delete(Guid id)
        {
            
            var result =await dbContext.Walks.FirstOrDefaultAsync(x=>x.Id == id);
            if(result == null)
            {
                return null;
            }
             dbContext.Walks.Remove(result);
            await dbContext.SaveChangesAsync();
            return result;
        }
    }
}
