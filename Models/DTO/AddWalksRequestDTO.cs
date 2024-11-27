using System.ComponentModel.DataAnnotations;

namespace NZWalks.Models.DTO
{
    public class AddWalksRequestDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [Range(0,50)]
        public double LengthInKm { get; set; }
        public string? WalkImgUrl { get; set; }
        [Required]
        public Guid DifficultyId { get; set; }
        [Required]
        public Guid RegionId { get; set; }
    }
}
