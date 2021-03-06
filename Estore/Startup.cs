using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.UnitOfWork;
using Desktop.common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Estore
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
            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.Cookie.Name = ".FEstore.Session";
                options.IdleTimeout = TimeSpan.FromSeconds(12600);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddMvc(config =>
            {
                // Add XML Content Negotiation
                config.RespectBrowserAcceptHeader = true;
                config.ReturnHttpNotAcceptable = true;
                config.OutputFormatters.Add(new XmlSerializerOutputFormatter());
            });
            services.AddSingleton
                   (Configuration.Get<AppSetting>());
            services.AddSingleton<IUnitOfWorkFactory>();
            services.AddRazorPages();
            services.AddRouting();
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseSession();

            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthorization();


            app.Use(async (context, next) =>
            {
                Console.WriteLine("Middleware checking role");
                String role = context.Session.GetString("Role");

                string requestPath = context.Request.Path.Value;
                Console.WriteLine("Request path: " + requestPath);
                if (requestPath.ToLower().Contains("admin"))
                {
                    if (role == null || !role.Equals("Admin"))
                    {
                        context.Response.Redirect("/", true);
                        await next();
                        return;
                    }
                    else
                    {
                        await next();
                        return;
                    }

                }

                if (requestPath.ToLower().Contains("member"))
                {
                    if (role == null || !role.Equals("Member"))
                    {
                        context.Response.Redirect("/", true);
                        await next();
                        return;
                    }
                    else
                    {
                        await next();
                        return;
                    }
                }
                await next();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("areaRoute", "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                   name: "default",
                   pattern: "{controller=Auth}/{action=Login}/{id?}");
                endpoints.MapRazorPages();
            });

        }
    }
}
