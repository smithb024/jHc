namespace HandicapModel.Admin.IO.TXT
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using CommonHandicapLib.Interfaces;
    using CommonHandicapLib.Messages;
    using HandicapModel.Interfaces.Admin.IO.TXT;
    using GalaSoft.MvvmLight.Messaging;

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
        /// The root directory.
        /// </summary>
        private string rootDirectory;

        /// <summary>
        /// The path to all the season data.
        /// </summary>
        private string dataPath;

        /// <summary>
        /// Initialises a new instance of the <see cref="EventIO"/> class.
        /// </summary>
        /// <param name="logger">application logger</param>
        public EventIO(IJHcLogger logger)
        {
            this.logger = logger;
            this.rootDirectory = RootIO.LoadRootFile();
            this.dataPath = $"{this.rootDirectory}{Path.DirectorySeparatorChar}{IOPaths.dataPath}{Path.DirectorySeparatorChar}";

            Messenger.Default.Register<ReinitialiseRoot>(this, this.ReinitialiseRoot);
        }

        /// <summary>
        /// Returns a list of all events.
        /// </summary>
        /// <param name="season">event's season</param>
        /// <returns>list of all events in the season</returns>
        public List<string> GetEvents(string season)
        {
            List<string> events = new List<string>();

            if (string.IsNullOrEmpty(season))
            {
                return events;
            }

            string[] eventsArray = Directory.GetDirectories(this.dataPath + season);

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
                Directory.CreateDirectory(this.dataPath + seasonName + '\\' + eventName);
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
                if (File.Exists(this.dataPath + seasonName + Path.DirectorySeparatorChar + IOPaths.currentEvent))
                {
                    using (StreamReader reader = new StreamReader(this.dataPath + seasonName + Path.DirectorySeparatorChar + IOPaths.currentEvent))
                    {
                        return reader.ReadLine() ?? string.Empty;
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
                    this.dataPath + seasonName + Path.DirectorySeparatorChar + IOPaths.currentEvent,
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

        /// <summary>
        /// Reinitialise the data path value from the file.
        /// </summary>
        /// <param name="message">reinitialise message</param>
        private void ReinitialiseRoot(ReinitialiseRoot message)
        {
            this.rootDirectory = RootIO.LoadRootFile();
            this.dataPath = $"{this.rootDirectory}{Path.DirectorySeparatorChar}{IOPaths.dataPath}{Path.DirectorySeparatorChar}";
        }
    }
}