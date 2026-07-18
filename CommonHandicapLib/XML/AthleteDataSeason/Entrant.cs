namespace CommonHandicapLib.XML.AthleteDataSeason
{
    using System.Xml.Serialization;

    /// <summary>
    /// Simple row object which represents a single row in the athlete details (season) table XML 
    /// file.
    /// </summary>
    public class Entrant
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
        /// All appearances for the athlete.
        /// </summary>
        private EntrantTimesRoot entrantTimes;

        /// <summary>
        /// All points scored in the mob trophy.
        /// </summary>
        private MobTrophyPointsRoot mobPoints;

        /// <summary>
        /// All points scored in the team trophy.
        /// </summary>
        private TeamTrophyPointsRoot teamPoints;

        /// <summary>
        /// Gets or sets the Key of the athlete in the row.
        /// </summary>
        [XmlAttribute("Key")]
        public int Key
        {
            get => this.key;
            set => this.key = value;
        }

        /// <summary>
        /// Gets or sets the name of the athlete in the row.
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
        /// Gets or sets all times.
        /// </summary>
        [XmlElement("tms")]
        public EntrantTimesRoot Times
        {
            get
            {
                return this.entrantTimes;
            }
            set
            {
                this.entrantTimes = value;
            }
        }

        /// <summary>
        /// Gets or sets all the points scored in the mob trophy.
        /// </summary>
        [XmlElement("hPts")]
        public MobTrophyPoints MobPoints
        {
            get => this.mobPoints;
            set => this.mobPoints = value;
        }

        /// <summary>
        /// Gets or sets all the points scored in the team trophy.
        /// </summary>
        [XmlElement("pts")]
        public MobTeamsPoints TeamPoints
        {
            get => this.teamPoints;
            set => this.teamPoints = value;
        }
    }
}