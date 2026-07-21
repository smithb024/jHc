namespace CommonHandicapLib.XML.AthleteDataSeason
{
    using System.Xml.Serialization;

    /// <summary>
    /// XML serialisable class which contains all the team trophy scores of a specific athlete.
    /// </summary>
    public class TeamTrophyPointsRoot
    {
        /// <summary>
        /// All team trophy points for the athlete.
        /// </summary>
        private TeamTrophyPoints points;

        /// <summary>
        /// All team trophy points.
        /// </summary>
        [XmlElement("pt")]
        public TeamTrophyPoints Points
        {
            get => this.points;
            set => this.points = value;
        }
    }
}
