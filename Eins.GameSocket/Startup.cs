using Eins.GameSocket.Hubs;
using Eins.TransportEntities.Eins;
using Eins.TransportEntities.Lobby;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eins.GameSocket
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();

            //Key = LobbyID
            services.AddSingleton(new ConcurrentDictionary<ulong, Game>());

            //Key = ID
            //ID = Snowflake -> soon™
            //Snowflake -> Similar IDs like discord
            //42 Bits timestamp (in miliseconds since some Date, maybe 01.01.2021 0:00:00)
            //10 bits Machine ID (basically 0 for now)
            //12 Bits for sequence (goes up if 2 are generated at the same time)
            services.AddSingleton(new ConcurrentDictionary<ulong, Lobby>());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<EinsGameHub>("/game");
            });
        }
    }
}
