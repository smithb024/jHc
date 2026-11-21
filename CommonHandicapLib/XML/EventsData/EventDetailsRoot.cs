namespace CommonHandicapLib.XML.EventsData
{
    using System.Xml.Serialization;

    /// <summary>
    /// Serialiseable class. This provides the base class for the event details file.
    /// </summary>
    [XmlRoot("Evt")]
    public class EventDetailsRoot
    {
        /// <summary>
        /// Gets or sets the date of the event.
        /// </summary>
        [XmlElement("Date")]
        public string Date { get; set; }
    }
}
