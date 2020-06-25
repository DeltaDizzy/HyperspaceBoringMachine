using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HyperspaceBoringMachine
{
    /// <summary>
    /// Holds all constant values
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// unused
        /// </summary>
        public static readonly string configDirPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

        /// <summary>
        /// The Discord snowflake ID for the events channel
        /// </summary>
        public static readonly ulong eventsChannelId = 0;
    }
}
