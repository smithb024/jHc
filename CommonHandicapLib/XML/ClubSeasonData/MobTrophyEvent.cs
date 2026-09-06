namespace CommonHandicapLib.XML.ClubSeasonData
{
    using System.Xml.Serialization;

    /// <summary>
    /// A XML row in the club season data file. This presents an event entered by the club in the 
    /// mob trophy.
    /// </summary>
    public class MobTrophyEvent
    {
        /// <summary>
        /// All club points.
        /// </summary>
        private MobTrophyPoints points;

        /// <summary>
        /// Gets or sets all club points.
        /// </summary>
        [XmlElement("pt")]
        public MobTrophyPoints Points
        {
            get => this.points;
            set => this.points = value;
        }

    }
}
