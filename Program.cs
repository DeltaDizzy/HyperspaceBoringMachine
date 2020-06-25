using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Net.Http;
using System.Runtime.InteropServices;
using HyperspaceBoringMachine.Services;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Reflection;

namespace HyperspaceBoringMachine
{
    /// <summary>
    /// Initalizes and handles core bot functions
    /// </summary>
    public class Program
    {

        static void Main(string[] args)
        { new Program().MainAsync().GetAwaiter().GetResult(); }

        /// <summary>
        /// Sets up services and logs in to the Discord Gateway
        /// </summary>
        /// <returns></returns>
        public async Task MainAsync()
        {
            var services = SetupServices();
            var client = services.GetRequiredService<DiscordSocketClient>();
            client.Log += LogAsync;
            services.GetRequiredService<CommandService>().Log += LogAsync;
            Modules.ModuleED.LoadUserTimeZoneDictionary();
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(App_Exit);

            await client.LoginAsync(TokenType.Bot, File.ReadAllText(Constants.configDirPath + @"\token.txt"));
            await client.SetGameAsync("with Time Zones");
            await client.StartAsync();

            await services.GetRequiredService<CommandHandlerService>().InitAsync();
            await Task.Delay(Timeout.Infinite);
        }

        private Task LogAsync(LogMessage log)
        {
            Console.WriteLine(log.ToString());
            return Task.CompletedTask;
        }


        private ServiceProvider SetupServices()
        {
            return new ServiceCollection()
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandlerService>()
                .AddSingleton<HttpClient>()
                .BuildServiceProvider();
        }

        static void App_Exit(object sender, EventArgs e)
        {
            Modules.ModuleED.SaveUserTimeZoneDictionary();
        }
    }
}
