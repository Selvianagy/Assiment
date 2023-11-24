using Assiment.core.Models;
using Assiment.DTO;
using Assiment.EF.Services;
using ECommerce.Data.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Assiment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration config;
        private UnitOfWork UnitOfWork;
        private AccountService _accountService;


        public AccountController(UserManager<ApplicationUser> userManager, IConfiguration config 
            ,UnitOfWork unitOfWork
            ,AccountService accountService)
        {
            this.userManager = userManager;
            this._accountService = accountService;
            this.config = config;
            this.UnitOfWork = unitOfWork;
        }


        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword(NewPassworedDto newPassowrd)
        {
            
            if (ModelState.IsValid)
            {
                
                    var result = await _accountService.ChangePasswordAsync(newPassowrd);

                    if (result.Succeeded)
                    {
                        UnitOfWork.CommitChanges();
                        return Ok("Update Passwoed Success");
                    }
                    else
                    {
                        return BadRequest(result.Errors.First());

                    }
              

            }
            return BadRequest(ModelState);

        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (ModelState.IsValid)
            {
                 IdentityResult result = await _accountService.RegisterationAsync(registerDto);
                if (result.Succeeded)
                {
                    UnitOfWork.CommitChanges();
                    return Ok("Created Success");
                }
                else
                {
                    return BadRequest(result.Errors.First());
                }
              
            }
            return BadRequest(ModelState);
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser userModel = await userManager.FindByEmailAsync(loginDto.Email);
                if (userModel != null && await userManager.CheckPasswordAsync(userModel, loginDto.Password))
                {
                    List<Claim> userClaims = new List<Claim>();
                    userClaims.Add(new Claim(ClaimTypes.NameIdentifier, userModel.Id));
                    userClaims.Add(new Claim(ClaimTypes.Name, userModel.UserName));
                    userClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

                   
                    var authSecritKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:SecretKey"]));

                    SigningCredentials credentials =
                        new SigningCredentials(authSecritKey, SecurityAlgorithms.HmacSha256);

                    JwtSecurityToken mytoken = new JwtSecurityToken(
                        issuer: config["JWT:ValidIss"],
                        audience: config["JWT:ValidAud"],
                        expires: DateTime.Now.AddHours(5),
                        claims: userClaims,
                        signingCredentials: credentials
                        );

                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(mytoken),
                        expiration = mytoken.ValidTo
                    });

                }
                return BadRequest("Invalid Login Account");
            }
            return BadRequest(ModelState);
        }
    }
}
