using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TelegrammAspMvcDotNetCoreBot.Models;
using TelegrammAspMvcDotNetCoreBot.Controllers;
using System.Net;
using System.IO;

namespace TelegrammAspMvcDotNetCoreBot
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

			//Thread thread = new Thread(new ThreadStart(Function));
			//thread.IsBackground = true;
			//thread.Name = "Function";
			//thread.Start();
			ScheduleUpdateController.Update();
		}

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
		}

		protected static void Function()
		{
			System.Timers.Timer timer = new System.Timers.Timer();
			timer.Elapsed += new System.Timers.ElapsedEventHandler(TimerEvent);
			timer.Interval = 5000;
			timer.Enabled = true;
			timer.AutoReset = true;
			timer.Start();
		}

		protected static void TimerEvent(object sender, System.Timers.ElapsedEventArgs e)
		{
			
			ScheduleUpdateController.Update();

			if (DateTime.Now.Minute == 1)
			{
				HttpWebRequest req = (HttpWebRequest)WebRequest.Create("http://misis.ru/files/-/01a3d5fd557b752504a92686c0f486b4/%D0%98%D0%A2%D0%90%D0%A1%D0%A3.xls");
				HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
				Stream stream = resp.GetResponseStream();
				FileStream file = new FileStream(@"sch.xls", FileMode.Create, FileAccess.Write);
				StreamWriter write = new StreamWriter(file);
				int b;
				for (int i = 0; ; i++)
				{
					b = stream.ReadByte();
					if (b == -1) break;
					write.Write((char)b);
				}
				write.Close();
				file.Close();
			}
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            // обработка ошибок HTTP
            app.UseStatusCodePages();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            //Bot Configurations
            Bot.GetBotClientAsync().Wait();
        }
    }
}
