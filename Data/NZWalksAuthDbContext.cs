using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NZWalks.Models.Domain;
using NZWalks.Models.DTO;

namespace NZWalks.Data
{
    public class NZWalksAuthDbContext : IdentityDbContext
    {
        //If you are using more than one connection string in your project then you have to specify
        //which Dbcontext you are using in specific time being.For that you have to use "DbcontectOption<NameOfThatClass/Dbcontext>".
        public NZWalksAuthDbContext(DbContextOptions<NZWalksAuthDbContext> options) : base(options)
        {
        }
        //public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var ReaderRoleId = "bf70f220-8cbb-4bd1-8570-2ca165da139a";
            var WriterRoleId = "a9acc078-b0a6-4c74-a1ab-2922387e5136";

            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = ReaderRoleId,
                    ConcurrencyStamp = ReaderRoleId,
                    Name = "Reader",
                    NormalizedName = "Reader".ToUpper()
                },
                new IdentityRole
                {
                    Id = WriterRoleId,
                    ConcurrencyStamp = WriterRoleId,
                    Name = "Writer",
                    NormalizedName = "Writer".ToUpper()
                }
            };
            //Seed the defined data to database Using HasData
            builder.Entity<IdentityRole>().HasData(roles);
            //After this, Next Perform Migration.

            //Note:
            //While performing migration you have to specify which Dbcontext need to excute
            //(this will do when you have more than one Dbcontext)
            //for that command is "Add-Migration "Creating Auth Database" -Context "NZWalksAuthDbContext""


            // for Update database use "Update-Database -Context "NZWalksAuthDbContext"

        }

    }
}
