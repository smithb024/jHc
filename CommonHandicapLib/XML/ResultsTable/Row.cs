namespace CommonHandicapLib.XML.ResultsTable
{
    using System;
    using System.Xml.Serialization;
    using CommonLib.Enumerations;

    /// <summary>
    /// Simple row object which represents a single row in the results table XML file.
    /// </summary>
    public class Row
    {
        /// <summary>
        /// The key for this athlete.
        /// </summary>
        private int key;

        /// <summary>
        /// The athlete's club.
        /// </summary>
        private string club;

        /// <summary>
        /// The athlete's handicap.
        /// </summary>
        private string handicap;

        /// <summary>
        /// The athlete's name.
        /// </summary>
        private string name;

        /// <summary>
        /// relevant notes.
        /// </summary>
        private string notes;

        /// <summary>
        /// Extra information.
        /// </summary>
        private string extraInformation;

        /// <summary>
        /// Finishing order.
        /// </summary>
        private int order;

        /// <summary>
        /// Indicates whether this is the athlete's personal best.
        /// </summary>
        private bool personalBest;

        /// <summary>
        /// The points scored in the mob trophy.
        /// </summary>
        private string mobTrophypoints;

        /// <summary>
        /// The points scored in the team trophy.
        /// </summary>
        private int teamTrophyPoints;

        /// <summary>
        /// The athlete's number.
        /// </summary>
        private string number;

        /// <summary>
        /// Used to separate results with the same time.
        /// </summary>
        private int runningOrder;

        /// <summary>
        /// The athlete's time.
        /// </summary>
        private string time;

        /// <summary>
        /// The athlete's sex.
        /// </summary>
        private SexType sex;

        /// <summary>
        /// Indicates whether this is the athlete's year best.
        /// </summary>
        private bool yearBest;

        /// <summary>
        /// Initialises a new instance of the <see cref="Row"/> class.
        /// </summary>
        public Row()
        {
            this.key = -1;
            this.club = string.Empty;
            this.handicap = string.Empty;
            this.name = string.Empty;
            this.notes = string.Empty;
            this.extraInformation = string.Empty;
            this.order = 0;
            this.personalBest = false;
            this.mobTrophypoints = string.Empty;
            this.teamTrophyPoints = 0;
            this.number = string.Empty;
            this.runningOrder = 0;
            this.time = string.Empty;
            this.sex = SexType.NotSpecified;
            this.yearBest = false;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="Row"/> class.
        /// </summary>
        /// <param name="key">the athlete key</param>
        /// <param name="name">the athlete's name</param>
        /// <param name="club">the athlete's club</param>
        /// <param name="handicap">the athlete's handicap</param>
        /// <param name="notes">relevant notes</param>
        /// <param name="extraInformation">extra information</param>
        /// <param name="order">finishing order</param>
        /// <param name="isPersonalBest">indicates whether this is the athlete's personal best</param>
        /// <param name="isYearBest">indicates whether this is the athlete's year best</param>
        /// <param name="mobTrophyPoints">the athlete's points in the Mob Trophy</param>
        /// <param name="teamTrophyPoints">the athlete's points in the Team Trpophy</param>
        /// <param name="number">the athlete's number</param>
        /// <param name="runningOrder">identifies the postion when sharing times</param>
        /// <param name="time">the athlete's time</param>
        /// <param name="sex">the athlete's sex</param>
        public Row(
            int key,
            string name,
            string club,
            string handicap,
            string notes,
            string extraInformation,
            int order,
            bool isPersonalBest,
            bool isYearBest,
            string mobTrophyPoints,
            int teamTrophyPoints,
            string number,
            int runningOrder,
            string time,
            SexType sex)
        {
            this.key = key;
            this.name = name;
            this.club = club;
            this.handicap = handicap;
            this.notes = notes;
            this.extraInformation = extraInformation;
            this.order = order;
            this.personalBest = isPersonalBest;
            this.yearBest = isYearBest;
            this.mobTrophypoints = mobTrophyPoints;
            this.teamTrophyPoints = teamTrophyPoints;
            this.number = number;
            this.runningOrder = runningOrder;
            this.time = time;
            this.sex = sex;
        }

        /// <summary>
        /// Gets or sets the Key of the athlete in the row.
        /// </summary>
        [XmlAttribute("key")]
        public int Key
        {
            get
            {
                return this.key;
            }

            set
            {
                this.key = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the athlete in the row.
        /// </summary>
        [XmlAttribute("name")]
        public string Name
        {
            get
            {
                return this.name;
            }

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
        /// Gets or sets the club of the athlete in the row.
        /// </summary>
        [XmlAttribute("club")]
        public string Club
        {
            get
            {
                return this.club;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    this.club = string.Empty;
                }
                else
                {
                    this.club = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the handicap of the athlete in the row.
        /// </summary>
        [XmlAttribute("HC")]
        public string Handicap
        {
            get
            {
                return this.handicap;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    this.handicap = string.Empty;
                }
                else
                {
                    this.handicap = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the notes of the athlete in the row.
        /// </summary>
        [XmlAttribute("notes")]
        public string Notes
        {
            get
            {
                return this.notes;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    this.notes = string.Empty;
                }
                else
                {
                    this.notes = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the extra information of the athlete in the row.
        /// </summary>
        [XmlAttribute("EI")]
        public string ExtraInformation
        {
            get
            {
                return this.extraInformation;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    this.extraInformation = string.Empty;
                }
                else
                {
                    this.extraInformation = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the points of the athlete in the row.
        /// </summary>
        [XmlAttribute("pts")]
        public string PMobTrophyoints
        {
            get
            {
                return this.mobTrophypoints;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    this.mobTrophypoints = string.Empty;
                }
                else
                {
                    this.mobTrophypoints = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the number of the athlete in the row.
        /// </summary>
        [XmlAttribute("number")]
        public string Number
        {
            get
            {
                return this.number;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    this.number = string.Empty;
                }
                else
                {
                    this.number = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the time of the athlete in the row.
        /// </summary>
        [XmlAttribute("time")]
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
        /// Gets or sets the sex of the athlete in the row.
        /// </summary>
        [XmlAttribute("sx")]
        public SexType Sex
        {
            get
            {
                return this.sex;
            }

            set
            {
                if (Enum.IsDefined(typeof(SexType), value))
                {
                    this.sex = value;
                }
                else
                {
                    this.sex = SexType.NotSpecified;
                }
            }
        }

        /// <summary>
        /// Gets or sets the order of the athlete in the row.
        /// </summary>
        [XmlAttribute("order")]
        public int Order
        {
            get
            {
                return this.order;
            }

            set
            {
                this.order = value;
            }
        }

        /// <summary>
        /// Gets or sets the Team Trophy points of the athlete in the row.
        /// </summary>
        [XmlAttribute("HPts")]
        public int TeamTrophyPoints
        {
            get
            {
                return this.teamTrophyPoints;
            }

            set
            {
                this.teamTrophyPoints = value;
            }
        }

        /// <summary>
        /// Gets or sets the running order of the athlete in the row.
        /// </summary>
        [XmlAttribute("timePosition")]
        public int RunningOrder
        {
            get
            {
                return this.runningOrder;
            }

            set
            {
                this.runningOrder = value;
            }
        }

        /// <summary>
        /// Gets or sets the is personal best flag of the athlete in the row.
        /// </summary>
        [XmlAttribute("PB")]
        public bool IsPersonalBest
        {
            get
            {
                return this.personalBest;
            }

            set
            {
                this.personalBest = value;
            }
        }

        /// <summary>
        /// Gets or sets the is year best flag of the athlete in the row.
        /// </summary>
        [XmlAttribute("YB")]
        public bool IsYearBest
        {
            get
            {
                return this.yearBest;
            }

            set
            {
                this.yearBest = value;
            }
        }
    }
}
