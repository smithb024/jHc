namespace jHCVMUI.ViewModels.ViewModels.Types.Athletes
{
    using System.Collections.ObjectModel;
    using CommonHandicapLib.Types;

    /// <summary>
    /// The raw results for a single athlete in a single event.
    /// </summary>
    public class RawResults : AthleteSeasonBase
    {
        /// <summary>
        /// The race time.
        /// </summary>
        private RaceTimeType raceTime;

        /// <summary>
        /// An order key, used if two athletes have the same time.
        /// </summary>
        private int order;

        /// <summary>
        /// The number of the athlete, used during the event.
        /// </summary>
        private string raceNumber;

        /// <summary>
        /// Initialises a new instance of the <see cref="RawResults"/> class.
        /// </summary>
        /// <param name="key">The athlete key</param>
        /// <param name="name">The athlete name</param>
        /// <param name="runningNumbers">The number associated with the athlete</param>
        public RawResults(
          int key,
          string name,
          ObservableCollection<string> runningNumbers)
          : base(key, name, runningNumbers)
        {
            this.raceTime = null;
            this.order = 0;
            this.raceNumber = string.Empty;
        }

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <summary>
        /// Gets and sets the race time
        /// </summary>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        public RaceTimeType RaceTime
        {
            get => this.raceTime;
            set
            {
                this.raceTime = value;
                this.RaisePropertyChangedEvent(nameof(this.RaceTime));
            }
        }

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <summary>
        /// Gets and sets the order.
        /// </summary>
        /// <remarks>
        /// The order is used to differentiate between different athletes with the same race time.
        /// </remarks>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        public int Order
        {
            get => this.order; 
            set
            {
                this.order = value;
                this.RaisePropertyChangedEvent(nameof(this.Order));
            }
        }

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <summary>
        /// Gets and sets the race number.
        /// </summary>
        /// <remarks>
        /// This is the number used by the athlete in the race.
        /// </remarks>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        public string RaceNumber
        {
            get => raceNumber;
            set
            {
                this.raceNumber = value;
                this.RaisePropertyChangedEvent(nameof(this.RaceNumber));
            }
        }
    }
}