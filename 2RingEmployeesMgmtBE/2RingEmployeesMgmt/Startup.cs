using Domain.Models;
using Domain.Services;
using Domain.Services.Interfaces;
using Domain.Services.Validation;
using FluentValidation;
using Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace _2RingEmployeesMgmt
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
            {
                builder.AllowAnyMethod().AllowAnyHeader().AllowCredentials().WithOrigins("http://localhost:8080");
            }));

            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "2RingEmployeeMgmt", Version = "v1" });
            });

            //database
            services.AddDbContext<EmployeesDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("2RingEmployeeDb"));
                options.UseLazyLoadingProxies();
            });

            //services
            services.AddScoped<IUnitOfWork, EmployeesDbContext>();
            services.AddTransient<IEmployeeService, EmployeeService>();
            services.AddTransient<IPositionService, PositionService>();
            services.AddTransient<IValidator<Employee>, EmployeeValidator>();
            services.AddTransient<IValidator<Position>, PositionValidator>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("CorsPolicy");

            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "2RingEmployeeMgmt");
                c.RoutePrefix = string.Empty;
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}