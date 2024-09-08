namespace HandicapModel.Admin.IO
{
    using System.IO;
    using CommonHandicapLib.Interfaces;
    using CommonHandicapLib.Messages;
    using CommonHandicapLib.Types;
    using HandicapModel.Admin.IO.TXT;
    using HandicapModel.Admin.IO.XML;
    using HandicapModel.Interfaces.Admin.IO;
    using HandicapModel.Interfaces.Admin.IO.XML;
    using CommonMessenger = NynaeveLib.Messenger.Messenger;

    /// <summary>
    /// Event data
    /// </summary>
    public class EventData : IEventData
    {
        /// <summary>
        /// The instance of the logger.
        /// </summary>
        private readonly IJHcLogger logger;

        /// <summary>
        /// The event data reader.
        /// </summary>
        private IEventDataReader eventDataReader;

        /// <summary>
        /// The root directory.
        /// </summary>
        private string rootDirectory;

        /// <summary>
        /// The path to all the season data.
        /// </summary>
        private string dataPath;

        /// <summary>
        /// Initialises a new instance <see cref="EventData"/> class.
        /// </summary>
        /// <param name="logger">application logger</param>
        public EventData(
            IJHcLogger logger)
        {
            this.logger = logger;
            this.eventDataReader =
                new EventDataReader(
                    this.logger);

            this.rootDirectory = RootIO.LoadRootFile();
            this.dataPath = $"{this.rootDirectory}{Path.DirectorySeparatorChar}{IOPaths.dataPath}{Path.DirectorySeparatorChar}";

            CommonMessenger.Default.Register<ReinitialiseRoot>(this, this.ReinitialiseRoot);
        }

        /// <summary>
        /// Saves the event misc details
        /// </summary>
        /// <param name="seasonName">season name</param>
        /// <param name="eventName">event name</param>
        /// <param name="summaryDetails">details to save</param>
        /// <returns>success flag</returns>
        public bool SaveEventData(
            string seasonName,
            string eventName,
            EventMiscData eventData)
        {
            return this.eventDataReader.SaveEventData(
              this.dataPath + seasonName + Path.DirectorySeparatorChar + eventName + Path.DirectorySeparatorChar + IOPaths.eventMiscFile,
              eventData);
        }

        /// <summary>
        /// Reads the event details.
        /// </summary>
        /// <param name="seasonName">season name</param>
        /// <param name="eventName">event name</param>
        /// <returns>decoded event data</returns>
        public EventMiscData LoadEventData(
            string seasonName,
            string eventName)
        {
            return this.eventDataReader.LoadEventData(
              this.dataPath + seasonName + Path.DirectorySeparatorChar + eventName + Path.DirectorySeparatorChar + IOPaths.eventMiscFile);
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