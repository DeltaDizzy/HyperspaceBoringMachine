using Discord;
using Discord.Commands;
using Discord.WebSocket;
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
    /// <summary>
    /// Contains all event code
    /// </summary>
    public class ModuleEvents : ModuleBase<SocketCommandContext>
    {
        static List<EventData> squadronEvents = new List<EventData>();
        static List<Timer> eventTimers = new List<Timer>();
        List<SocketGuildChannel> channelList;

        /// <summary>
        /// Responsible for everything related to events, including creation and modification
        /// </summary>
        /// <param name="op">The operation to execute. Can be 'create' or 'edit'</param>
        /// <param name="name">Title of the event</param>
        /// <param name="time">The time the event takes place at</param>
        /// <param name="roles">The roles to mention when the award takes place</param>
        /// <returns>Task.CompletedTask</returns>
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
                TimeSpan t = DateTime.Parse(time).Subtract(DateTime.UtcNow);

                CreateEvent(new EventData(name, t, roleArray));
            }
            return Task.CompletedTask;
        }

        void CreateEvent(EventData data)
        {
            TimeSpan timeToEvent = data.time;
            squadronEvents.Add(data);
            var ti = new Timer(new TimerCallback(PostEvent));

        }

        void UpdateEventTime(int id, TimeSpan time)
        {
            EventData d = squadronEvents.Find(e => e.id == id);
            d.time = time;

        }

        /// <summary>
        /// Serializes and saves the list of events
        /// </summary>
        public static void SaveEventList()
        {
            string output = JsonConvert.SerializeObject(squadronEvents);
            File.WriteAllText(Constants.configDirPath + @"\events.txt", output);
        }

        /// <summary>
        /// Loads and deserializes the list of events
        /// </summary>
        public static void LoadEventList()
        {
            if (File.Exists(Constants.configDirPath + @"\events.txt"))
            {
                squadronEvents = (List<EventData>)JsonConvert.DeserializeObject(File.ReadAllText(Constants.configDirPath + @"\events.txt"));
            }
        }

        void PostEvent(object state)
        {
            Console.WriteLine(state);
            // find event
            EventData data = squadronEvents.Find(e => e.time.TotalSeconds == 0);
            // find channel
            var channel = Context.Guild.GetTextChannel(Constants.eventsChannelId);
            channel.SendMessageAsync($"{data.name} is now starting!");
            // send message
            

        }

        class EventData
        {
            public string name;
            public TimeSpan time;
            public IRole[] roles;
            public int id;
            public Timer timer;

            public EventData(string name, TimeSpan time, IRole[] roles)
            {
                this.name = name;
                this.time = time;
                this.roles = roles;
            }

            public EventData(string name, TimeSpan time, IRole[] roles, int id)
            {
                this.name = name;
                this.time = time;
                this.roles = roles;
                this.id = id;
            }

            public EventData(string name, TimeSpan time, IRole[] roles, int id, Timer timer)
            {
                this.name = name;
                this.time = time;
                this.roles = roles;
                this.id = id;
                this.timer = timer;
            }
        }
    }
}
