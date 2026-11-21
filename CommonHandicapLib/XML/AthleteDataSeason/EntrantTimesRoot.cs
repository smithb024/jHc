namespace CommonHandicapLib.XML.AthleteDataSeason
{
    using System.Xml.Serialization;

    /// <summary>
    /// XML serialisable class which contains all the appearances of a specific athlete.
    /// </summary>
    public class EntrantTimesRoot
    {
        /// <summary>
        /// All appearances for the athlete.
        /// </summary>
        private EntrantTimes appearances;

        /// <summary>
        /// All appearances.
        /// </summary>
        [XmlElement("time")]
        public EntrantTimes Appearances
        {
            get => this.appearances;
            set => this.appearances = value;
        }
    }
}
