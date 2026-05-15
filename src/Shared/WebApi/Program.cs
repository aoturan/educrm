using EduCrm.Infrastructure.Extensions;
using EduCrm.Modules.Account.Application.Email;
using EduCrm.Modules.Account.Application.EmailVerification;
using EduCrm.Modules.Account.Application.Errors;
using EduCrm.Modules.Account.Application.PasswordReset;
using EduCrm.Modules.Account.Application.Extensions;
using EduCrm.Modules.Account.Infrastructure.Extensions;
using EduCrm.Modules.People.Application.Errors;
using EduCrm.Modules.People.Application.Extensions;
using EduCrm.Modules.People.Infrastructure.Extensions;
using EduCrm.Modules.Program.Application.Errors;
using EduCrm.Modules.Program.Application.Extensions;
using EduCrm.Modules.Program.Infrastructure.Extensions;
using EduCrm.Modules.Support.Application.Extensions;
using EduCrm.Modules.Support.Infrastructure.Extensions;
using EduCrm.Modules.Account.Application.Repositories;
using EduCrm.Modules.Account.Contracts.Dtos;
using EduCrm.SharedKernel.Errors;
using EduCrm.WebApi.Extensions;
using EduCrm.WebApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Register domain error-to-HTTP-status mappings
ErrorHttpStatusMapper.Register(AccountErrorMappings.Mappings);
ErrorHttpStatusMapper.Register(PeopleErrorMappings.Mappings);
builder.Services.AddProgramInfra();
builder.Services.AddProgramApplication();
ErrorHttpStatusMapper.Register(ProgramErrorMappings.Mappings);

builder.Services.AddInfrastructure();
builder.Services.AddPostgresDb(builder.Configuration);

builder.Services.AddJwtAuth(builder.Configuration);

builder.Services.Configure<PlanLimitsOptions>(builder.Configuration.GetSection("PlanLimits"));
builder.Services.Configure<PlanPricingOptions>(builder.Configuration.GetSection("PlanPricing"));
builder.Services.Configure<DigitalOceanSpacesOptions>(builder.Configuration.GetSection("DigitalOceanSpaces"));
builder.Services.Configure<EmailOptions>(builder.Configuration.GetSection("Email"));
builder.Services.Configure<PasswordResetOptions>(builder.Configuration.GetSection("Account:PasswordReset"));
builder.Services.Configure<EmailVerificationOptions>(builder.Configuration.GetSection("Account:EmailVerification"));

builder.Services.AddAccountInfra();
builder.Services.AddAccountApplication();

builder.Services.AddPeopleInfra();
builder.Services.AddPeopleApplication();

builder.Services.AddSupportInfra();
builder.Services.AddSupportApplication();

builder.Services.AddFluentValidation();
builder.Services.AddWebApiConventions();

var corsOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy => policy
        .WithOrigins(corsOrigins)
        .WithMethods("GET", "POST", "DELETE")
        .WithHeaders("Authorization", "Content-Type")
        .WithExposedHeaders("Content-Disposition")
        .SetPreflightMaxAge(TimeSpan.FromMinutes(10)));
});


var app = builder.Build();

app.UseGlobalMiddlewares();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();

app.UseMiddleware<CurrentUserSnapshotMiddleware>();

app.UseAuthorization();

app.UseMiddleware<OrganizationContextMiddleware>();

app.MapControllers();

app.Run();