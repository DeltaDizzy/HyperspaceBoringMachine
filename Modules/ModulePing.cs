using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HyperspaceBoringMachine.Modules
{
    public class ModulePing : ModuleBase<SocketCommandContext>
    {
        Dictionary<int, int> UserTimeZones;

        [Command("ping")]
        public Task PingAsync()
        {
            UserTimeZones.Add(0, 1);
            UserTimeZones.Add(2, 3);
            UserTimeZones.Add(4, 5);
            Console.WriteLine(String.Join(Environment.NewLine, UserTimeZones));
            return Task.CompletedTask;
        }
    }
}
