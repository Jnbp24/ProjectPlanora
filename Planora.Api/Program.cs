using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Planora.Api.Services;
using Planora.Api.Services.Auth;
using Planora.DataAccess.Context;
using Planora.DataAccess.Models.Auth;
using Planora.Api.Services.User;
using Planora.Api.Services.Task;
using Planora.Api.Services.Category;
using Planora.Api.Services.Project;
using Planora.DataAccess.Repositories.Category;
using Planora.DataAccess.Repositories.Project;
using Planora.DataAccess.Repositories.Task;
using Planora.DataAccess.Repositories.User;

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
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<ITaskService, TaskService>();
        builder.Services.AddScoped<ICategoryService, CategoryService>();
        builder.Services.AddScoped<IProjectService, ProjectService>();

        //Repository
        builder.Services.AddScoped<ITaskRepository, TaskRepository>();
        builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IProjectRepository, ProjectRepository>();

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