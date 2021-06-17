using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SmartBoat.Infrastructure.Helpers;
using SmartBoat.Infrastructure.Models.Identity;
using SmartBoat.Infrastructure.Settings;
using SmartBoat.Shared.Authentication;

namespace SmartBoat.Infrastructure.Services.Users
{
    public interface IUserService
    {
        Task<SignUpResponse> Register(RegisterRequest model);
        Task<LoginResponse> Login(LoginRequest model);
        Task<UserDataResponse> UserData(ClaimsPrincipal claimsPrincipal);
    }

    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMongoDbSettings _settings;

        public UserService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IMongoDbSettings settings)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _settings = settings;
        }

        public async Task<LoginResponse> Login(LoginRequest model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

            if (!result.Succeeded)
                throw new UnauthorizedAccessException("Bad Credentials");

            var appUser = _userManager.Users.SingleOrDefault(r => r.Email == model.Email);
            var token = AuthenticationHelper.GenerateJwtToken(model.Email, appUser, _settings);

            var rootData = new LoginResponse(token, appUser.UserName, appUser.Email);
            return rootData;
        }

        public async Task<SignUpResponse> Register(RegisterRequest model)
        {
            var user = new ApplicationUser { Name = model.Name, LastName = model.LastName, City = model.City, UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                throw new Exception(string.Join(",", result.Errors?.Select(error => error.Description)));

            await _signInManager.SignInAsync(user, false);
            var token = AuthenticationHelper.GenerateJwtToken(model.Email, user, _settings);

            var rootData = new SignUpResponse(token, user.UserName, user.Email);
            return rootData;
        }

        public async Task<UserDataResponse> UserData(ClaimsPrincipal claimsPrincipal)
        {
            var user = await _userManager.GetUserAsync(claimsPrincipal);
            var userData = new UserDataResponse
            {
                Name = user.Name,
                LastName = user.LastName,
                City = user.City,
                Email = user.Email,
                UserName = user.UserName
            };

            return userData;
        }
    }
}
