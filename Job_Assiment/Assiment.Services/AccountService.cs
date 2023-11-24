using Assiment.core.Models;
using Assiment.DTO;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Assiment.EF.Services
{
    public class AccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;


        public AccountService( UserManager<ApplicationUser> userManager,IConfiguration config) {
            _userManager = userManager;
        }

        public async Task<IdentityResult> ChangePasswordAsync(NewPassworedDto newPasswored)
        {

            ApplicationUser user = await _userManager.FindByEmailAsync(newPasswored.Email);

            if (user != null)
            {
                PasswordHasher<ApplicationUser> passwordHasher = new PasswordHasher<ApplicationUser>();
                var newPasswordHash = passwordHasher.HashPassword(user, newPasswored.newPassword);

                user.PasswordHash = newPasswordHash;
                var result = await _userManager.UpdateAsync(user);

                return result;
            }
            return IdentityResult.Failed(new IdentityError { Description = "User not found." });

        }

        public async Task<IdentityResult> RegisterationAsync(RegisterDto registerDto)
        {

            ApplicationUser userModel = new ApplicationUser();
            userModel.Email = registerDto.Email;
            userModel.UserName = registerDto.UserName;
            userModel.City = registerDto.City;
            userModel.Country = registerDto.Country;
            userModel.gender = registerDto.gender;
            if (registerDto.Password == registerDto.ConfirmePassword)
            {
                IdentityResult result = await _userManager.CreateAsync(userModel, registerDto.Password);
                return result;
               
            }
            return IdentityResult.Failed(new IdentityError { Description = "The ConfirmedPassword and Password Not Match" });

        }



    }
}
