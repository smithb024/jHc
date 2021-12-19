namespace HandicapModel.Admin.IO.TXT
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using CommonHandicapLib.Interfaces;
    using HandicapModel.Interfaces.Admin.IO.TXT;

    /// <summary>
    /// Event specific IO.
    /// </summary>
    public class EventIO : IEventIo
    {
        /// <summary>
        /// Application logger
        /// </summary>
        private readonly IJHcLogger logger;

        /// <summary>
        /// Initialises a new instance of the <see cref="EventIO"/> class.
        /// </summary>
        /// <param name="logger">application logger</param>
        public EventIO(IJHcLogger logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Returns a list of all events.
        /// </summary>
        /// <param name="season">event's season</param>
        /// <returns>list of all events in the season</returns>
        public List<string> GetEvents(string season)
        {
            List<string> events = new List<string>();
            string[] eventsArray = System.IO.Directory.GetDirectories(RootPath.DataPath + season);

            foreach (string occasion in eventsArray)
            {
                events.Add(occasion.Substring(occasion.LastIndexOf('\\') + 1));
            }

            return events;
        }

        /// <summary>
        /// Creates a directory for a new event
        /// </summary>
        /// <param name="eventName">new event</param>
        /// <param name="seasonName">event's season</param>
        /// <returns>success flag</returns>
        public bool CreateNewEvent(
            string seasonName,
            string eventName)
        {
            try
            {
                Directory.CreateDirectory(RootPath.DataPath + seasonName + '\\' + eventName);
            }
            catch (Exception ex)
            {
                this.logger.WriteLog("Can't create new event: " + ex.ToString());
                return false;
            }

            return true;
        }

        /// <summary>
        /// Loads the current event name.
        /// </summary>
        /// <returns>current event</returns>
        public string LoadCurrentEvent(string seasonName)
        {
            try
            {
                if (File.Exists(RootPath.DataPath + seasonName + Path.DirectorySeparatorChar + IOPaths.currentEvent))
                {
                    using (StreamReader reader = new StreamReader(RootPath.DataPath + seasonName + Path.DirectorySeparatorChar + IOPaths.currentEvent))
                    {
                        return reader.ReadLine();
                    }
                }
            }
            catch (Exception ex)
            {
                this.logger.WriteLog("Error, failed to read current event: " + ex.ToString());
            }

            return string.Empty;
        }

        /// <summary>
        /// Saves the current event name.
        /// </summary>
        /// <param name="season">current event</param>
        /// <returns>success flag</returns>
        public bool SaveCurrentEvent(
            string seasonName,
            string eventName)
        {
            bool success = false;

            try
            {
                using (
                  StreamWriter writer =
                  new StreamWriter(
                    RootPath.DataPath + seasonName + Path.DirectorySeparatorChar + IOPaths.currentEvent,
                    false))
                {
                    writer.Write(eventName);
                    success = true;
                }
            }
            catch (Exception ex)
            {
                this.logger.WriteLog("Error, failed to save current season: " + ex.ToString());
            }

            return success;
        }
    }
}