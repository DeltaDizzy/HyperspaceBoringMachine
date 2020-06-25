using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HyperspaceBoringMachine.Services
{
    public class CommandHandlerService
    {
        private readonly CommandService _commands;
        private readonly DiscordSocketClient _discord;
        private readonly IServiceProvider _services;

        public CommandHandlerService(IServiceProvider services)
        {
            _commands = services.GetRequiredService<CommandService>();
            _discord = services.GetRequiredService<DiscordSocketClient>();
            _services = services;

            _commands.CommandExecuted += CommandExecutedAsync;
            _discord.MessageReceived += MessageRecievedAsync;
        }

        public async Task InitAsync()
        {
            // register modules
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        public async Task MessageRecievedAsync(SocketMessage rawMessage)
        {
            // ignore clyde and bots
            if (!(rawMessage is SocketUserMessage message)) return;
            if (message.Source != MessageSource.User) return;

            // index prefix ends at
            int argPos = 1;

            if (!message.HasStringPrefix("h.", ref argPos)) return;

            var context = new SocketCommandContext(_discord, message);

            // parse and execute command
            await _commands.ExecuteAsync(context, argPos, _services);
        }

        // post-execution logic
        public async Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            if (!command.IsSpecified)
            {
                
            }
            if (!result.IsSuccess)
            {
                await context.Channel.SendMessageAsync($"error: {result}");
            }

            
        }
    }
}
