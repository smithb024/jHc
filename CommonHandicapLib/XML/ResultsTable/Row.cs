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
        private int key;
        private string club;
        private string handicap;
        private string name;
        private string notes;
        private string extraInformation;
        private int order;
        private bool personalBest;
        private string points;
        private int harmonyPoints;
        private string number;
        private int runningOrder;
        private string time;
        private SexType sex;
        private bool yearBest;


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
            this.points = string.Empty;
            this.harmonyPoints = 0;
            this.number = string.Empty;
            this.runningOrder = 0;
            this.time = string.Empty;
            this.sex = SexType.Default;
            this.yearBest = false;
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
        public string Points
        {
            get
            {
                return this.points;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    this.points = string.Empty;
                }
                else
                {
                    this.points = value;
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
                    this.sex = SexType.Default;
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
        /// Gets or sets the harmony points of the athlete in the row.
        /// </summary>
        [XmlAttribute("HPts")]
        public int HarmonyPoints
        {
            get
            {
                return this.harmonyPoints;
            }

            set
            {
                this.harmonyPoints = value;
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
