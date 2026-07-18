namespace CommonHandicapLib.XML.AthleteDataSeason
{
    using System.Xml.Serialization;

    /// <summary>
    /// Simple object which represents a collection of entrants.
    /// </summary>
    public class EntrantList
    {
        /// <summary>
        /// All known entrants.
        /// </summary>
        private EntrantCollection allEntrants;

        /// <summary>
        /// Initialises a new instance of the <see cref="EntrantList"/> class.
        /// </summary>
        public EntrantList()
        {
            this.AllEntrants = new EntrantCollection();
        }

        /// <summary>
        /// Gets or sets a collection of all known entrants.
        /// </summary>
        [XmlElement("entrant")]
        public EntrantCollection AllEntrants
        {
            get => this.allEntrants;
            set => this.allEntrants = value;
        }
    }
}