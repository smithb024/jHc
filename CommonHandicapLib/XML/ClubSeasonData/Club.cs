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
        /// All points scored by the club (team).
        /// </summary>
        private TeamTrophyPointsRoot clubPoints;

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
        /// Gets or sets all the points scored by the club.
        /// </summary>
        [XmlElement("pts")]
        public TeamTrophyPointsRoot ClubPoints
        {
            get => this.clubPoints;
            set => this.clubPoints = value;
        }

    }
}
