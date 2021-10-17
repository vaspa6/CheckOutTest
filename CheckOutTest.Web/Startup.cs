using CheckOutTest.Web.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using CheckOutTest.Web.Configuration.Middleware;
using CheckOutTest.Core.BankManagement.Interfaces;
using CheckOutTest.Core.MockBank;
using CheckOutTest.Core.PaymentManagement.Interfaces;
using CheckOutTest.Core.PaymentManagement;
using CheckOutTest.Data.Repositories.Payment;
using CheckOutTest.Data.Configuration;
using Microsoft.EntityFrameworkCore;

namespace CheckOutTest.Web
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
            services.AddAutoMapper(typeof(Startup));
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Api-Key",
                    new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Description = "Use the key provided during onboarding",
                        Type = SecuritySchemeType.ApiKey,
                        Name = "Api-Key",
                    });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Api-Key",
                                 },
                        }, new List<string>()
                        },
                    });
                options.OperationFilter<AddAuthHeader>();
            });

            services.AddControllers();
            services.AddEntityFrameworkSqlite().AddDbContext<DatabaseContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("SQLDB")));
            services.AddScoped<IBank, MockBank>();
            services.AddScoped<IPaymentManager, PaymentManager>();
            services.AddScoped<IPaymentRepo, PaymentRepo>();
            services.AddAuthentication(options =>
            {
                options.AddScheme<ApiKeyAuth>("ApiKeyScheme", "ApiKeyScheme");
                options.DefaultScheme = "ApiKeyScheme";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<DatabaseContext>();
                context.Database.EnsureCreated();
            }
        }
    }
}
