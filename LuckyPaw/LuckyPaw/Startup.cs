using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using LuckyPaw.Models;
using LuckyPaw.Data;
using Microsoft.AspNetCore.Identity.UI.Services;
using LuckyPaw.Helpers;

namespace LuckyPaw
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSession();

            services.AddDbContext<LuckyPawContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("LuckyPawContext")));

            //services.AddIdentity<IdentityUser, IdentityRole>().AddDefaultUI(UIFramework.Bootstrap4).AddDefaultTokenProviders().AddEntityFrameworkStores<LuckyPawContext>();
            //services.AddIdentity<IdentityUser, IdentityRole>()
            //.AddEntityFrameworkStores<LuckyPawContext>()
            //.AddDefaultTokenProviders();

            // services.AddDefaultIdentity<IdentityUser>()
            //  .AddDefaultUI(UIFramework.Bootstrap4).AddEntityFrameworkStores<LuckyPawContext>();

            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<LuckyPawContext>().AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;
                //options.SignIn.RequireConfirmedEmail = true;
            });

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.SlidingExpiration = true;
            });

            services.AddTransient<IEmailSender, EmailSender>();
            services.Configure<AuthMessageSenderOptions>(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider services)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            // Detail of the use https://docs.microsoft.com/en-us/aspnet/core/fundamentals/error-handling?view=aspnetcore-2.2
            app.UseStatusCodePages();

            // Use session for the shopping cart
            app.UseSession();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            // Use authentication for the identity
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            // Create user roles
            //CreateUserRoles(services).Wait();
        }

        /*
        private async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            // Get role manager and user manager
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            IdentityResult roleResult;

            //Adding the Admin Role
            var roleAdminCheck = await RoleManager.RoleExistsAsync("Admin");
            if (!roleAdminCheck)
            {
                //create the roles and seed them to the database
                roleResult = await RoleManager.CreateAsync(new IdentityRole("Admin"));
            }

            //Adding the Manager Role
            var roleManagerCheck = await RoleManager.RoleExistsAsync("Manager");
            if (!roleManagerCheck)
            {
                //create the roles and seed them to the database
                roleResult = await RoleManager.CreateAsync(new IdentityRole("Manager"));
            }

            //Adding the User Role
            var roleUserCheck = await RoleManager.RoleExistsAsync("User");
            if (!roleUserCheck)
            {
                //create the roles and seed them to the database
                roleResult = await RoleManager.CreateAsync(new IdentityRole("User"));
            }

            //Assign Admin role to the main User here we have given our newly registered 
            //login id for Admin management
            // Use the code below to initially choose a user to be admin, so that the admin user can 
            // update the account roles for other users. After setting the admin role, you need to comment it out
            // so that it does not re-assign the role because the admin user role has already been set to admin.

            //IdentityUser user = await UserManager.FindByEmailAsync("admin@admin.com");
            //await UserManager.RemoveFromRoleAsync(user, "User");
            //await UserManager.AddToRoleAsync(user, "Admin");
        }*/
    }
}
