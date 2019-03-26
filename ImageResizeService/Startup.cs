using ImageResizeService.Infrastructure.ApplicationSettings;
using ImageResizeService.Services.ImageProcessor;
using ImageResizeService.Services.ImageService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ImageResizeService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        private const string CorsPolicyName = "DefaultPolicy";

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //Add services
            services.AddScoped<IImageProcessor, ImageProcessor>();
            services.AddSingleton<IImageService, ImageService>();
            //Configure settings
            services.ConfigureSettings<HttpClientRetrySettings>(Configuration.GetSection("RetrySettings"));

            services.AddCors(options =>
                options.AddPolicy(CorsPolicyName, builder => builder.AllowAnyOrigin().WithMethods("GET")));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseHsts();

            app.UseCors(CorsPolicyName);

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}