namespace CommonHandicapLib.XML.ClubSeasonData
{
    using System.Xml.Serialization;

    /// <summary>
    /// XML serialisable class which contains all the team trophy scores of a specific athlete.
    /// </summary>
    public class MobTrophyPointsRoot
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
