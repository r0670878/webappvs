using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using webappvs.Helpers;
using webappvs.Models;

namespace webappvs.Controllers.api
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly AppSettings _appSettings;

        public UserController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration,
            IOptions<AppSettings> appSettings
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _appSettings = appSettings.Value;
        }


        //[AllowAnonymous]
        //[HttpPost("authenticate")]
        //public async Task<Object> Authenticate([FromBody] AuthenticateModel model)
        //{
        //    var result = await _signInManager.PasswordSignInAsync(model.Gebruikersnaam, model.Wachtwoord, false, false);

        //    if (!result.Succeeded)
        //    {
        //        return BadRequest(new { message = "Username or password is incorrect" });
        //    }

        //    ApplicationUser appUser = _userManager.Users.SingleOrDefault(r => r. == model.Gebruikersnaam);
        //    model.Token = GenerateJwtToken(appUser).ToString();
        //    model.Id = appUser.Id.ToString();
        //    return model;

        //}

        //private string GenerateJwtToken(ApplicationUser user)
        //{
        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
        //    var tokenDescriptor = new SecurityTokenDescriptor
        //    {
        //        Subject = new ClaimsIdentity(new Claim[]
        //        {
        //            new Claim(ClaimTypes.Name, user.Id.ToString())
        //        }),
        //        Expires = DateTime.UtcNow.AddDays(7),
        //        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        //    };
        //    var token = tokenHandler.CreateToken(tokenDescriptor);

        //    return tokenHandler.WriteToken(token);
        //}

    }
}
