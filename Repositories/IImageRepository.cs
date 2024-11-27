using NZWalks.Models.Domain;
using System.Net;

namespace NZWalks.Repositories
{
    public interface IImageRepository
    {
       Task<Image> Upload(Image image);
    }
}
