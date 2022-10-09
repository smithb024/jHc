namespace CommonHandicapLib.XML.AthleteData
{
    using System.Xml.Serialization;
    using CommonLib.Enumerations;

    /// <summary>
    /// Simple row object which represents a single row in the results table XML file.
    /// </summary>
    public class Athlete
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
        private string predeclaredHandicap;

        /// <summary>
        /// The athlete's name.
        /// </summary>
        private string name;

        /// <summary>
        /// The athletes forename.
        /// </summary>
        private string forename;

        /// <summary>
        /// The athlete's family name.
        /// </summary>
        private string familyName;

        /// <summary>
        /// The athlete's sex.
        /// </summary>
        private SexType sex;

        /// <summary>
        /// Indicates whether the athlete has a signed consent form.
        /// </summary>
        private bool signedConsent;

        /// <summary>
        /// Indicates whether the athlete is currently active.
        /// </summary>
        private bool active;

        /// <summary>
        /// The year of birth for the athlete.
        /// </summary>
        private string birthYear;

        /// <summary>
        /// The month of birth for the athlete.
        /// </summary>
        private string birthMonth;

        /// <summary>
        /// The day of birth for the athlete.
        /// </summary>
        private string birthDay;

        /// <summary>
        /// All appearances for the athlete.
        /// </summary>
        private AthleteDataAppearances appearances;

        /// <summary>
        /// All the numbers associated with the athlete.
        /// </summary>
        private AthleteDataRunningNumbers runningNumbers;

        /// <summary>
        /// Gets or sets the Key of the athlete in the row.
        /// </summary>
        [XmlAttribute("Key")]
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
        [XmlAttribute("Name")]
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
        /// Gets or sets the forename of the athlete in the row.
        /// </summary>
        [XmlAttribute("Forename")]
        public string Forename
        {
            get
            {
                return this.forename;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    this.forename = string.Empty;
                }
                else
                {
                    this.forename = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the family name of the athlete in the row.
        /// </summary>
        [XmlAttribute("FamilyName")]
        public string FamilyName
        {
            get
            {
                return this.familyName;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    this.familyName = string.Empty;
                }
                else
                {
                    this.familyName = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the club of the athlete in the row.
        /// </summary>
        [XmlAttribute("Club")]
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
        public string PredeclaredHandicap
        {
            get
            {
                return this.predeclaredHandicap;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    this.predeclaredHandicap = string.Empty;
                }
                else
                {
                    this.predeclaredHandicap = value;
                }
            }
        }


        /// <summary>
        /// Gets the athlete's sex.
        /// </summary>
        [XmlAttribute("Sex")]
        public SexType Sex
        {
            get
            {
                return this.sex;
            }

            set
            {
                this.sex = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the athlete has a signed consent form.
        /// </summary>
        [XmlAttribute("SC")]
        public bool SignedConsent
        {
            get
            {
                return this.signedConsent;
            }

            set
            {
                this.signedConsent = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the athlete is currently active.
        /// </summary>
        [XmlAttribute("Atv")]
        public bool Active
        {
            get
            {
                return this.active;
            }

            set
            {
                this.active = value;
            }
        }

        /// <summary>
        /// Gets the year of birth for the athlete.
        /// </summary>
        [XmlAttribute("bY")]
        public string BirthYear
        {
            get
            {
                return this.birthYear;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    this.birthYear = string.Empty;
                }
                else
                {
                    this.birthYear = value;
                }
            }
        }

        /// <summary>
        /// Gets the month of birth for the athlete.
        /// </summary>
        [XmlAttribute("bM")]
        public string BirthMonth
        {
            get
            {
                return this.birthMonth;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    this.birthMonth = string.Empty;
                }
                else
                {
                    this.birthMonth = value;
                }
            }
        }

        /// <summary>
        /// Gets the day of birth for the athlete.
        /// </summary>
        [XmlAttribute("bD")]
        public string BirthDay
        {
            get
            {
                return this.birthDay;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    this.birthDay = string.Empty;
                }
                else
                {
                    this.birthDay = value;
                }
            }
        }

        /// <summary>
        /// Gets or setss all appearances.
        /// </summary>
        [XmlElement("apn")]
        public AthleteDataAppearances Appearances
        {
            get
            {
                return this.appearances;
            }
            set
            {
                this.appearances = value;
            }
        }

        /// <summary>
        /// Gets or sets all the running numbers associated with an athlete.
        /// </summary>
        [XmlElement("runningNumbers")]
        public AthleteDataRunningNumbers RunningNumbers
        {
            get
            {
                return this.runningNumbers;
            }
            set
            {
                this.runningNumbers = value;
            }
        }
    }
}