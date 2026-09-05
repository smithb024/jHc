namespace CommonHandicapLib.XML.ClubSeasonData
{
    using System.Xml.Serialization;

    /// <summary>
    /// XML serialisable class which contains all the mob trophy scores of a specific athlete.
    /// </summary>
    public class ClubPointsRoot
    {
        /// <summary>
        /// All club points.
        /// </summary>
        private ClubPoints points;

        /// <summary>
        /// Gets or sets all club points.
        /// </summary>
        [XmlElement("pt")]
        public ClubPoints Points
        {
            get => this.points;
            set => this.points = value;
        }
    }
}
