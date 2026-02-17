using EduCrm.Infrastructure.Extensions;
using EduCrm.Modules.Account.Application.Extensions;
using EduCrm.Modules.Account.Infrastructure.Extensions;
using EduCrm.Modules.People.Application.Extensions;
using EduCrm.Modules.People.Infrastructure.Extensions;
using EduCrm.WebApi.Extensions;
using EduCrm.WebApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddInfrastructure();
builder.Services.AddPostgresDb(builder.Configuration);

builder.Services.AddJwtAuth(builder.Configuration);

builder.Services.AddPeopleInfra();
builder.Services.AddPeopleApplication();

builder.Services.AddAccountInfra();
builder.Services.AddAccountApplication();

builder.Services.AddFluentValidation();
builder.Services.AddWebApiConventions();

var corsOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("DevCors", policy =>
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

    options.AddPolicy("ProdCors", policy =>
    {
        if (corsOrigins.Length > 0)
        {
            policy.WithOrigins(corsOrigins)
                .AllowAnyHeader()
                .AllowAnyMethod();
        }
        else
        {
            policy.SetIsOriginAllowed(_ => false);
        }
    });
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

if (app.Environment.IsDevelopment())
{
    app.UseCors("DevCors");
}
else
{
    app.UseCors("ProdCors");
}

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<OrganizationContextMiddleware>();

app.MapControllers();

app.Run();