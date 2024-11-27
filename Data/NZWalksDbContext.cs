using Microsoft.EntityFrameworkCore;
using NZWalks.Models.Domain;

namespace NZWalks.Data
{
    public class NZWalksDbContext: DbContext
    {
        //If you are using more than one connection string in your project then you have to specify
        //which Dbcontext you are using in specific time being.For that you have to use "DbcontectOption<NameOfThatClass/Dbcontext>".
        public NZWalksDbContext(DbContextOptions<NZWalksDbContext> dbContextOptions):base(dbContextOptions) 
        {
                
        }

        public DbSet<Difficulty> Difficulties { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Walk> Walks { get; set; }

        public DbSet<Image> Images { get; set; }

        //seeding
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //seed the difficulty model 
            //Easy, Medium, Hard

            //Dedine the data for seeding
            var difficulties = new List<Difficulty>
            {
                new Difficulty()
                {
                    Id = Guid.Parse("5816bfcf-d130-49ef-8b48-bc574a93f257"),
                    Name ="Easy"
                },
                new Difficulty()
                {
                    Id = Guid.Parse("1a438f00-9979-4ed9-8939-ef3c9743173b"),
                    Name ="Medium"
                },
                new Difficulty()
                {
                    Id = Guid.Parse("e0da852d-4ef3-46cb-b4d1-74219f74cd70"),
                    Name ="Hard"
                }
            };

            //Seed the defined data to database Using HasData
            modelBuilder.Entity<Difficulty>().HasData(difficulties);


            // Seed data for Regions
            var regions = new List<Region>
            {
                new Region
                {
                    Id = Guid.Parse("f7248fc3-2585-4efb-8d1d-1c555f4087f6"),
                    Name = "Auckland",
                    Code = "AKL",
                    RegionImgUrl = "https://images.pexels.com/photos/5169056/pexels-photo-5169056.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
                },
                new Region
                {
                    Id = Guid.Parse("6884f7d7-ad1f-4101-8df3-7a6fa7387d81"),
                    Name = "Northland",
                    Code = "NTL",
                    RegionImgUrl = null
                },
                new Region
                {
                    Id = Guid.Parse("14ceba71-4b51-4777-9b17-46602cf66153"),
                    Name = "Bay Of Plenty",
                    Code = "BOP",
                    RegionImgUrl = null
                },
                new Region
                {
                    Id = Guid.Parse("cfa06ed2-bf65-4b65-93ed-c9d286ddb0de"),
                    Name = "Wellington",
                    Code = "WGN",
                    RegionImgUrl = "https://images.pexels.com/photos/4350631/pexels-photo-4350631.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
                },
                new Region
                {
                    Id = Guid.Parse("906cb139-415a-4bbb-a174-1a1faf9fb1f6"),
                    Name = "Nelson",
                    Code = "NSN",
                    RegionImgUrl = "https://images.pexels.com/photos/13918194/pexels-photo-13918194.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
                },
                new Region
                {
                    Id = Guid.Parse("f077a22e-4248-4bf6-b564-c7cf4e250263"),
                    Name = "Southland",
                    Code = "STL",
                    RegionImgUrl = null
                },
            };
            modelBuilder.Entity<Region>().HasData(regions);

            // At the end, we have to perform migrations

        }
    }
}
