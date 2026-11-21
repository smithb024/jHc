namespace CommonHandicapLib.XML.ClubData
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    /// <summary>
    /// Serialiseable class. This provides the base class for the club data file.
    /// </summary>
    [XmlRoot("ClubDetails")]
    public class ClubDetailsRoot 
    {
        /// <summary>
        /// Gets or sets the collection of club names.
        /// </summary>
        [XmlElement("club")]
        public List<string> Clubs { get; set; }
    }
}