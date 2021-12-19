namespace HandicapModel.Admin.IO
{
    using System.IO;
    using CommonHandicapLib.Interfaces;
    using CommonHandicapLib.Types;
    using HandicapModel.Admin.IO.XML;
    using HandicapModel.Interfaces.Admin.IO;
    using HandicapModel.Interfaces.Admin.IO.XML;

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
              RootPath.DataPath + seasonName + Path.DirectorySeparatorChar + eventName + Path.DirectorySeparatorChar + IOPaths.eventMiscFile,
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
              RootPath.DataPath + seasonName + Path.DirectorySeparatorChar + eventName + Path.DirectorySeparatorChar + IOPaths.eventMiscFile);
        }
    }
}