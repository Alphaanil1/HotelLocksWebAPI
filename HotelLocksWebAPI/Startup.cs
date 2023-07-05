using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using HotelLock.DAL.Context;
using HotelLock.BusinessObjects.Models;
using HotelLock.DAL.Repositories.InterfaceRepositories;
using HotelLock.BLL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using HotelLock.DAL.Repositories;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace HotelLocksWebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {


            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                var Key = Encoding.UTF8.GetBytes(Configuration["JWT:JWTKey"]);
                o.SaveToken = true;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["JWT:JWTIssuer"],
                    ValidAudience = Configuration["JWT:JWTAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Key)
                };

            });
            // Unauthorized (401) MiddleWare


            services.AddSingleton<IJWTManagerRepository, JWTManagerRepository>();
            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "HotelLockWebAPI", Version = "v1" });
            });

            // // services.AddDbContext<HRMSContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DevConnection")));
            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
            services.AddTransient<IEmailService, EmailService>();
            //services.AddCors();
            // Default Policy
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.WithOrigins("http://64.202.191.110:2241", "http://localhost:4200")
                                            .AllowAnyHeader()
                                            .AllowAnyMethod();
                    });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(options =>
            options.WithOrigins("http://localhost:4200", "http://64.202.191.110:2241", "http://64.202.191.110:2242")
            .AllowAnyMethod()
            .AllowAnyHeader()
            );


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HotelLockWebAPI v1"));
            }

            app.Use(async (context, next) =>
            {
                await next();

                // Unauthorized (401) MiddleWare
                if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
                {
                    string retrunvalue = "{\"Error\":{\"Code\": 401,\"Message\": \"Token Validation Has Failed. Request Access Denied\"}}";
                    await context.Response.WriteAsync(retrunvalue);

                }
            });

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication(); // This need to be added	
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

    }
}
