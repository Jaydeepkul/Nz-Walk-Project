namespace NZWalks.Models.Domain
{
    public class Walk
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public double LengthInKm { get; set; }
        public string? WalkImgUrl { get; set; }

        public Guid DifficultyId { get; set; }
        public Guid RegionId { get; set; }

        //Navaigation Properties
        public Difficulty Difficulty { get; set; }
        public Region Region { get; set; }
    }
}
