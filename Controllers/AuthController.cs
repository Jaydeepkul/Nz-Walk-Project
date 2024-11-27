using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.Data;
using NZWalks.Models.DTO;
using NZWalks.Repositories;

namespace NZWalks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;
        private readonly NZWalksAuthDbContext nZWalksAuthDbContext;

        public AuthController(UserManager<IdentityUser> userManager,ITokenRepository tokenRepository,NZWalksAuthDbContext nZWalksAuthDbContext)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
            this.nZWalksAuthDbContext = nZWalksAuthDbContext;
        }

        //HTTPPOST/AUTH/REGISTER
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(RegisterRequestDTO registerRequestDTO)
        {
            var identityUser = new IdentityUser
            {
                UserName = registerRequestDTO.UserName,
                Email = registerRequestDTO.UserName
            };
          var RegisterResult= await userManager.CreateAsync(identityUser, registerRequestDTO.Password);
            if(RegisterResult.Succeeded)
            {
                //Add the role to this user
                if(registerRequestDTO.Roles !=null && registerRequestDTO.Roles.Any())
                {
                    RegisterResult= await userManager.AddToRolesAsync(identityUser, registerRequestDTO.Roles);
                    if (RegisterResult.Succeeded)
                    {
                        return Ok("Register User Successfully, Please Login!");
                    }
                }
                              
            }
            return BadRequest("Something went wrong!");
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody]LoginRequestDto loginRequestDto)
        {
            var userResult = await userManager.FindByEmailAsync(loginRequestDto.Username);
            if(userResult != null)
            {
                var passwordResult = await userManager.CheckPasswordAsync(userResult, loginRequestDto.Password);
                if (passwordResult != null)
                {
                    var roles = await userManager.GetRolesAsync(userResult);
                    if (roles != null)
                    {
                        //create Token
                        var JwtToken = tokenRepository.CreateJWTToken(userResult, roles.ToList());

                        var response = new LoginResponseDto
                        {
                            JwtToken = JwtToken,
                        };

                        return Ok(response);
                    }
                   
                }
            }
            return BadRequest("Invalid Username or Password");
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUser() 
        {
           var result = await userManager.Users.Select(u => new User
           {
              
               UserName = u.UserName,
               Email = u.Email,
               Password = u.PasswordHash
               // Add other properties you want to include
           }).ToListAsync();



            return Ok(result);
           
        }

    }
}
