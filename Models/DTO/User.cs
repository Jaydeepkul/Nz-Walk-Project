using System.ComponentModel.DataAnnotations;
using System.Security.Principal;

namespace NZWalks.Models.DTO
{
    public class User
    {
       
        public string UserName { get; set; }
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
