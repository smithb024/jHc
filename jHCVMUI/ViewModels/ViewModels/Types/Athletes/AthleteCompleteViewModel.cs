namespace jHCVMUI.ViewModels.ViewModels.Types.Athletes
{
    using System.Collections.ObjectModel;

    using Misc;

    /// <summary>
    /// View model of an athlete class, defines all the simple properties needed to show in an
    /// athlete.
    /// </summary>
    public class AthleteCompleteViewModel : AthleteSimpleViewModel
    {
        private string sb = string.Empty;
        private ObservableCollection<string> currentSeasonNumbers;
        private ObservableCollection<AppearancesViewModel> currentSeasonTimes;
        private ObservableCollection<AppearancesViewModel> allTimes;
        private int currentSeasonTotalPoints = 0;
        private int currentSeasonFinishingPoints = 0;
        private int currentSeasonPositionPoints = 0;
        private int currentSeasonBestPoints = 0;

        /// <summary>
        /// Initialises a new instance of the <see cref="AthleteCompleteViewModel"/> class.
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
        /// <param name="sb">current season best</param>
        /// <param name="currentSeasonNumbers">registed race numbers</param>
        /// <param name="currentSeasonTimes">race times for the current season</param>
        /// <param name="allTimes">all race times</param>
        /// <param name="currentSeasonTotalPoints">total points</param>
        /// <param name="currentSeasonFinishingPoints">points received for finishing</param>
        /// <param name="currentSeasonPositionPoints">
        /// points received for finishing in a placing position
        /// </param>
        /// <param name="currentSeasonBestPoints">points received for running a season best</param>
        public AthleteCompleteViewModel(
          int key,
          string name,
          string club,
          string sex,
          string roundedHandicap,
          string pb,
          string lastAppearance,
          int noRuns,
          bool signedConsent,
          bool active,
          string sb,
          ObservableCollection<string> currentSeasonNumbers,
          ObservableCollection<AppearancesViewModel> currentSeasonTimes,
          ObservableCollection<AppearancesViewModel> allTimes,
          int currentSeasonTotalPoints,
          int currentSeasonFinishingPoints,
          int currentSeasonPositionPoints,
          int currentSeasonBestPoints)
          : base(key, name, club, sex, roundedHandicap, pb, lastAppearance, noRuns, signedConsent, active)
        {
            this.SB = sb;
            this.CurrentSeasonNumbers = currentSeasonNumbers;
            this.AllTimes = allTimes;
            this.CurrentSeasonTimes = currentSeasonTimes;
            this.CurrentSeasonTotalPoints = currentSeasonTotalPoints;
            this.CurrentSeasonFinishingPoints = currentSeasonFinishingPoints;
            this.CurrentSeasonPositionPoints = currentSeasonPositionPoints;
            this.CurrentSeasonBestPoints = currentSeasonBestPoints;
        }

        /// <summary>
        /// Gets and sets the season best.
        /// </summary>
        public string SB
        {
            get { return this.sb; }
            set
            {
                this.sb = value;
                RaisePropertyChangedEvent("SB");
            }
        }

        /// <summary>
        /// Gets and sets the numbers used by the athlete in the current season.
        /// </summary>
        public ObservableCollection<string> CurrentSeasonNumbers
        {
            get { return this.currentSeasonNumbers; }
            set
            {
                this.currentSeasonNumbers = value;
                RaisePropertyChangedEvent("CurrentSeasonNumbers");
            }
        }

        /// <summary>
        /// Gets and sets the times set in the current season.
        /// </summary>
        public ObservableCollection<AppearancesViewModel> CurrentSeasonTimes
        {
            get { return this.currentSeasonTimes; }
            set
            {
                this.currentSeasonTimes = value;
                RaisePropertyChangedEvent("CurrentSeasonTimes");
            }
        }

        /// <summary>
        /// Gets and sets the all times set.
        /// </summary>
        public ObservableCollection<AppearancesViewModel> AllTimes
        {
            get { return this.allTimes; }
            set
            {
                this.allTimes = value;
                RaisePropertyChangedEvent("AllTimes");
            }
        }

        /// <summary>
        /// Gets and sets total points for the season.
        /// </summary>
        public int CurrentSeasonTotalPoints
        {
            get { return this.currentSeasonTotalPoints; }
            set
            {
                this.currentSeasonTotalPoints = value;
                RaisePropertyChangedEvent("CurrentSeasonTotalPoints");
            }
        }

        /// <summary>
        /// Gets and sets total finishing points for the season.
        /// </summary>
        public int CurrentSeasonFinishingPoints
        {
            get { return this.currentSeasonFinishingPoints; }
            set
            {
                this.currentSeasonFinishingPoints = value;
                RaisePropertyChangedEvent("CurrentSeasonFinishingPoints");
            }
        }

        /// <summary>
        /// Gets and sets total position points for the season.
        /// </summary>
        public int CurrentSeasonPositionPoints
        {
            get { return this.currentSeasonPositionPoints; }
            set
            {
                this.currentSeasonPositionPoints = value;
                RaisePropertyChangedEvent("CurrentSeasonPositionPoints");
            }
        }

        /// <summary>
        /// Gets and sets total best points for the season.
        /// </summary>
        public int CurrentSeasonBestPoints
        {
            get { return this.currentSeasonBestPoints; }
            set
            {
                this.currentSeasonBestPoints = value;
                RaisePropertyChangedEvent("CurrentSeasonBestPoints");
            }
        }
    }
}