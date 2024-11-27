using NZWalks.Models.Domain;

namespace NZWalks.Models.DTO
{
    public class WalkDto
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public double LengthInKm { get; set; }
        public string? WalkImgUrl { get; set; }

        //public Guid DifficultyId { get; set; }
        //public Guid RegionId { get; set; }

        //Navigatiion property
        public DifficultyDTO Difficulty { get; set; }
        public RegionDTO Region { get; set; }
    }
}
