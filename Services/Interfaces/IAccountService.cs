using System;
using ClassicCars.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace ClassicCars.Services.Interfaces
{
	public interface IAccountService
	{
        Task<IdentityResult> RegisterAsync(RegisterViewModel model);
        Task<SignInResult> LoginAsync(LoginViewModel model);
        Task LogoutAsync();
    }
}

