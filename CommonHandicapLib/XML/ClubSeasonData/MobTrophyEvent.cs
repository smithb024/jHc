namespace CommonHandicapLib.XML.ClubSeasonData
{
    using System.Xml.Serialization;

    /// <summary>
    /// A XML row in the club season data file. This presents an event entered by the club in the 
    /// mob trophy.
    /// </summary>
    public class MobTrophyEvent
    {
        /// <summary>
        /// The size of the team.
        /// </summary>
        private int teamSize;

        /// <summary>
        /// The points scored by a virtual runner.
        /// </summary>
        private int virtualRunnerScore;

        /// <summary>
        /// The date of the event.
        /// </summary>
        private string date;

        /// <summary>
        /// The team score.
        /// </summary>
        private int score;

        /// <summary>
        /// All club points.
        /// </summary>
        private MobTrophyPoints points;

        /// <summary>
        /// Gets or sets size of the team in the current event.
        /// </summary>
        [XmlAttribute("tm")]
        public int TeamSize
        {
            get => this.teamSize;
            set => this.teamSize = value;
        }

        /// <summary>
        /// Gets or sets points scored by a virtual runner.
        /// </summary>
        [XmlAttribute("vr")]
        public int VirtualRunnerScore
        {
            get => this.virtualRunnerScore;
            set => this.virtualRunnerScore = value;
        }

        /// <summary>
        /// Gets or sets the date of the event.
        /// </summary>
        [XmlAttribute("dt")]
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

        /// <summary>
        /// Gets or sets the teams score.
        /// </summary>
        [XmlAttribute("scr")]
        public int Score
        {
            get => this.score;
            set => this.score = value;
        }

        /// <summary>
        /// Gets or sets all club points.
        /// </summary>
        [XmlElement("pt")]
        public MobTrophyPoints Points
        {
            get => this.points;
            set => this.points = value;
        }

    }
}
