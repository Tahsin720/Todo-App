//Ignore Spelling: validator Auth HRM Api
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TodoApp.Configurations.Validation;
using TodoApp.Domain.Entities;
using TodoApp.Models.Auth;
using TodoApp.Models.Common;
using TodoApp.Utiities.Exceptions;

namespace TodoApp.Controllers
{
    [Route("api/auth/refresh-token")]
    [ApiController]
    [Tags("Auth")]
    public class RefreshTokenController : ControllerBase
    {
        private readonly IMediator _validate;
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;
        public RefreshTokenController(IMediator validate, UserManager<User> userManager,
             ITokenService tokenService)
        {
            _validate = validate;
            _userManager = userManager;
            _tokenService = tokenService;
        }
        [HttpPost]
        public async Task<ActionResult<ApiResponse<TokenModel>>> Post([FromBody] TokenModel tokenData)
        {
            var validationResult = await _validate.ValidateAsync(tokenData);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            string userId = _tokenService.GetUserNameFromToken(tokenData.AccessToken) ?? "";

            User user = await _userManager.FindByIdAsync(userId) ?? new User();

            if (user is null
                || string.IsNullOrEmpty(user.RefreshToken)
                || !user.RefreshToken.Equals(tokenData.RefreshToken, StringComparison.Ordinal)
                || DateTime.UtcNow > user.RefreshTokenExpiresAt)
            {
                throw new BadRequestException("Please, try logging in again!");
            }

            var token = await _tokenService.GetTokens(user);
            return Ok(new ApiResponse<TokenModel>(token));
        }
    }
}
