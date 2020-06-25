using Discord;
using Discord.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HyperspaceBoringMachine.Modules
{
    public class ModuleEvents : ModuleBase<SocketCommandContext>
    {
        static List<EventData> squadronEvents = new List<EventData>();


        [Command("event")]
        public Task EventCommand(string op, string name, string time, [Remainder]string roles)
        {
            string[] rolesInput = roles.Split(' '); // seperate role names
            for (int i = 0; i < rolesInput.Length; i++)
            {
                rolesInput[i] = rolesInput[i].ToLower();
            }
            if (rolesInput.Contains("everyone"))
            {

            }
            IRole[] roleArray = new IRole[5];
            for (int i = 0; i < roleArray.Length; i++)
            {
                roleArray[i] = Context.Guild.Roles.Where(r => r.Name.ToLower() == rolesInput[i]).FirstOrDefault();
            }
            if (op.ToLower() == "create")
            {
                // get datetime
                DateTime d = DateTime.Parse(time);
                TimeSpan t = d.

                CreateEvent(new EventData(name, , roleArray), new Action(PostEvent));
            }
            return Task.CompletedTask;
        }

        void CreateEvent(EventData data, Action action)
        {
            DateTime eventTime = data.time;
            squadronEvents.Add(squadronEvents.Count - 1,);
        }

        public static void SaveEventDictionary()
        {
            string output = JsonConvert.SerializeObject(squadronEvents);
            File.WriteAllText(Constants.configDirPath + @"\events.txt", output);
        }

        public static void LoadEventDictionary()
        {
            if (File.Exists(Constants.configDirPath + @"\events.txt"))
            {
                squadronEvents = (List<EventData>)JsonConvert.DeserializeObject(File.ReadAllText(Constants.configDirPath + @"\events.txt"));
            }
        }

        void PostEvent()
        {
            // find event
            EventData data = squadronEvents.Find(e => e.time.)
            Context.Channel.SendMessageAsync($"Event {}");
        }

        class EventData
        {
            public string name;
            public TimeSpan time;
            public IRole[] roles;

            public EventData(string name, TimeSpan time, IRole[] roles)
            {
                this.name = name;
                this.time = time;
                this.roles = roles;
            }
        }
    }
}
