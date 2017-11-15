using CRMCore.Framework.MvcCore.Extensions;
using CRMCore.WebApp.Config;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace CRMCore.WebApp
{
    public class Startup
    {
        private static readonly string[] IdSrvPaths =
        {
            "/client-callback-popup",
            "/client-callback-silent",
            "/account",
            "/error"
        };

        public Startup(IHostingEnvironment env, IConfiguration config)
        {
            Environment = env;
            Configuration = config;
        }

        public IConfiguration Configuration { get; }

        private IHostingEnvironment Environment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcModules();
        }

        public void Configure(IApplicationBuilder app)
        {
            MapAndUseIdSrv(app);
            MapAndUseWebApp(app);

            app.UseModules();
        }

        private void MapAndUseIdSrv(IApplicationBuilder app)
        {
            app.Map(Constants.IdentityPrefix, identityApp =>
            {
                if (Environment.IsDevelopment())
                {
                    identityApp.UseDeveloperExceptionPage();
                }
                else
                {
                    identityApp.UseExceptionHandler("/error");
                }

                // TODO: aPhuong will add more configurations for IdSrv 
                // TODO: ...

                identityApp.UseStaticFiles();
                identityApp.MapWhen(x => IsIdentityRequest(x), mvcApp =>
                {
                    mvcApp.UseMvc();
                });
            });
        }

        private void MapAndUseWebApp(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true,
                    ReactHotModuleReplacement = true
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.MapWhen(x => !IsIdentityRequest(x), webApp =>
            {
                // TODO: Thang will map Swagger here
                // TODO: ...

                webApp.UseMvc(routes =>
                {
                    routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                    routes.MapSpaFallbackRoute(
                        name: "spa-fallback",
                        defaults: new { controller = "Home", action = "Index" });
                });
            });

            app.UseStaticFiles();
        }

        private static bool IsIdentityRequest(HttpContext context)
        {
            return IdSrvPaths.Any(p => context.Request.Path.StartsWithSegments(p));
        }
    }
}