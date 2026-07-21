namespace CommonHandicapLib.XML.AthleteDataSeason
{
    using System.Xml.Serialization;

    /// <summary>
    /// Simple row object which represents a single points row in the teams trophy.
    /// </summary>
    public class TeamTrophyPoint
    {
        /// <summary>
        /// The race points scored in the team trophy.
        /// </summary>
        private int points;

        /// <summary>
        /// The date of the event.
        /// </summary>
        private string date;

        /// <summary>
        /// Gets or sets the points scored in the team trophy.
        /// </summary>
        [XmlAttribute("pts")]
        public int Points
        {
            get => this.points;
            set => this.points = value;
        }

        /// <summary>
        /// Gets or sets the date of the event.
        /// </summary>
        [XmlAttribute("date")]
        public string Date
        {
            get => this.date;

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    this.date = string.Empty;
                }
                else
                {
                    this.date = value;
                }
            }
        }
    }
}
