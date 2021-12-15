namespace HandicapModel.Interfaces.Admin.IO.XML
{
    using CommonHandicapLib.Types;

    /// <summary>
    /// The event data reader
    /// </summary>
    public interface IEventDataReader
    {
        /// Save the event data.
        /// </summary>
        /// <param name="fileName">file name</param>
        /// <param name="date">event date</param>
        bool SaveEventData(
            string fileName,
            EventMiscData eventData);

        /// <summary>
        /// Reads the event details xml from file and decodes it.
        /// </summary>
        /// <param name="fileName">name of xml file</param>
        /// <returns>decoded event data</returns>
        EventMiscData LoadEventData(string fileName);
    }
}