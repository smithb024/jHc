namespace jHCVMUI.ViewModels.ViewModels.Types.Athletes
{
    using System.Collections.ObjectModel;
    using CommonHandicapLib.Types;

    public class RawResults : AthleteSeasonBase
    {
        private RaceTimeType raceTime;
        private int order;
        private string raceNumber;

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
            get { return raceTime; }
            set
            {
                raceTime = value;
                RaisePropertyChangedEvent("RaceTime");
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
            get { return order; }
            set
            {
                order = value;
                RaisePropertyChangedEvent("Order");
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
            get { return raceNumber; }
            set
            {
                raceNumber = value;
                RaisePropertyChangedEvent("RaceNumber");
            }
        }
    }
}