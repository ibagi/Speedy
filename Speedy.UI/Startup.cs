using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using ElectronNET.API;
using ElectronNET.API.Entities;
using Speedy.Core;
using System.Threading;
using System.IO;

namespace Speedy.UI
{
    public class Startup
    {
        private CancellationTokenSource _cancellationTokenSource;

        public Startup()
        {
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSingleton(ctx =>
            {
                var monitorLoop = new NetworkMonitorLoop();
                monitorLoop.Run(_cancellationTokenSource.Token);

                return monitorLoop;
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}"
                );
            });

            BootstrapElectron();
        }

        private async void BootstrapElectron()
        {
            var options = new BrowserWindowOptions
            {
                Title = "Speedy",
                Icon = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot/favicon.ico"),
                Show = false,
            };

            var browserWindow = await Electron.WindowManager.CreateWindowAsync(options);    

            browserWindow.OnReadyToShow += () =>
            {
                browserWindow.Show();      
                browserWindow.SetMenuBarVisibility(false);                
            };
        }
    }
}
