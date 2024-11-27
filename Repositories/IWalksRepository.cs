using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.Models.Domain;

namespace NZWalks.Repositories
{
    public interface IWalksRepository
    {
        Task<Walk> CreateAsync(Walk walk);
        Task<List<Walk>> GetAllWalks(string? fiterOn=null,string? fiterQuery=null, string? sortBy=null, bool Isassending = true, int Pagesize = 1000,  int PageNumber = 1);
       Task<Walk?> GetById(Guid id);
        Task<Walk?> Update(Guid id, Walk walk);
       Task<Walk> Delete(Guid id);
    }
}
