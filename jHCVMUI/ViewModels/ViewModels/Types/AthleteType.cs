namespace jHCVMUI.ViewModels.ViewModels.Types
{
    using System;
    using System.Collections.ObjectModel;

    using CommonLib.Enumerations;
    using CommonHandicapLib.Types;

    /// <summary>
    /// This is the view model for a athlete.
    /// </summary>
    public class AthleteType : ViewModelBase
    {
        private int m_key = -1;

        /// <summary>
        /// The athlete's forename.
        /// </summary>
        private string forename;

        /// <summary>
        /// The athlete's family name.
        /// </summary>
        private string familyName;

        private string m_club = string.Empty;
        private SexType m_sex = SexType.NotSpecified;
        private string predeclaredHandicap = string.Empty;
        private StatusType m_status = StatusType.Ok;
        private string birthYear = string.Empty;
        private string birthMonth = string.Empty;
        private string birthDay = string.Empty;
        private bool active = true;

        /// <summary>
        /// Indicates whether the parental consent form has been signed.
        /// </summary>
        private bool signedConsent;

        private ObservableCollection<string> runningNumbers = new ObservableCollection<string>();

        /// Initialises a new instance of the <see cref="AthleteType"/> class.
        /// </summary>
        /// <param name="key">athlete key</param>
        /// <param name="name">athlete forename</param>
        /// <param name="familyName">athlete family name</param>
        /// <param name="club">athlete club</param>
        /// <param name="sex">athlete sex</param>
        /// <param name="runningNumbers">numbers the athlete is registered to run under</param>
        /// <param name="birthYear">athlete birth year</param>
        /// <param name="birthMonth">athlete birth month</param>
        /// <param name="birthDay">athlete birth dat</param>
        /// <param name="signedConsent">
        /// Indicates whether the parental consent form has been signed.
        /// </param>
        /// <param name="active">indicates whether the athlete is currently active</param>
        /// <param name="predeclaredHandicap">user declared handicap</param>
        public AthleteType(
          int key,
          string forename,
          string familyName,
          string club,
          SexType sex,
          ObservableCollection<string> runningNumbers,
          string birthYear,
          string birthMonth,
          string birthDay,
          bool signedConsent,
          bool active,
          string predeclaredHandicap)
        {
            this.m_key = key;
            this.forename = forename;
            this.familyName = familyName;
            this.m_club = club;
            this.m_sex = sex;
            this.runningNumbers = runningNumbers;
            this.birthYear = birthYear;
            this.birthMonth = birthMonth;
            this.birthDay = birthDay;
            this.signedConsent = signedConsent;
            this.active = active;
            this.predeclaredHandicap = predeclaredHandicap;
        }

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>Key</name>
        /// <date>01/03/15</date>
        /// <summary>
        /// Gets and sets the key.
        /// </summary>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        public int Key
        {
            get { return m_key; }
            set { m_key = value; }
        }

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>Name</name>
        /// <date>01/03/15</date>
        /// <summary>
        /// Gets and sets the name.
        /// </summary>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        public string Forename
        {
            get => this.forename;
            set
            {
                this.forename = value;
                this.RaisePropertyChangedEvent(nameof(this.Forename));
            }
        }

        /// <summary>
        /// Gets the surname.
        /// </summary>
        public string FamilyName
        {
            get => this.familyName;
            set
            {
                this.familyName = value;
                this.RaisePropertyChangedEvent(nameof(this.FamilyName));
            }
        }

        /// <summary>
        /// Gets the surname.
        /// </summary>
        public string Name
        {
            get => $"{this.forename} {this.familyName}";
        }

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>Club</name>
        /// <date>01/03/15</date>
        /// <summary>
        /// Gets and sets the club.
        /// </summary>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        public string Club
        {
            get { return m_club; }
            set
            {
                m_club = value;
                RaisePropertyChangedEvent("Club");
            }
        }

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>Sex</name>
        /// <date>01/03/15</date>
        /// <summary>
        /// Gets and sets the sex.
        /// </summary>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        public SexType Sex
        {
            get { return m_sex; }
            set
            {
                m_sex = value;
                RaisePropertyChangedEvent("Sex");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating if the athlete account is active or not.
        /// </summary>
        public bool SignedConsent
        {
            get
            {
                return this.signedConsent;
            }

            set
            {
                this.signedConsent = value;
                RaisePropertyChangedEvent(nameof(this.SignedConsent));
            }
        }

        /// <summary>
        /// Gets or sets a value indicating if the athlete account is active or not.
        /// </summary>
        public bool Active
        {
            get { return active; }
            set
            {
                active = value;
                RaisePropertyChangedEvent("Active");
            }
        }

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>InitialHandicap</name>
        /// <date>09/03/15</date>
        /// <summary>
        /// Gets and sets initial handicap.
        /// </summary>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        public string PredeclaredHandicap
        {
            get { return predeclaredHandicap; }
            set
            {
                predeclaredHandicap = value;
                RaisePropertyChangedEvent(nameof(this.PredeclaredHandicap));
            }
        }

        /// <summary>
        /// Gets and sets the runnig numbers.
        /// </summary>
        public ObservableCollection<string> RunningNumbers
        {
            get { return this.runningNumbers; }
            private set
            {
                this.runningNumbers = value;
                RaisePropertyChangedEvent("RunningNumbers");
            }
        }

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>Status</name>
        /// <date>10/03/15</date>
        /// <summary>
        /// Gets and sets the status.
        /// </summary>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        public StatusType Status
        {
            get { return m_status; }
            set
            {
                m_status = value;
                RaisePropertyChangedEvent("Status");
            }
        }

        public string BirthYear
        {
            get { return this.birthYear; }
            set
            {
                this.birthYear = value;
                RaisePropertyChangedEvent(nameof(this.BirthYear));
            }
        }

        public string BirthMonth
        {
            get { return this.birthMonth; }
            set
            {
                this.birthMonth = value;
                RaisePropertyChangedEvent(nameof(this.BirthMonth));
            }
        }

        public string BirthDay
        {
            get { return this.birthDay; }
            set
            {
                this.birthDay = value;
                RaisePropertyChangedEvent(nameof(this.BirthDay));
            }
        }

        /// <summary>
        /// Add a new running number to the collection.
        /// </summary>
        /// <param name="newNumber">number to add</param>
        public void AddNewRunningNumber(string newNumber)
        {
            this.RunningNumbers.Add(newNumber);
            this.RaisePropertyChangedEvent("RunningNumber");
        }
    }
}