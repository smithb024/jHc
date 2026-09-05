namespace CommonHandicapLib.XML.ClubSeasonData
{
    using System.Xml.Serialization;

    /// <summary>
    /// A XML row in the club season data file. This presents the points scored by the club.
    /// </summary>
    public class ClubPoint
    {
        /// <summary>
        /// The finishing points scored.
        /// </summary>
        private int finishingPoints;

        /// <summary>
        /// The position points scored.
        /// </summary>
        private int positionPoints;

        /// <summary>
        /// The year best points scored.
        /// </summary>
        private int ybPoints;

        /// <summary>
        /// The date of the event.
        /// </summary>
        private string date;

        /// <summary>
        /// Gets or sets the finishing points scored.
        /// </summary>
        [XmlAttribute("fPt")]
        public int FinishingPoints
        {
            get => this.finishingPoints;
            set => this.finishingPoints = value;
        }

        /// <summary>
        /// Gets or sets the position points scored.
        /// </summary>
        [XmlAttribute("pPt")]
        public int PositionPoints
        {
            get => this.positionPoints;
            set => this.positionPoints = value;
        }

        /// <summary>
        /// Gets or sets the year best points scored.
        /// </summary>
        [XmlAttribute("bPt")]
        public int YbPoints
        {
            get => this.ybPoints;
            set => this.ybPoints = value;
        }

        /// <summary>
        /// Gets or sets the date of the event.
        /// </summary>
        [XmlAttribute("evPt")]
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
