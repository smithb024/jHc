namespace CommonHandicapLib.XML.AthleteData
{
    using System.Xml.Serialization;

    /// <summary>
    /// A XML row in the athlete data file. This presents the data and time for a specific athlete 
    /// for a specific event.
    /// </summary>
    public class AthleteDataTime
    {
        /// <summary>
        /// The race time recorded for this date.
        /// </summary>
        private string time;

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
            get
            {
                return this.time;
            }

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
            get
            {
                return this.date;
            }

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
