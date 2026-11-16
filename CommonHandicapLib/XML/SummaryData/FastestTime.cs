namespace CommonHandicapLib.XML.SummaryData
{
    using System.Xml.Serialization;

    /// <summary>
    /// A XML row in a summary data file. This presents a piece of fastest time information.
    /// </summary>
    public class FastestTime
    {
        /// <summary>
        /// The key for this athlete.
        /// </summary>
        private int key;

        /// <summary>
        /// The athlete's name.
        /// </summary>
        private string name;

        /// <summary>
        /// The minutes taken in this fastest time.
        /// </summary>
        private int minutes;

        /// <summary>
        /// The seconds taken in this fastest time.
        /// </summary>
        private int seconds;

        /// <summary>
        /// The date of the fastest time.
        /// </summary>
        private string date;

        /// <summary>
        /// Gets or sets the Key of this athlete.
        /// </summary>
        [XmlAttribute("Key")]
        public int Key
        {
            get => this.key;
            set => this.key = value;
        }

        /// <summary>
        /// Gets or sets the name of this athlete.
        /// </summary>
        [XmlAttribute("Name")]
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
        /// Gets or sets the minutes taken in this fastest time.
        /// </summary>
        [XmlAttribute("Minutes")]
        public int Minutes
        {
            get => this.minutes;
            set => this.minutes = value;
        }

        /// <summary>
        /// Gets or sets the seconds taken in this fastest time.
        /// </summary>
        [XmlAttribute("Seconds")]
        public int Seconds
        {
            get => this.seconds;
            set => this.seconds = value;
        }

        /// <summary>
        /// Gets or sets the date of the fastest time.
        /// </summary>
        [XmlAttribute("Date")]
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
