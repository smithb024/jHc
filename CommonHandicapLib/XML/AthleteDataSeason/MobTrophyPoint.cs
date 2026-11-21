namespace CommonHandicapLib.XML.AthleteDataSeason
{
    using System.Xml.Serialization;

    /// <summary>
    /// A XML row in the seson athlete data file. This presents the points scored in the mob
    /// trophy competition.
    /// </summary>
    public class MobTrophyPoint
    {
        /// <summary>
        /// The finishing points scored in the mob trophy.
        /// </summary>
        private int finishingPoints;

        /// <summary>
        /// The position points scored in the mob trophy.
        /// </summary>
        private int positionPoints;

        /// <summary>
        /// The year best points scored in the mob trophy.
        /// </summary>
        private int ybPoints;

        /// <summary>
        /// The date of the event.
        /// </summary>
        private string date;

        /// <summary>
        /// Gets or sets the finishing points scored in the mob trophy.
        /// </summary>
        [XmlAttribute("fpts")]
        public int FinishingPoints
        {
            get => this.finishingPoints;
            set => this.finishingPoints = value;
        }

        /// <summary>
        /// Gets or sets the position points scored in the mob trophy.
        /// </summary>
        [XmlAttribute("ppts")]
        public int PositionPoints
        {
            get => this.positionPoints;
            set => this.positionPoints = value;
        }

        /// <summary>
        /// Gets or sets the year best points scored in the mob trophy.
        /// </summary>
        [XmlAttribute("bpts")]
        public int YbPoints
        {
            get => this.ybPoints;
            set => this.ybPoints = value;
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
