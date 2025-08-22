// Ignore Spelling: validator Auth HRM Api

using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Configurations.Validation;
using TodoApp.Domain.Entities;
using TodoApp.Models.Auth;
using TodoApp.Models.Common;
using TodoApp.Utiities.Exceptions;

namespace TodoApp.Controllers
{
    [Route("api/auth")]
    [ApiController]
    [Tags("Auth")]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _validate;
        private readonly UserManager<User> _userManager;
        private readonly IPasswordHasher<User> _hasher;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        public AuthController(
            IMediator validate,
            UserManager<User> userManager,
            IPasswordHasher<User> hasher,
            IMapper mapper,
            ITokenService tokenService)
        {
            _validate = validate;
            _userManager = userManager;
            _hasher = hasher;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<TokenModel>>> Login([FromBody] LoginDto model)
        {

            ValidationResult validationResult = await _validate.ValidateAsync(model);

            if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user is null) throw new NotFoundException("User Not Found!");

            if (_hasher.VerifyHashedPassword(user, user.PasswordHash ?? "", model.Password) == PasswordVerificationResult.Failed)
            {
                throw new BadRequestException("Credentials are not valid!");
            }

            var token = await _tokenService.GetTokens(user);
            return Ok(new ApiResponse<TokenModel>(token));
        }

        [HttpPost("signup")]
        public async Task<ActionResult<ApiResponse<string>>> CreateUser(UserCreateDto model)
        {

            ValidationResult validationResult = await _validate.ValidateAsync(model);

            if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);

            User user = _mapper.Map<User>(model);
            user.UserName = model.Email;

            IdentityResult result = await _userManager.CreateAsync(user, model.Password);

            return result.Succeeded ? Ok(new ApiResponse<string>("Successfully Created!"))
                                    : throw new BadRequestException("User creation failed due to identity errors.");
        }

    }

}
