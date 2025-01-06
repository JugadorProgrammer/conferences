using Conference.Core.Interfaces;
using Conference.Core.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Conference.Server
{
    public class Program
    {
        private static void ConfigureCookie(CookieAuthenticationOptions options)
        {
            options.Cookie.Path = "/";
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
            options.Cookie.MaxAge = TimeSpan.FromDays(365);
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            options.LoginPath = new PathString("/Authentication/SingIn");
            options.LogoutPath = new PathString("/Authentication/SignOut");
        }

        private static void RegisterDIServices(WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<IDataBaseService, DataBaseService>(
                _ => new DataBaseService(builder.Configuration["ConnectionStrings:DataBaseConnection"]!));
        }

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Logging.ClearProviders().AddConsole();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(ConfigureCookie);

            RegisterDIServices(builder);

            var app = builder.Build();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.Run();
        }
    }
}
