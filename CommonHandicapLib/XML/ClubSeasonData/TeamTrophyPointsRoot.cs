namespace CommonHandicapLib.XML.ClubSeasonData
{
    using System.Xml.Serialization;

    /// <summary>
    /// XML serialisable class which contains all the mob trophy scores of a specific athlete.
    /// </summary>
    public class TeamTrophyPointsRoot
    {
        /// <summary>
        /// All club points.
        /// </summary>
        private TeamTrophyPoints points;

        /// <summary>
        /// Gets or sets all club points.
        /// </summary>
        [XmlElement("pt")]
        public TeamTrophyPoints Points
        {
            get => this.points;
            set => this.points = value;
        }
    }
}
