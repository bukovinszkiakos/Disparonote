﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

AddServices();
ConfigureSwagger();
AddDbContexts();
AddAuthentication();
AddIdentity();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
Console.WriteLine("CONNECTION STRING:");
Console.WriteLine(builder.Configuration.GetConnectionString("DefaultConnection"));

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DisparoNoteDbContext>();

    var providerName = dbContext.Database.ProviderName;

    if (!string.IsNullOrEmpty(providerName) && providerName.Contains("SqlServer"))
    {
        dbContext.Database.Migrate();
    }
}

app.Run();

// Method implementations
void AddServices()
{
    builder.Services.AddScoped<INoteService, NoteService>();
    builder.Services.AddScoped<INoteRepository, NoteRepository>();
    builder.Services.AddScoped<ITokenService, TokenService>();
    builder.Services.AddScoped<IAuthService, AuthService>();
    builder.Services.AddHostedService<NoteCleanupService>();
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
}
void ConfigureSwagger()
{
    builder.Services.AddSwaggerGen(option =>
    {
        option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
        option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter a valid token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "Bearer"
        });
        option.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type=ReferenceType.SecurityScheme,
                        Id="Bearer"
                    }
                },
                new string[]{}
            }
        });
    });
}
void AddDbContexts()
{
    builder.Services.AddDbContext<DisparoNoteDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
}
void AddAuthentication()
{
    var jwtSection = builder.Configuration.GetSection("Jwt");
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ClockSkew = TimeSpan.Zero,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSection["ValidIssuer"],
                ValidAudience = jwtSection["ValidAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["IssuerSigningKey"]))
            };

            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    if (string.IsNullOrEmpty(context.Token))
                    {
                        context.Token = context.Request.Cookies["token"];
                    }
                    return Task.CompletedTask;
                }
            };
        });
}
void AddIdentity()
{
    builder.Services
        .AddIdentityCore<IdentityUser>(options =>
        {
            options.SignIn.RequireConfirmedAccount = false;
            options.User.RequireUniqueEmail = true;
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 6;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
        })
        .AddEntityFrameworkStores<DisparoNoteDbContext>();
}

public partial class Program { }