namespace jHCVMUI.ViewModels.ViewModels.Types.Athletes
{
    using System.Collections.ObjectModel;

    using Misc;

    /// <summary>
    /// View model of an athlete class, defines all the simple properties needed to show in an
    /// athlete.
    /// </summary>
    public class AthleteSimpleViewModel : AthleteBase
    {
        private string m_club = string.Empty;
        private string m_sex = string.Empty;
        private string m_handicap = string.Empty;
        private string trueHandicap = string.Empty;
        private string m_pb = string.Empty;
        private string m_lastAppearance = string.Empty;

        /// <summary>
        /// Indicates whether a signed consent form has been recieved.
        /// </summary>
        private bool signedConsent;
        private bool active = false;
        private int m_numberOfRuns = 0;

        /// <summary>
        /// Initialises a new instance of the <see cref="AthleteSimpleViewModel"/> class.
        /// </summary>
        /// <param name="key">athlete unique identifier</param>
        /// <param name="name">athlete name</param>
        /// <param name="club">current club</param>
        /// <param name="sex">sex of the athlete</param>
        /// <param name="roundedHandicap">current handicap (rounded to nearest half minute)</param>
        /// <param name="pb">current personal best</param>
        /// <param name="lastAppearance">date of the last appearance</param>
        /// <param name="noRuns">number of events entered</param>
        /// <param name="signedConsent">
        /// Indicates whether a signed consent form has been received.
        /// </param>
        /// <param name="active">
        /// Indicates whether the athlete is currently active.
        /// </param>
        public AthleteSimpleViewModel(
          int key,
          string name,
          string club,
          string sex,
          string roundedHandicap,
          string pb,
          string lastAppearance,
          int noRuns,
          bool signedConsent,
          bool active)
          : base(key, name)
        {
            this.Club = club;
            this.Sex = sex;
            this.RoundedHandicap = roundedHandicap;
            this.PB = pb;
            this.LastAppearance = lastAppearance;
            this.NumberOfRuns = noRuns;
            this.signedConsent = signedConsent;
            this.Active = active;
        }

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>Club</name>
        /// <date>03/05/15</date>
        /// <summary>
        /// Gets and sets the club.
        /// </summary>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        public string Club
        {
            get => this.m_club; 
            set
            {
                this.m_club = value;
                this.RaisePropertyChangedEvent(nameof(this.Club));
            }
        }

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>Sex</name>
        /// <date>03/05/15</date>
        /// <summary>
        /// Gets and sets the sex.
        /// </summary>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        public string Sex
        {
            get => this.m_sex;
            set
            {
                this.m_sex = value;
                this.RaisePropertyChangedEvent(nameof(this.Sex));
            }
        }

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>Handicap</name>
        /// <date>03/05/15</date>
        /// <summary>
        /// Gets and sets rounded handicap.
        /// </summary>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        public string RoundedHandicap
        {
            get => this.m_handicap;
            set
            {
                this.m_handicap = value;
                this.RaisePropertyChangedEvent(nameof(this.RoundedHandicap));
            }
        }

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>PB</name>
        /// <date>03/05/15</date>
        /// <summary>
        /// Gets and sets the pb.
        /// </summary>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        public string PB
        {
            get => this.m_pb;
            set
            {
                this.m_pb = value;
                this.RaisePropertyChangedEvent(nameof(this.PB));
            }
        }

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>LastAppearance</name>
        /// <date>03/05/15</date>
        /// <summary>
        /// Gets and sets last appearance.
        /// </summary>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        public string LastAppearance
        {
            get => this.m_lastAppearance; 
            set
            {
                this.m_lastAppearance = value;
                this.RaisePropertyChangedEvent(nameof(this.LastAppearance));
            }
        }

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>NumberOfRuns</name>
        /// <date>03/05/15</date>
        /// <summary>
        /// Gets and sets number of runs.
        /// </summary>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        public int NumberOfRuns
        {
            get => this.m_numberOfRuns; 
            set
            {
                this.m_numberOfRuns = value;
                this.RaisePropertyChangedEvent(nameof(this.NumberOfRuns));
            }
        }

        /// <summary>
        /// Gets and sets a value indicating if the athlete is active or not.
        /// </summary>
        public bool SignedConsent
        {
            get => this.signedConsent;
            set
            {
                this.signedConsent = value;
                this.RaisePropertyChangedEvent(nameof(this.signedConsent));
            }
        }

        /// <summary>
        /// Gets and sets a value indicating if the athlete is active or not.
        /// </summary>
        public bool Active
        {
            get => this.active;
            set
            {
                this.active = value;
                this.RaisePropertyChangedEvent(nameof(this.Active));
            }
        }
    }
}