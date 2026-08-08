namespace CommonHandicapLib.XML.AthleteDataSeason
{
    using System.Xml.Serialization;

    /// <summary>
    /// XML serialisable class which contains all the mob trophy scores of a specific athlete.
    /// </summary>
    public class MobTrophyPointsRoot
    {
        /// <summary>
        /// All mob trophy points for the athlete.
        /// </summary>
        private MobTrophyPoints points;

        /// <summary>
        /// All mob trophy points.
        /// </summary>
        [XmlElement("pt")]
        public MobTrophyPoints Points
        {
            get => this.points;
            set => this.points = value;
        }
    }
}
