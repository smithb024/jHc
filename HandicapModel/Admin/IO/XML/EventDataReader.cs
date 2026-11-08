namespace HandicapModel.Admin.IO.XML
{
    using CommonHandicapLib.Interfaces;
    using CommonHandicapLib.Types;
    using CommonHandicapLib.XML.EventsData;
    using CommonLib.Types;
    using HandicapModel.Interfaces.Admin.IO.XML;
    using NynaeveLib.XML;
    using System;

    /// <summary>
    /// The event data reader
    /// </summary>
    internal class EventDataReader : IEventDataReader
    {
        /// <summary>
        /// The instance of the logger.
        /// </summary>
        private readonly IJHcLogger logger;

        /// <summary>
        /// Initialises a new instance of the <see cref="EventDataReader"/> class.
        /// </summary>
        /// <param name="logger"></param>
        public EventDataReader(IJHcLogger logger)
        {
            this.logger = logger;
        }

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>SaveEventData</name>
        /// <date>04/03/15</date>
        /// <summary>
        /// Save the event data.
        /// </summary>
        /// <param name="fileName">file name</param>
        /// <param name="date">event date</param>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        public bool SaveEventData(
            string fileName,
            EventMiscData eventData)
        {
            bool success = true;
            EventDetailsRoot saveEvent = new EventDetailsRoot();

            try
            {
                saveEvent.Date = eventData.EventDate.ToString();

                XmlFileIo.WriteXml<EventDetailsRoot>(
                    saveEvent,
                    fileName);
            }
            catch (Exception ex)
            {
                this.logger.WriteLog("Error saving event file: " + ex.ToString());
                success = false;
            }

            return success;
        }

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>LoadResultsTable</name>
        /// <date>04/03/15</date>
        /// <summary>
        /// Reads the event details xml from file and decodes it.
        /// </summary>
        /// <param name="fileName">name of xml file</param>
        /// <returns>decoded event data</returns>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        public EventMiscData LoadEventData(string fileName)
        {
            EventMiscData eventData = new EventMiscData();

            try
            {
                EventDetailsRoot eventDetailsRoot =
                    XmlFileIo.ReadXml<EventDetailsRoot>(
                        fileName);
                eventData.EventDate =
                    new DateType(
                        eventDetailsRoot.Date);
            }
            catch (Exception ex)
            {
                this.logger.WriteLog("Error reading event data: " + ex.ToString());

                eventData = new EventMiscData();
            }

            return eventData;
        }
    }
}