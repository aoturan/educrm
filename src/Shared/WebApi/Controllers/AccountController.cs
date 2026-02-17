using EduCrm.Modules.Account.Application.Security;
using EduCrm.Modules.Account.Application.UseCases.ChangePassword;
using EduCrm.Modules.Account.Application.UseCases.GetMe;
using EduCrm.Modules.Account.Application.UseCases.Login;
using EduCrm.Modules.Account.Application.UseCases.Register;
using EduCrm.Modules.Account.Application.UseCases.UpdateProfile;
using EduCrm.SharedKernel.Abstractions;
using EduCrm.WebApi.Contracts.Account;
using EduCrm.WebApi.Extensions;
using EduCrm.WebApi.Validations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduCrm.WebApi.Controllers;


[ApiController]
[Route("api/account")]
public sealed class AccountController : ControllerBase
{
    private readonly IRegisterService _register;
    private readonly ILoginService _login;
    private readonly IGetMeService _getMe;
    private readonly IChangePasswordService _changePassword;
    private readonly IUpdateProfileService _updateProfile;
    private readonly ICurrentUser _currentUser;
    private readonly IOrgContext _orgContext;
    private readonly IPasswordHasher _hasher;
    private readonly IRequestValidator _validator;
    
    public AccountController(
        IRegisterService register, 
        ILoginService login,
        IGetMeService getMe,
        IChangePasswordService changePassword,
        IUpdateProfileService updateProfile,
        ICurrentUser currentUser,
        IOrgContext orgContext,
        IPasswordHasher hasher, 
        IRequestValidator validator)
    {
        _register = register;
        _login = login;
        _getMe = getMe;
        _changePassword = changePassword;
        _updateProfile = updateProfile;
        _currentUser = currentUser;
        _orgContext = orgContext;
        _hasher = hasher;
        _validator = validator;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register(
        [FromBody] RegisterRequest req,
        CancellationToken ct)
    {
        var validation = await _validator.ValidateAsync(req, ct);
        if (!validation.IsValid) return validation.ToValidationProblem(this);
        
        // 1) hash (Phase 1)
        var passwordHash = _hasher.Hash(req.Password);

        // 2) map -> input
        var input = new RegisterInput(
            req.Name,
            req.Email,
            req.OrganizationName,
            passwordHash,
            req.Phone
        );

        // 3) call
        var result = await _register.RegisterAsync(input, ct);

        return result.ToActionResult(HttpContext, this, r => 
            Ok(new RegisterResponse(r.Token, r.Email, r.FullName, r.Initials, r.OrganizationName)));
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest req,
        CancellationToken ct)
    {
        var validation = await _validator.ValidateAsync(req, ct);
        if (!validation.IsValid) return validation.ToValidationProblem(this);

        // 1) map -> input
        var input = new LoginInput(req.Email, req.Password);

        // 2) call
        var result = await _login.LoginAsync(input, ct);

        return result.ToActionResult(HttpContext, this, r => 
            Ok(new LoginResponse(r.Token, r.Email, r.FullName, r.Initials, r.OrganizationName)));
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> Me(CancellationToken ct)
    {
        var userId = _currentUser.UserId;
        if (userId is null)
        {
            return Unauthorized();
        }

        var result = await _getMe.GetMeAsync(userId.Value, ct);

        return result.ToActionResult(HttpContext, this, r => 
            Ok(new MeResponse(r.Email, r.FullName, r.Initials, r.OrganizationName)));
    }

    [HttpPost("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword(
        [FromBody] ChangePasswordRequest req,
        CancellationToken ct)
    {
        var validation = await _validator.ValidateAsync(req, ct);
        if (!validation.IsValid) return validation.ToValidationProblem(this);

        var userId = _currentUser.UserId;
        if (userId is null)
        {
            return Unauthorized();
        }

        // Hash new password
        var newPasswordHash = _hasher.Hash(req.NewPassword);

        var input = new ChangePasswordInput(
            userId.Value,
            req.OldPassword,
            newPasswordHash);

        var result = await _changePassword.ChangePasswordAsync(input, ct);

        return result.ToActionResult(HttpContext, this);
    }

    [HttpPost("profile")]
    [Authorize]
    public async Task<IActionResult> UpdateProfile(
        [FromBody] UpdateProfileRequest req,
        CancellationToken ct)
    {
        var validation = await _validator.ValidateAsync(req, ct);
        if (!validation.IsValid) return validation.ToValidationProblem(this);

        var userId = _currentUser.UserId;
        var orgId = _orgContext.OrganizationId;
        
        if (userId is null || orgId is null)
        {
            return Unauthorized();
        }

        var input = new UpdateProfileInput(
            userId.Value,
            orgId.Value,
            req.FullName,
            req.Email,
            req.OrganizationName);

        var result = await _updateProfile.UpdateProfileAsync(input, ct);

        return result.ToActionResult(HttpContext, this, r => 
            Ok(new MeResponse(r.Email, r.FullName, r.Initials, r.OrganizationName)));
    }
}