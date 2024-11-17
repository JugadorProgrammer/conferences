using Conference.Core.Interfaces;
using Conference.Core.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Conference.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.HttpOnly = true;
                    options.Cookie.IsEssential = true;
                    options.Cookie.MaxAge = TimeSpan.FromDays(365);
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                    options.LoginPath = new PathString("/Authentication/SingIn");
                    options.LogoutPath = new PathString("/Authentication/SignOut");
                });

            builder.Services.AddTransient<IDataBaseService, DataBaseService>(
                _ => new DataBaseService(builder.Configuration["ConnectionStrings:DataBaseConnection"]!));

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
