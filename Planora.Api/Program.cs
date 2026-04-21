using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Planora.Api.Services;
using Planora.Api.Services.Auth;
using Planora.Api.Services.Category;
using Planora.Api.Services.Task;
using Planora.Api.Services.User;
using Planora.Api.Services.Auth.JwtToken;
using Planora.DataAccess.Context;
using Planora.DataAccess.Models.Auth;
using Planora.DataAccess.Repositories.Category;
using Planora.DataAccess.Repositories.CalenderYear;
using Planora.DataAccess.Repositories.Task;
using Planora.DataAccess.Repositories.User;
using Planora.Api.Services.CalenderYear;
using Microsoft.AspNetCore.Diagnostics;
using Planora.Api.Services.Auth.PasswordReset;
using Planora.Api.Services.Email;

namespace Planora.Api;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers(options =>
        {
            options.SuppressAsyncSuffixInActionNames = false;
        });
        builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(
            builder.Configuration.GetConnectionString("DefaultConnection")));
            
        //Service Layer
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<IPasswordResetService, PasswordResetService>();
        builder.Services.AddScoped<IEmailService, EmailService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<ITaskService, TaskService>();
        builder.Services.AddScoped<ICategoryService, CategoryService>();
        builder.Services.AddScoped<IEmailService, EmailService>();
        builder.Services.AddScoped<IPasswordResetService, PasswordResetService>();
        builder.Services.AddScoped<ICalenderYearService, CalenderYearService>();

        //Repository
        builder.Services.AddScoped<ITaskRepository, TaskRepository>();
        builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<ICalenderYearRepository, CalenderYearRepository>();

        //Auth Services
        builder.Services.AddIdentity<AuthUser, IdentityRole>()
            .AddEntityFrameworkStores<DatabaseContext>();

        builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
            
        builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
                    ),
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    ValidateLifetime = true
                };
            });
            
        builder.Services.AddAuthorization();
            
        //CORS
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("DevOpenPolicy", policy =>
            {
                policy.SetIsOriginAllowed(origin => true) // Allows any Port/IP (Live Server, etc.)
                    .AllowAnyHeader()
                    .AllowAnyMethod(); 
            });
        });
                
        var app = builder.Build();
            
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            try
            {
                await DbInitializer.SeedAdminUser(services);
            }
            catch (Exception ex)
            {
                // Log errors here (e.g., using Serilog or built-in ILogger)
                Console.WriteLine($"An error occurred seeding the DB: {ex.Message}");
            }
        }
            
        if (app.Environment.IsDevelopment())
        {
            app.UseCors("DevOpenPolicy");
        }

        app.UseExceptionHandler(err => err.Run(async controllerException =>
        {
            var exception = controllerException.Features.Get<IExceptionHandlerFeature>()?.Error;
            var (status, message) = exception switch
            {
                KeyNotFoundException e => (404, e.Message),
                ArgumentException e => (400, e.Message),
                InvalidOperationException => (409, exception?.Message ?? "Conflict"),
                _ => (500, "An error occurred")
            };
            controllerException.Response.StatusCode = status;
            await controllerException.Response.WriteAsJsonAsync(new { error = message });
        }));

        // Configure the HTTP request pipeline.
        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseDefaultFiles();
        app.UseStaticFiles();
          
        app.MapControllers();

        app.Run();
    }
}
