using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.Models.Domain;
using NZWalks.Models.DTO;
using NZWalks.Repositories;

namespace NZWalks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }
        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload([FromForm] ImageUploadrequestDTO imageUploadrequestDTO)
        {
            ValidateFileUpload(imageUploadrequestDTO);

            if(ModelState.IsValid)
            {
                //convert DTO to Domain Model
                var ImageDomainModel = new Image
                {
                    File=imageUploadrequestDTO.File,
                    FileExtension = Path.GetExtension(imageUploadrequestDTO.File.FileName),
                    FileSizeInBytes= imageUploadrequestDTO.File.Length,
                    FileName = imageUploadrequestDTO.File.FileName,
                    FileDescription = imageUploadrequestDTO.FileDescription

                };



                //User repository to upload image
                await imageRepository.Upload(ImageDomainModel);
                return Ok(ImageDomainModel);
            }
            return BadRequest(ModelState);
        }
            
        private void ValidateFileUpload (ImageUploadrequestDTO imageUploadrequestDTO)
        {
            var allowExtensions = new string[] { ".jpg", ".jpeg", ".png","JPG","JPEG","PNG" };

            if (allowExtensions.Contains(Path.GetExtension(imageUploadrequestDTO.File.FileName))==false)
            {
                ModelState.AddModelError("file", "Unsupported File Extension");
            }

            if (imageUploadrequestDTO.File.Length > 10485760)
            {
                ModelState.AddModelError("file", "File size mire than 10MB,PLease upload a smaller size file");
            }
        }
    }
}
    