using NZWalks.Data;
using NZWalks.Models.Domain;

namespace NZWalks.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly NZWalksDbContext nZWalksDbContext;
        private readonly IHttpContextAccessor httpContextAccessor;

        public ImageRepository(IWebHostEnvironment webHostEnvironment,NZWalksDbContext nZWalksDbContext,IHttpContextAccessor httpContextAccessor)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.nZWalksDbContext = nZWalksDbContext;
            this.httpContextAccessor = httpContextAccessor;
        }
        public async Task<Image> Upload(Image image)
        {
            var LocalFilePath = Path.Combine(webHostEnvironment.ContentRootPath, "Images", $"{image.FileName}{image.FileExtension}");

            //Upload Image to Local Path
            using var stream = new FileStream(LocalFilePath, FileMode.Create);
            await image.File.CopyToAsync(stream);

            //https://localhost:1234/images.jpg
            var urlfilePath = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}{httpContextAccessor.HttpContext.Request.PathBase}/Images/{image.FileName}{image.FileExtension}";

            image.FilePath = urlfilePath;


            //Add the image to Image table
            await nZWalksDbContext.Images.AddAsync(image);
            await nZWalksDbContext.SaveChangesAsync();

            return image;

        }
    }
}
