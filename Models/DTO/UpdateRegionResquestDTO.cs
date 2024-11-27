using System.ComponentModel.DataAnnotations;

namespace NZWalks.Models.DTO
{
    public class UpdateRegionResquestDTO
    {
        [Required]
        [MinLength(3, ErrorMessage = "Code has to be minimum of 3 character")]
        [MaxLength(3, ErrorMessage = "Code has to be maximum of 3 character")]
        public string Code { get; set; }
        [Required]
        [MinLength(100, ErrorMessage = "Name has to be minimum of 100 character")]
        public string Name { get; set; }
        public string? RegionImgUrl { get; set; }
    }
}
