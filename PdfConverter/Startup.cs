using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace PdfConverter
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
            services.AddPdfConverter();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var multiplier = app.ApplicationServices.GetRequiredService<TaskMultiplier>();

            var random = new Random();
            var logger = app.ApplicationServices.GetRequiredService<ILogger<Startup>>();

            multiplier.RunAsync(async token =>
            {
                while (true)
                {
                    var time = random.Next(2, 10);

                    logger.LogInformation("{Id}/{TaskId} Waiting {Time} seconds", TaskMultiplier.TaskId, TaskMultiplier.CurrentId, time);

                    await Task.Delay(TimeSpan.FromSeconds(time), token);

                    if (time % 2 == 0)
                    {
                        logger.LogInformation("{Id}/{TaskId} Done", TaskMultiplier.TaskId, TaskMultiplier.CurrentId);
                        return;
                    }
                }
            }, new TaskMultiplierSettings { MaxCount = 5 }, CancellationToken.None).Wait();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
