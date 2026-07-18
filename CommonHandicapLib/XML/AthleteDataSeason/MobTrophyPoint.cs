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
        /// The points recorded for this date.
        /// </summary>
        private string points;

        /// <summary>
        /// The date of the event.
        /// </summary>
        private string date;

        /// <summary>
        /// Gets or sets the time of the athlete.
        /// </summary>
        [XmlAttribute("rtm")]
        public string Time
        {
            get => this.time;

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    this.time = string.Empty;
                }
                else
                {
                    this.time = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the date of the event.
        /// </summary>
        [XmlAttribute("rdt")]
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
