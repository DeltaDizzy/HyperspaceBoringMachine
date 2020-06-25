using Discord;
using Discord.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HyperspaceBoringMachine.Modules
{
    public class ModuleED : ModuleBase<SocketCommandContext>
    {
        static Dictionary<ulong, int> UserTimeZones;
        [Command("online")]
        public Task OnlineCMDRsAsync()
        {
            StringBuilder onlineCMDRs = new StringBuilder();
            IGuildUser[] users = Context.Guild.Users.ToArray(); 
            for (int i = 0; i < users.Length; i++)
            {
                onlineCMDRs.Append(String.Format("{0} is currently {1} Elite: Dangerous.{2}", users[i].Username, IsUserPlaying(users[i]), Environment.NewLine));
            }
            Context.Channel.SendMessageAsync(onlineCMDRs.ToString());
            return Task.CompletedTask;
        }

        [Command("convert")]
        public Task ConvertTimeZones(string time, string fromZone, string toZone)
        {
            DateTime msgTime = DateTime.Parse(time);

            //Context.Channel.SendMessageAsync(msgTime.ToString());
            
            return Task.CompletedTask;
        }

        [Command("tzset")]
        public Task SetUserTimeZone(string offset)
        {
            if (UserTimeZones == null)
            {
                UserTimeZones = new Dictionary<ulong, int>();
            }
            UserTimeZones.Add(ulong.Parse(Context.User.Id.ToString()), int.Parse(offset));
            return Task.CompletedTask;
        }

        public static void SaveUserTimeZoneDictionary()
        {
            string output = JsonConvert.SerializeObject(UserTimeZones);
            File.WriteAllText(Constants.configDirPath + @"\userTimeZones.txt", output);
        }

        public static void LoadUserTimeZoneDictionary()
        {
            if (File.Exists(Constants.configDirPath + @"\userTimeZones.txt"))
            {
                UserTimeZones = (Dictionary<ulong, int>)JsonConvert.DeserializeObject(File.ReadAllText(Constants.configDirPath + @"\userTimeZones.txt"));
            }
        }

        string IsUserPlaying(IGuildUser user)
        {
            if (user.Activity == null) return "not playing";

            if (user.Activity.Type == ActivityType.Playing && user.Activity.Name == "Elite: Dangerous")
            {
                return "playing";
            }
            else
            {
                return "not playing";
            }
        }
    }
}
