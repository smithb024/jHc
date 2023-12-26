namespace CommonHandicapLib.XML.AthleteData
{
    using System.Xml.Serialization;

    /// <summary>
    /// Simple object which represents a collection of athletes.
    /// </summary>
    public class AthleteList
    {
        /// <summary>
        /// All known athletes.
        /// </summary>
        private AthleteCollection allAthletes;

        /// <summary>
        /// Initialises a new instance of the <see cref="AthleteList"/> class.
        /// </summary>
        public AthleteList()
        {
            this.AllAthletes = new AthleteCollection();
        }

        /// <summary>
        /// Gets or sets a collection of all known athletes.
        /// </summary>
        [XmlElement("Athlete")]
        public AthleteCollection AllAthletes
        {
            get => this.allAthletes;
            set => this.allAthletes = value;
        }
    }
}