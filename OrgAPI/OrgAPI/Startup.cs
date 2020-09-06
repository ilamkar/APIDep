using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OrgDAL;

namespace OrgAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public object IExecptionHandlerFeature { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //services.AddControllers(
            //   // Exception Handling globally - custom Filter
            //   config => config.Filters.Add(new MyExceptionFilter())
            //   ).AddXmlSerializerFormatters().AddXmlDataContractSerializerFormatters();
            services.AddControllers(
               // Exception Handling globally - custom Filter
               config => config.Filters.Add(new AuthorizeFilter())
               ).AddXmlSerializerFormatters().AddXmlDataContractSerializerFormatters();


            // for cookie based authentication
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });
            services.AddDbContext<OrganizationDbContext>(); //injecting organizationDbContext object

      
            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<OrganizationDbContext>().AddDefaultTokenProviders();

            //return error code for unauthorized user


            services.ConfigureApplicationCookie(opt =>
            {
                opt.Events = new CookieAuthenticationEvents
                {
                    OnRedirectToLogin = RedirectContext =>
                     {
                         RedirectContext.HttpContext.Response.StatusCode = 401;
                         return Task.CompletedTask;
                     },
                    OnRedirectToAccessDenied = RedirectContext =>
                    {
                        RedirectContext.HttpContext.Response.StatusCode = 401;
                        return Task.CompletedTask;
                    }
                };
            });

            services.AddSwaggerDocument();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();

            // Error Handling Globally - Middleware
            //app.UseExceptionHandler(

            //    options =>
            //    {
            //        options.Run(async context =>
            //        {
            //            context.Response.StatusCode = 500;
            //            context.Response.ContentType = "application/json";
            //            var ex = context.Features.Get<IExceptionHandlerFeature>();

            //            if (ex != null)
            //            {
            //                await context.Response.WriteAsync(ex.Error.Message);
            //            }

            //        });
            //    }

            //    );

            app.UseRouting();
                app.UseOpenApi();
            app.UseSwaggerUi3();

           // app.UseCors("CorsPolicy");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
