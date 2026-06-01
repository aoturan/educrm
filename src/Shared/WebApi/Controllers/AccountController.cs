using EduCrm.Modules.Account.Application.Security;
using EduCrm.Modules.Account.Application.UseCases.ChangePassword;
using EduCrm.Modules.Account.Application.UseCases.ChangeUserPasswordByAdmin;
using EduCrm.Modules.Account.Application.UseCases.ChangeUserStatus;
using EduCrm.Modules.Account.Application.UseCases.CreatePaymentNotification;
using EduCrm.Modules.Account.Application.UseCases.CreateSubscriptionRequest;
using EduCrm.Modules.Account.Application.UseCases.CreateUser;
using EduCrm.Modules.Account.Application.UseCases.GetBankAccounts;
using EduCrm.Modules.Account.Application.UseCases.GetBillingDetail;
using EduCrm.Modules.Account.Application.UseCases.GetMe;
using EduCrm.Modules.Account.Application.UseCases.GetNotifications;
using EduCrm.Modules.Account.Application.UseCases.GetOrganization;
using EduCrm.Modules.Account.Application.UseCases.GetPlanPricing;
using EduCrm.Modules.Account.Application.UseCases.GetPlanUsage;
using EduCrm.Modules.Account.Application.UseCases.ListUsers;
using EduCrm.Modules.Account.Application.UseCases.Login;
using EduCrm.Modules.Account.Application.UseCases.Register;
using EduCrm.Modules.Account.Application.UseCases.RequestPasswordReset;
using EduCrm.Modules.Account.Application.UseCases.ResendEmailVerification;
using EduCrm.Modules.Account.Application.UseCases.ResetPassword;
using EduCrm.Modules.Account.Application.UseCases.TransferAdminRole;
using EduCrm.Modules.Account.Application.UseCases.VerifyEmail;
using EduCrm.Modules.Account.Domain.Enums;
using EduCrm.Modules.Account.Application.UseCases.UpdateOrganization;
using EduCrm.Modules.Account.Application.UseCases.UpdateProfile;
using EduCrm.Modules.Account.Application.UseCases.UpdateUserByAdmin;
using EduCrm.Modules.Account.Application.UseCases.UpsertBillingDetail;
using EduCrm.Infrastructure.Turnstile;
using EduCrm.SharedKernel.Abstractions;
using EduCrm.WebApi.Helpers;
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
    private readonly IGetNotificationsService _getNotifications;
    private readonly IGetOrganizationService _getOrganization;
    private readonly IUpdateOrganizationService _updateOrganization;
    private readonly IChangePasswordService _changePassword;
    private readonly IRequestPasswordResetService _requestPasswordReset;
    private readonly IResetPasswordService _resetPassword;
    private readonly IVerifyEmailService _verifyEmail;
    private readonly IResendEmailVerificationService _resendEmailVerification;
    private readonly IUpdateProfileService _updateProfile;
    private readonly ICreateUserService _createUser;
    private readonly IChangeUserStatusService _changeUserStatus;
    private readonly IChangeUserPasswordByAdminService _changeUserPasswordByAdmin;
    private readonly IListUsersService _listUsers;
    private readonly IUpdateUserByAdminService _updateUserByAdmin;
    private readonly ITransferAdminRoleService _transferAdminRole;
    private readonly IGetPlanUsageService _getPlanUsage;
    private readonly IGetPlanPricingService _getPlanPricing;
    private readonly IGetBankAccountsService _getBankAccounts;
    private readonly IGetBillingDetailService _getBillingDetail;
    private readonly IUpsertBillingDetailService _upsertBillingDetail;
    private readonly ICreateSubscriptionRequestService _createSubscriptionRequest;
    private readonly ICreatePaymentNotificationService _createPaymentNotification;
    private readonly IOrgContext _orgContext;
    private readonly IPasswordHasher _hasher;
    private readonly IRequestValidator _validator;
    private readonly ITurnstileVerifier _turnstile;

    public AccountController(
        IRegisterService register,
        ILoginService login,
        IGetMeService getMe,
        IGetNotificationsService getNotifications,
        IGetOrganizationService getOrganization,
        IUpdateOrganizationService updateOrganization,
        IChangePasswordService changePassword,
        IRequestPasswordResetService requestPasswordReset,
        IResetPasswordService resetPassword,
        IVerifyEmailService verifyEmail,
        IResendEmailVerificationService resendEmailVerification,
        IUpdateProfileService updateProfile,
        ICreateUserService createUser,
        IChangeUserStatusService changeUserStatus,
        IChangeUserPasswordByAdminService changeUserPasswordByAdmin,
        IListUsersService listUsers,
        IUpdateUserByAdminService updateUserByAdmin,
        ITransferAdminRoleService transferAdminRole,
        IGetPlanUsageService getPlanUsage,
        IGetPlanPricingService getPlanPricing,
        IGetBankAccountsService getBankAccounts,
        IGetBillingDetailService getBillingDetail,
        IUpsertBillingDetailService upsertBillingDetail,
        ICreateSubscriptionRequestService createSubscriptionRequest,
        ICreatePaymentNotificationService createPaymentNotification,
        IOrgContext orgContext,
        IPasswordHasher hasher,
        IRequestValidator validator,
        ITurnstileVerifier turnstile)
    {
        _register = register;
        _login = login;
        _getMe = getMe;
        _getNotifications = getNotifications;
        _getOrganization = getOrganization;
        _updateOrganization = updateOrganization;
        _changePassword = changePassword;
        _requestPasswordReset = requestPasswordReset;
        _resetPassword = resetPassword;
        _verifyEmail = verifyEmail;
        _resendEmailVerification = resendEmailVerification;
        _updateProfile = updateProfile;
        _createUser = createUser;
        _changeUserStatus = changeUserStatus;
        _changeUserPasswordByAdmin = changeUserPasswordByAdmin;
        _listUsers = listUsers;
        _updateUserByAdmin = updateUserByAdmin;
        _transferAdminRole = transferAdminRole;
        _getPlanUsage = getPlanUsage;
        _getPlanPricing = getPlanPricing;
        _getBankAccounts = getBankAccounts;
        _getBillingDetail = getBillingDetail;
        _upsertBillingDetail = upsertBillingDetail;
        _createSubscriptionRequest = createSubscriptionRequest;
        _createPaymentNotification = createPaymentNotification;
        _orgContext = orgContext;
        _hasher = hasher;
        _validator = validator;
        _turnstile = turnstile;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register(
        [FromBody] RegisterRequest req,
        CancellationToken ct)
    {
        var blocked = await this.CheckRateLimitsAsync(ct,
            ("auth.register.ip",    RateLimitKey.Ip(HttpContext)),
            ("auth.register.email", RateLimitKey.Email(req.Email)));
        if (blocked is not null) return blocked;

        var validation = await _validator.ValidateAsync(req, ct);
        if (!validation.IsValid) return validation.ToValidationProblem(this);

        var turnstile = await _turnstile.VerifyAsync(req.TurnstileToken, HttpContext.Connection.RemoteIpAddress?.ToString(), ct);
        if (turnstile.IsFailure) return turnstile.ToActionResult(HttpContext, this);

        // 1) hash (Phase 1)
        var passwordHash = _hasher.Hash(req.Password);

        // 2) map -> input
        var input = new RegisterInput(
            req.Name,
            req.Email,
            req.OrganizationName,
            passwordHash,
            req.Phone,
            req.PlanCode
        );

        // 3) call
        var result = await _register.RegisterAsync(input, ct);

        return result.ToActionResult(HttpContext, this, r =>
            Ok(new RegisterResponse(r.Email, r.Status, r.Token, r.FullName, r.Initials, r.OrganizationName, r.Role)));
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest req,
        CancellationToken ct)
    {
        var blocked = await this.CheckRateLimitsAsync(ct,
            ("auth.login.ip",    RateLimitKey.Ip(HttpContext)),
            ("auth.login.email", RateLimitKey.Email(req.Email)));
        if (blocked is not null) return blocked;

        var validation = await _validator.ValidateAsync(req, ct);
        if (!validation.IsValid) return validation.ToValidationProblem(this);

        var turnstile = await _turnstile.VerifyAsync(req.TurnstileToken, HttpContext.Connection.RemoteIpAddress?.ToString(), ct);
        if (turnstile.IsFailure) return turnstile.ToActionResult(HttpContext, this);

        // 1) map -> input
        var input = new LoginInput(req.Email, req.Password);

        // 2) call
        var result = await _login.LoginAsync(input, ct);

        return result.ToActionResult(HttpContext, this, r =>
            Ok(new LoginResponse(r.Email, r.Status, r.Token, r.FullName, r.Initials, r.OrganizationName, r.Role)));
    }

    [HttpPost("password-reset/request")]
    [AllowAnonymous]
    public async Task<IActionResult> RequestPasswordReset(
        [FromBody] RequestPasswordResetRequest req,
        CancellationToken ct)
    {
        var blocked = await this.CheckRateLimitsAsync(ct,
            ("auth.forgot_password.ip",    RateLimitKey.Ip(HttpContext)),
            ("auth.forgot_password.email", RateLimitKey.Email(req.Email)));
        if (blocked is not null) return blocked;

        var validation = await _validator.ValidateAsync(req, ct);
        if (!validation.IsValid) return validation.ToValidationProblem(this);

        var turnstile = await _turnstile.VerifyAsync(req.TurnstileToken, HttpContext.Connection.RemoteIpAddress?.ToString(), ct);
        if (turnstile.IsFailure) return turnstile.ToActionResult(HttpContext, this);

        await _requestPasswordReset.RequestAsync(new RequestPasswordResetInput(req.Email), ct);

        return Ok(new RequestPasswordResetResponse(
            "E-Posta adresiniz mevcut ise şifre sıfırlama bilgileri gönderilmiştir. Lütfen spam/istenmeyen kutunuzu da kontrol ediniz."));
    }

    [HttpPost("password-reset")]
    [AllowAnonymous]
    public async Task<IActionResult> ResetPassword(
        [FromBody] ResetPasswordRequest req,
        CancellationToken ct)
    {
        var blocked = await this.CheckRateLimitsAsync(ct,
            ("auth.reset_password.ip",    RateLimitKey.Ip(HttpContext)),
            ("auth.reset_password.email", RateLimitKey.Email(req.Email)));
        if (blocked is not null) return blocked;

        var turnstile = await _turnstile.VerifyAsync(
            req.TurnstileToken,
            HttpContext.Connection.RemoteIpAddress?.ToString(),
            ct);
        if (turnstile.IsFailure) return turnstile.ToActionResult(HttpContext, this);

        var result = await _resetPassword.ResetAsync(
            new ResetPasswordInput(req.Email, req.Token, req.NewPassword), ct);

        if (result.IsFailure)
            return result.ToActionResult(HttpContext, this);

        return Ok(new ResetPasswordResponse("Şifreniz başarıyla güncellendi."));
    }

    [HttpPost("verify-email")]
    [AllowAnonymous]
    public async Task<IActionResult> VerifyEmail(
        [FromBody] VerifyEmailRequest req,
        CancellationToken ct)
    {
        var blocked = await this.CheckRateLimitsAsync(ct,
            ("auth.verify_email.ip",    RateLimitKey.Ip(HttpContext)),
            ("auth.verify_email.email", RateLimitKey.Email(req.Email)));
        if (blocked is not null) return blocked;

        var validation = await _validator.ValidateAsync(req, ct);
        if (!validation.IsValid) return validation.ToValidationProblem(this);

        var result = await _verifyEmail.VerifyAsync(new VerifyEmailInput(req.Email, req.Token), ct);

        if (result.IsFailure)
            return result.ToActionResult(HttpContext, this);

        return Ok(new VerifyEmailResponse("E-Posta adresiniz doğrulandı."));
    }

    [HttpPost("verify-email/resend")]
    [AllowAnonymous]
    public async Task<IActionResult> ResendEmailVerification(
        [FromBody] ResendEmailVerificationRequest req,
        CancellationToken ct)
    {
        var blocked = await this.CheckRateLimitsAsync(ct,
            ("auth.resend_verification.ip",    RateLimitKey.Ip(HttpContext)),
            ("auth.resend_verification.email", RateLimitKey.Email(req.Email)));
        if (blocked is not null) return blocked;

        var validation = await _validator.ValidateAsync(req, ct);
        if (!validation.IsValid) return validation.ToValidationProblem(this);

        await _resendEmailVerification.ResendAsync(new ResendEmailVerificationInput(req.Email), ct);

        return Ok(new ResendEmailVerificationResponse(
            "E-Posta adresiniz mevcut ise doğrulama bağlantısı gönderilmiştir. Lütfen spam/istenmeyen kutunuzu da kontrol ediniz."));
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> Me(CancellationToken ct)
    {
        var result = await _getMe.GetMeAsync(ct);

        return result.ToActionResult(HttpContext, this, r =>
            Ok(new MeResponse(r.Email, r.FullName, r.Initials, r.Role)));
    }

    [HttpGet("validate-token")]
    [Authorize]
    public IActionResult ValidateToken()
        => Ok(new ValidateTokenResponse(true));

    [HttpPost("notifications")]
    [Authorize(Policy = "ActiveUser")]
    public async Task<IActionResult> Notifications(CancellationToken ct)
    {
        var orgId = _orgContext.OrganizationId;
        if (orgId is null)
        {
            return Unauthorized();
        }

        var result = await _getNotifications.GetAsync(orgId.Value, ct);

        return result.ToActionResult(HttpContext, this, r =>
            Ok(new NotificationsResponse(
                r.Items.Select(i => new NotificationItemResponse(i.Message, i.Link)).ToList())));
    }

    [HttpGet("organization")]
    [Authorize(Policy = "ActiveUser")]
    public async Task<IActionResult> Organization(CancellationToken ct)
    {
        var result = await _getOrganization.GetAsync(ct);

        return result.ToActionResult(HttpContext, this, r =>
            Ok(new OrganizationResponse(
                r.Id,
                r.OrganizationName,
                r.ContactName,
                r.ContactEmail,
                r.ContactPhone)));
    }

    [HttpGet("plan-usage")]
    [Authorize(Policy = "ActiveUser")]
    public async Task<IActionResult> PlanUsage(CancellationToken ct)
    {
        var result = await _getPlanUsage.GetAsync(ct);

        return result.ToActionResult(HttpContext, this, r =>
            Ok(new PlanUsageResponse(
                r.PlanCode,
                r.Status,
                r.StartsAtUtc,
                r.EndsAtUtc,
                r.DowngradedFromPlanCode,
                r.DowngradedAtUtc,
                r.ExportEnabled,
                r.PendingRequest is null
                    ? null
                    : new PendingRequestResponse(
                        r.PendingRequest.RequestedPlanCode,
                        r.PendingRequest.Status,
                        r.PendingRequest.PaymentMethod,
                        r.PendingRequest.Amount,
                        r.PendingRequest.PaymentReferenceCode,
                        r.PendingRequest.RequestedAtUtc,
                        r.PendingRequest.HasPaymentNotification),
                new LimitUsageResponse(r.Users.Limit,          r.Users.Current),
                new LimitUsageResponse(r.ActivePersons.Limit,  r.ActivePersons.Current),
                new LimitUsageResponse(r.ActivePrograms.Limit, r.ActivePrograms.Current),
                new LimitUsageResponse(r.OpenFollowUps.Limit,  r.OpenFollowUps.Current))));
    }

    [HttpGet("plan-pricing")]
    [AllowAnonymous]
    public async Task<IActionResult> PlanPricing(CancellationToken ct)
    {
        var result = await _getPlanPricing.GetAsync(ct);

        return result.ToActionResult(HttpContext, this, r =>
            Ok(new PlanPricingResponse(
                r.Items.Select(i => new PlanPricingItemResponse(i.PlanCode, i.Amount)).ToList())));
    }

    [HttpGet("bank-accounts")]
    [Authorize(Policy = "ActiveUser")]
    public async Task<IActionResult> BankAccounts(CancellationToken ct)
    {
        var result = await _getBankAccounts.GetAsync(ct);

        return result.ToActionResult(HttpContext, this, r =>
            Ok(new BankAccountsResponse(
                r.Items.Select(i => new BankAccountItemResponse(
                    i.BankName, i.AccountHolder, i.Iban, i.Note)).ToList())));
    }

    [HttpGet("billing")]
    [Authorize(Policy = "ActiveUser")]
    public async Task<IActionResult> Billing(CancellationToken ct)
    {
        var result = await _getBillingDetail.GetAsync(ct);

        return result.ToActionResult(HttpContext, this, r =>
            Ok(new BillingDetailResponse(
                r.BillingType,
                r.BillingName,
                r.TaxNumber,
                r.TaxOffice,
                r.BillingEmail,
                r.BillingAddress)));
    }

    [HttpPost("billing")]
    [Authorize(Policy = "ActiveUser")]
    public async Task<IActionResult> SaveBilling(
        [FromBody] UpsertBillingDetailRequest req,
        CancellationToken ct)
    {
        var validation = await _validator.ValidateAsync(req, ct);
        if (!validation.IsValid) return validation.ToValidationProblem(this);

        var input = new UpsertBillingDetailInput(
            req.BillingType,
            req.BillingName,
            req.TaxNumber,
            req.TaxOffice,
            req.BillingEmail,
            req.BillingAddress);

        var result = await _upsertBillingDetail.UpsertAsync(input, ct);

        return result.ToActionResult(HttpContext, this, r =>
            Ok(new BillingDetailResponse(
                r.BillingType,
                r.BillingName,
                r.TaxNumber,
                r.TaxOffice,
                r.BillingEmail,
                r.BillingAddress)));
    }

    [HttpPost("subscription-payment-notification")]
    [Authorize(Policy = "ActiveUser")]
    [RequestSizeLimit(6 * 1024 * 1024)]
    [RequestFormLimits(MultipartBodyLengthLimit = 6 * 1024 * 1024)]
    public async Task<IActionResult> SubmitPaymentNotification(
        [FromForm] CreatePaymentNotificationRequest req,
        CancellationToken ct)
    {
        var validation = await _validator.ValidateAsync(req, ct);
        if (!validation.IsValid) return validation.ToValidationProblem(this);

        var receipt = req.Receipt!;
        await using var stream = receipt.OpenReadStream();

        var input = new CreatePaymentNotificationInput(
            req.SenderName,
            req.PaymentDate,
            req.Amount,
            req.Note,
            stream,
            receipt.FileName,
            receipt.ContentType,
            receipt.Length);

        var result = await _createPaymentNotification.CreateAsync(input, ct);

        return result.ToActionResult(HttpContext, this, r =>
            StatusCode(StatusCodes.Status201Created, new PaymentNotificationResponse(
                r.Id,
                r.SubscriptionRequestId,
                r.SenderName,
                r.PaymentDate,
                r.Amount,
                r.Note,
                r.ReceiptFileName,
                r.ReceiptContentType,
                r.ReceiptSizeBytes,
                r.CreatedAtUtc)));
    }

    [HttpPost("subscription-request")]
    [Authorize(Policy = "ActiveUser")]
    public async Task<IActionResult> CreateSubscriptionRequest(
        [FromBody] CreateSubscriptionRequestRequest req,
        CancellationToken ct)
    {
        var validation = await _validator.ValidateAsync(req, ct);
        if (!validation.IsValid) return validation.ToValidationProblem(this);

        var input = new CreateSubscriptionRequestInput(req.RequestedPlanCode);

        var result = await _createSubscriptionRequest.CreateAsync(input, ct);

        return result.ToActionResult(HttpContext, this, r =>
            StatusCode(StatusCodes.Status201Created, new SubscriptionRequestResponse(
                r.Id,
                r.RequestedPlanCode,
                r.Status,
                r.PaymentMethod,
                r.Amount,
                r.PaymentReferenceCode,
                r.RequestedAtUtc,
                r.ExpiresAtUtc)));
    }

    [HttpPost("organization")]
    [Authorize(Policy = "ActiveUser")]
    public async Task<IActionResult> UpdateOrganization(
        [FromBody] UpdateOrganizationRequest req,
        CancellationToken ct)
    {
        var validation = await _validator.ValidateAsync(req, ct);
        if (!validation.IsValid) return validation.ToValidationProblem(this);

        var input = new UpdateOrganizationInput(
            req.OrganizationName,
            req.ContactName,
            req.ContactEmail,
            req.ContactPhone);

        var result = await _updateOrganization.UpdateAsync(input, ct);

        return result.ToActionResult(HttpContext, this, r =>
            Ok(new OrganizationResponse(
                r.Id,
                r.OrganizationName,
                r.ContactName,
                r.ContactEmail,
                r.ContactPhone)));
    }

    [HttpPost("change-password")]
    [Authorize(Policy = "ActiveUser")]
    public async Task<IActionResult> ChangePassword(
        [FromBody] ChangePasswordRequest req,
        CancellationToken ct)
    {
        var validation = await _validator.ValidateAsync(req, ct);
        if (!validation.IsValid) return validation.ToValidationProblem(this);

        var newPasswordHash = _hasher.Hash(req.NewPassword);

        var input = new ChangePasswordInput(
            req.OldPassword,
            newPasswordHash);

        var result = await _changePassword.ChangePasswordAsync(input, ct);

        return result.ToActionResult(HttpContext, this);
    }

    [HttpPost("profile")]
    [Authorize(Policy = "ActiveUser")]
    public async Task<IActionResult> UpdateProfile(
        [FromBody] UpdateProfileRequest req,
        CancellationToken ct)
    {
        var validation = await _validator.ValidateAsync(req, ct);
        if (!validation.IsValid) return validation.ToValidationProblem(this);

        var input = new UpdateProfileInput(req.FullName);

        var result = await _updateProfile.UpdateProfileAsync(input, ct);

        return result.ToActionResult(HttpContext, this, r =>
            Ok(new UpdateProfileResponse(r.Email, r.FullName, r.Initials)));
    }

    [HttpPost("user")]
    [Authorize(Policy = "ActiveUser")]
    public async Task<IActionResult> CreateUser(
        [FromBody] CreateUserRequest req,
        CancellationToken ct)
    {
        var validation = await _validator.ValidateAsync(req, ct);
        if (!validation.IsValid) return validation.ToValidationProblem(this);

        var passwordHash = _hasher.Hash(req.Password);

        var input = new CreateUserInput(
            req.Name,
            req.Email,
            passwordHash);

        var result = await _createUser.CreateAsync(input, ct);

        return result.ToActionResult(HttpContext, this, r =>
            StatusCode(StatusCodes.Status201Created, new CreateUserResponse(
                r.UserId,
                r.Email,
                r.FullName,
                r.Role,
                r.Status.ToString(),
                r.LastLoginAtUtc)));
    }

    [HttpGet("user/list")]
    [Authorize(Policy = "ActiveUser")]
    public async Task<IActionResult> ListUsers(
        [FromQuery] ListUsersQuery query,
        CancellationToken ct)
    {
        var validation = await _validator.ValidateAsync(query, ct);
        if (!validation.IsValid) return validation.ToValidationProblem(this);

        var statuses = ParseStatuses(query.Status);

        var input = new ListUsersInput(
            query.Page,
            query.PageSize,
            statuses);

        var result = await _listUsers.ListAsync(input, ct);

        return result.ToActionResult(HttpContext, this, r =>
            Ok(new UserListPagedResponse(
                r.Items.Select(x => new UserListItemResponse(
                    x.UserId,
                    x.FullName,
                    x.Email,
                    x.Role,
                    x.Status.ToString(),
                    x.LastLoginAtUtc)).ToList(),
                r.Page,
                r.PageSize,
                r.TotalCount)));
    }

    private static IReadOnlyCollection<UserStatus>? ParseStatuses(string? raw)
    {
        if (string.IsNullOrWhiteSpace(raw)) return null;

        var parsed = raw
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(token => Enum.Parse<UserStatus>(token, ignoreCase: true))
            .Distinct()
            .ToArray();

        return parsed.Length == 0 ? null : parsed;
    }

    [HttpPost("user/update")]
    [Authorize(Policy = "ActiveUser")]
    public async Task<IActionResult> UpdateUserByAdmin(
        [FromBody] UpdateUserByAdminRequest req,
        CancellationToken ct)
    {
        var validation = await _validator.ValidateAsync(req, ct);
        if (!validation.IsValid) return validation.ToValidationProblem(this);

        var input = new UpdateUserByAdminInput(
            req.UserId,
            req.FullName);

        var result = await _updateUserByAdmin.UpdateAsync(input, ct);

        return result.ToActionResult(HttpContext, this, r =>
            Ok(new UpdateUserByAdminResponse(
                r.UserId,
                r.Email,
                r.FullName,
                r.Role,
                r.Status.ToString(),
                r.LastLoginAtUtc)));
    }

    [HttpPost("user/change-password")]
    [Authorize(Policy = "ActiveUser")]
    public async Task<IActionResult> ChangeUserPasswordByAdmin(
        [FromBody] ChangeUserPasswordByAdminRequest req,
        CancellationToken ct)
    {
        var validation = await _validator.ValidateAsync(req, ct);
        if (!validation.IsValid) return validation.ToValidationProblem(this);

        var newPasswordHash = _hasher.Hash(req.Password);

        var input = new ChangeUserPasswordByAdminInput(
            req.UserId,
            newPasswordHash);

        var result = await _changeUserPasswordByAdmin.ChangeAsync(input, ct);

        return result.ToActionResult(HttpContext, this);
    }

    [HttpPost("user/transfer-admin")]
    [Authorize(Policy = "ActiveUser")]
    public async Task<IActionResult> TransferAdminRole(
        [FromBody] TransferAdminRoleRequest req,
        CancellationToken ct)
    {
        var validation = await _validator.ValidateAsync(req, ct);
        if (!validation.IsValid) return validation.ToValidationProblem(this);

        var input = new TransferAdminRoleInput(req.UserId);

        var result = await _transferAdminRole.TransferAsync(input, ct);

        return result.ToActionResult(HttpContext, this);
    }

    [HttpPost("user/{userId:guid}/status")]
    [Authorize(Policy = "ActiveUser")]
    public async Task<IActionResult> ChangeUserStatus(
        Guid userId,
        [FromBody] ChangeUserStatusRequest req,
        CancellationToken ct)
    {
        var validation = await _validator.ValidateAsync(req, ct);
        if (!validation.IsValid) return validation.ToValidationProblem(this);

        var input = new ChangeUserStatusInput(
            userId,
            req.Status);

        var result = await _changeUserStatus.ChangeAsync(input, ct);

        return result.ToActionResult(HttpContext, this);
    }
}
