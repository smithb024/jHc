namespace jHCVMUI.ViewModels.ViewModels.Types.Athletes
{
    using CommonHandicapLib.Types;

    /// <summary>
    /// Class describing a single result for the GUI.
    /// </summary>
    public class ResultsTableRowViewModel : AthleteBase
    {
        /// <summary>
        /// Athlete's club
        /// </summary>
        private string club;

        /// <summary>
        /// The handicap the athlete ran with.
        /// </summary>
        private string handicap;

        /// <summary>
        /// Athlete's notes detailing first time/position data.
        /// </summary>
        private string extraInfo;

        /// <summary>
        /// Athlete's notes.
        /// </summary>
        private string notes;

        /// <summary>
        /// Indicates whether this is the first time for this athlete.
        /// </summary>
        private bool firstTimer;

        /// <summary>
        /// Speed order, indicates the position by speed.
        /// </summary>
        private int runningOrder;

        /// <summary>
        /// Position in the event including handicap time.
        /// </summary>
        private int position;

        /// <summary>
        /// Indicates whether a personal best has been achieved.
        /// </summary>
        private string pb;

        /// <summary>
        /// Total number of points scored.
        /// </summary>
        private int points;

        /// <summary>
        /// Total number of points scored for finishing.
        /// </summary>
        private int finishingPoints;

        /// <summary>
        /// Total number of points scored for finishing in a placing position.
        /// </summary>
        private int positionPoints;

        /// <summary>
        /// Total number of points scored for achiving a season best time.
        /// </summary>
        private int bestPoints;

        /// <summary>
        /// Total number of points scored in the harmony competition.
        /// </summary>
        private string harmonyPoints;

        /// <summary>
        /// Athlete's race number.
        /// </summary>
        private string raceNumber;

        /// <summary>
        /// Total time including the handicap.
        /// </summary>
        private string totalTime;

        /// <summary>
        /// Total time spent running.
        /// </summary>
        private string runningTime;

        /// <summary>
        /// Athlete's sex.
        /// </summary>
        private string sex;

        /// <summary>
        /// Indicates whether a season best has been achieved.
        /// </summary>
        private string sb = string.Empty;

        /// <summary>
        /// Indicates whether the rows show a full set of information or a truncated set of information.
        /// </summary>
        private bool expandedData;

        /// <summary>
        /// Initialises a new instance of the <see cref="ResultsTableRowViewModel"/> class.
        /// </summary>
        /// <param name="key">athete key</param>
        /// <param name="position">athlete position--</param>
        /// <param name="name">athlete name</param>
        /// <param name="club">athlete club</param>
        /// <param name="handicap">athlte handicap</param>
        /// <param name="extraInfo">athlete extra information</param>
        /// <param name="notes">athlete notes</param>
        /// <param name="firstTimer">athlete first timer</param>
        /// <param name="runningOrder">running order</param>
        /// <param name="pb">personal best</param>
        /// <param name="points">total points</param>
        /// <param name="finishingPoints">finishing points</param>
        /// <param name="positionPoints">position points</param>
        /// <param name="bestPoints">best points</param>
        /// <param name="harmonyPoints">harmony competition points</param>
        /// <param name="raceNumber">race number</param>
        /// <param name="totalTime">total time</param>
        /// <param name="runningTime">running time</param>
        /// <param name="sex">athlete sex</param>
        /// <param name="sb">season best</param>
        public ResultsTableRowViewModel(
          int key,
          int position,
          string name,
          string club,
          string handicap,
          string extraInfo,
          string notes,
          bool firstTimer,
          int runningOrder,
          bool pb,
          int points,
          int finishingPoints,
          int positionPoints,
          int bestPoints,
          int harmonyPoints,
          string raceNumber,
          string totalTime,
          string runningTime,
          string sex,
          bool sb)
          : base(key, name)
        {
            this.club = club;
            this.handicap = handicap;
            this.extraInfo = extraInfo;
            this.notes = notes;
            this.firstTimer = firstTimer;
            this.runningOrder = runningOrder;
            this.points = points;
            this.finishingPoints = finishingPoints;
            this.position = position;
            this.positionPoints = positionPoints;
            this.bestPoints = bestPoints;
            this.harmonyPoints = harmonyPoints > 0 ? $"{harmonyPoints}" : string.Empty;
            this.pb = pb ? "Y" : string.Empty;
            this.raceNumber = raceNumber;
            this.totalTime = totalTime;
            this.runningTime = runningTime;
            this.sex = sex;
            this.sb = sb ? "Y" : string.Empty;

            this.expandedData = false;
        }

        /// <summary>
        /// Gets or sets the athlete's club.
        /// </summary>
        public string Club
        {
            get
            {
                return this.club;
            }

            set
            {
                this.club = value;
                RaisePropertyChangedEvent("Club");
            }
        }

        /// <summary>
        /// Gets or sets the handicap the athlete ran with.
        /// </summary>
        public string Handicap
        {
            get
            {
                return this.handicap;
            }

            set
            {
                this.handicap = value;
                RaisePropertyChangedEvent("Handicap");
            }
        }

        /// <summary>
        /// Gets or sets the extra information.
        /// </summary>
        public string ExtraInfo
        {
            get
            {
                return this.extraInfo;
            }

            set
            {
                this.extraInfo = value;
                RaisePropertyChangedEvent("ExtraInfo");
            }
        }

        /// <summary>
        /// Gets or sets the athlete's notes.
        /// </summary>
        public string Notes
        {
            get
            {
                return this.notes;
            }

            set
            {
                this.notes = value;
                RaisePropertyChangedEvent("Notes");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this is the athlete's first time.
        /// </summary>
        public bool FirstTimer
        {
            get
            {
                return this.firstTimer;
            }

            set
            {
                this.firstTimer = value;
                RaisePropertyChangedEvent("FirstTimer");
            }
        }

        /// <summary>
        /// Gets or sets the finishing position calculated against speed.
        /// </summary>
        public int RunningOrder
        {
            get
            {
                return this.runningOrder;
            }

            set
            {
                this.runningOrder = value;
                RaisePropertyChangedEvent("RunningOrder");
            }
        }

        /// <summary>
        /// Gets or sets a string which indicates whether a personal best was achieved.
        /// </summary>
        public string PersonalBest
        {
            get
            {
                return this.pb;
            }

            set
            {
                this.pb = value;
                RaisePropertyChangedEvent("PersonalBest");
            }
        }

        /// <summary>
        /// Gets or sets the total points scored in this event.
        /// </summary>
        public int TotalPoints
        {
            get
            {
                return this.points;
            }

            set
            {
                this.points = value;
                RaisePropertyChangedEvent("TotalPoints");
            }
        }

        /// <summary>
        /// Gets or sets the total points scored for finishing.
        /// </summary>
        public int FinishingPoints
        {
            get
            {
                return this.finishingPoints;
            }

            set
            {
                this.finishingPoints = value;
                RaisePropertyChangedEvent("FinishingPoints");
            }
        }

        /// <summary>
        /// Gets or sets the athlete position.
        /// </summary>
        public int Position
        {
            get
            {
                return this.position;
            }

            set
            {
                this.position = value;
                RaisePropertyChangedEvent(nameof(this.Position));
            }
        }

        /// <summary>
        /// Gets or sets the points scored for finshing in placing position.
        /// </summary>
        public int PositionPoints
        {
            get
            {
                return this.positionPoints;
            }

            set
            {
                this.positionPoints = value;
                RaisePropertyChangedEvent("PositionPoints");
            }
        }

        /// <summary>
        /// Gets or sets the points scored for achiving a new season best time,
        /// </summary>
        public int BestPoints
        {
            get
            {
                return this.bestPoints;
            }

            set
            {
                this.bestPoints = value;
                RaisePropertyChangedEvent("BestPoints");
            }
        }

        /// <summary>
        /// Gets the points scored in the harmony competition.
        /// </summary>
        public string HarmonyPoints => this.harmonyPoints;

        /// <summary>
        /// Gets or sets the atlete's race number.
        /// </summary>
        public string RaceNumber
        {
            get
            {
                return this.raceNumber;
            }

            set
            {
                this.raceNumber = value;
                RaisePropertyChangedEvent("RaceNumber");
            }
        }

        /// <summary>
        /// Gets or sets the total time.
        /// </summary>
        public string TotalTime
        {
            get
            {
                return this.totalTime;
            }

            set
            {
                this.totalTime = value;
                RaisePropertyChangedEvent("TotalTime");
            }
        }

        /// <summary>
        /// Gets or sets the time spent running.
        /// </summary>
        public string RunningTime
        {
            get
            {
                return this.runningTime;
            }

            set
            {
                this.runningTime = value;
                RaisePropertyChangedEvent("RunningTime");
            }
        }

        /// <summary>
        /// Gets or sets the athlete's sex.
        /// </summary>
        public string Sex
        {
            get
            {
                return this.sex;
            }

            set
            {
                this.sex = value;
                RaisePropertyChangedEvent("Sex");
            }
        }

        /// <summary>
        /// Gets or sets a value which indicates whether a new season best time has been achived.
        /// </summary>
        public string SeasonBest
        {
            get
            {
                return this.sb;
            }

            set
            {
                this.sb = value;
                RaisePropertyChangedEvent("SeasonBest");
            }
        }

        /// <summary>
        /// Gets and sets the expanded data flag.
        /// </summary>
        public bool ExpandedData
        {
            get
            {
                return this.expandedData;
            }

            set
            {
                this.expandedData = value;
                RaisePropertyChangedEvent("ExpandedData");
            }
        }

        /// <summary>
        /// Record the success of the result.
        /// </summary>
        public ResultsState SuccessState
        {
            get
            {
                if (PersonalBest == "Y")
                {
                    return ResultsState.NewPB;
                }
                else if (SeasonBest == "Y")
                {
                    return ResultsState.NewSB;
                }
                else if (FirstTimer)
                {
                    return ResultsState.FirstTimer;
                }

                return ResultsState.DefaultState;
            }
        }
    }
}