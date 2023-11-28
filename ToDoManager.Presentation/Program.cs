using Autofac.Core;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ToDoManager.Application.Extensions;
using ToDoManager.Infrastructure.Extensions;
using ToDoManager.Infrastructure.Options;
using ToDoManager.Persistance.Extensions;

namespace ToDoManager.Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();
            builder.Services.AddApplicationLayer();
            builder.Services.AddInfrastructureLayer();
            builder.Services.AddPersistenceLayer(builder.Configuration,builder.Host);
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); 
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

             
            builder.Services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                //opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = false,
                    ValidIssuer = AuthOption.ISSUER,
                    ValidAudience = AuthOption.AUDIENCE,
                    IssuerSigningKey = AuthOption.GetSymmetricSecurityKey()
                };
            });

            builder.Services.AddAuthorization(option => option.DefaultPolicy =
                new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser().Build());


            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",new OpenApiInfo { Title = "Cryptolus",Version = "v1" });
                options.AddSecurityDefinition("Bearer",new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Pleace enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                   {
                        new OpenApiSecurityScheme
                        {
                             Reference = new OpenApiReference
                             {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                             }
                        },
                        new string[]{}
                   }

                });
            });

            var app = builder.Build();

            app.UseSession();
            app.UseSwagger();
            app.UseSwaggerUI();
            


            if(!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseCookiePolicy(new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.Strict,
                HttpOnly = HttpOnlyPolicy.Always,
                Secure = CookieSecurePolicy.Always
            });
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.Use(async (context,next) =>
            {
                var token = context.Request.Cookies[".AspNetCore.Application.Id"];
                if(!string.IsNullOrEmpty(token))
                    context.Request.Headers.Add("Authorization","Bearer " + token);

                await next();
            });
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=LogIn}/{id?}");

            app.Run();
        }
    }
}