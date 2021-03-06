﻿namespace FrontEnd
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;

    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            app.UseDefaultFiles(new DefaultFilesOptions
            {
                RequestPath = "/frontend"
            });
            app.UseStaticFiles(new StaticFileOptions
            {
                RequestPath = "/frontend"
            });
        }
    }
}
