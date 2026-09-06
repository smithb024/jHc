namespace CommonHandicapLib.XML.ClubSeasonData
{
    using CommonHandicapLib.XML.AthleteDataSeason;
    using System.Xml.Serialization;

    /// <summary>
    /// Simple row object which represents a single row in the club details (season) table XML 
    /// file.
    /// </summary>
    public class Club
    {
        /// <summary>
        /// The athlete's name.
        /// </summary>
        private string name;

        /// <summary>
        /// All team trophy points scored by the club.
        /// </summary>
        private TeamTrophyPointsRoot teamPoints;

        /// <summary>
        /// All mob trophy points scored by the club.
        /// </summary>
        private MobTrophyPointsRoot mobPoints;

        /// <summary>
        /// Gets or sets the name of the club in the row.
        /// </summary>
        [XmlAttribute("name")]
        public string Name
        {
            get => this.name;

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    this.name = string.Empty;
                }
                else
                {
                    this.name = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets all the team trophy points scored by the club.
        /// </summary>
        [XmlElement("pts")]
        public TeamTrophyPointsRoot TeamPoints
        {
            get => this.teamPoints;
            set => this.teamPoints = value;
        }

        /// <summary>
        /// Gets or sets all the mob trophy points scored by the club.
        /// </summary>
        [XmlElement("hPts")]
        public MobTrophyPointsRoot MobPoints
        {
            get => this.mobPoints;
            set => this.mobPoints = value;
        }
    }
}
