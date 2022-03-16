namespace jHCVMUI.ViewModels.ViewModels.Types.Clubs
{
    using System.Collections.ObjectModel;
    using jHCVMUI.ViewModels.ViewModels.Types;

    /// <summary>
    /// View model which describes a single row on the Mob Trophy Points table on the Data Pane
    /// </summary>
    public class MobTrophyPointsTableRowViewModel : ViewModelBase
    {
        /// <summary>
        /// Name of the club.
        /// </summary>
        private string name;

        /// <summary>
        /// Collection of all points awarded to the club.
        /// </summary>
        private ObservableCollection<PointsType> points;

        /// <summary>
        /// Total points awarded to the club.
        /// </summary>
        private int totalPoints;

        /// <summary>
        /// All finishing points awarded to the club.
        /// </summary>
        private string finishingPoints;

        /// <summary>
        /// All position points awarded to the club.
        /// </summary>
        private string positionPoints;

        /// <summary>
        /// All season best points awarded to the club
        /// </summary>
        private string sbPoints;

        /// <summary>
        /// Indicates whether the row has been expanded.
        /// </summary>
        private bool expandedData;

        /// <summary>
        /// Initialises a new instance of the <see cref="MobTrophyPointsTableRowViewModel"/> class.
        /// </summary>
        /// <param name="name"></param>
        public MobTrophyPointsTableRowViewModel(
            string name,
            int totalPoints,
            string finishingPoints,
            string positionPoints,
            string sbPoints)
        {
            this.name = name;
            this.points = new ObservableCollection<PointsType>();
            this.totalPoints = totalPoints;
            this.finishingPoints = finishingPoints;
            this.positionPoints = positionPoints;
            this.sbPoints = sbPoints;
            this.expandedData = false;
        }

        /// <summary>
        /// Gets or sets the club name
        /// </summary>
        public string ClubName
        {
            get
            {
                return name;
            }
            private set
            {
                name = value;
                RaisePropertyChangedEvent("ClubName");
            }
        }

        /// <summary>
        /// Gets or sets the total points awarded to the club.
        /// </summary>
        public int TotalPoints
        {
            get
            {
                return totalPoints;
            }
            private set
            {
                totalPoints = value;
                RaisePropertyChangedEvent("TotalPoints");
            }
        }

        /// <summary>
        /// Gets or sets the total finishing points awarded to the club.
        /// </summary>
        public string FinishingPoints
        {
            get
            {
                return finishingPoints;
            }
            private set
            {
                finishingPoints = value;
                RaisePropertyChangedEvent("FinishingPoints");
            }
        }

        /// <summary>
        /// Gets or sets the total position points awarded to the club.
        /// </summary>
        public string PositionPoints
        {
            get
            {
                return positionPoints;
            }
            private set
            {
                positionPoints = value;
                RaisePropertyChangedEvent("PositionPoints");
            }
        }

        /// <summary>
        /// Gets or sets the total season best points awarded to the club.
        /// </summary>
        public string SBPoints
        {
            get
            {
                return sbPoints;
            }
            private set
            {
                sbPoints = value;
                RaisePropertyChangedEvent("SBPoints");
            }
        }

        /// <summary>
        /// Gets or sets the club name
        /// </summary>
        public ObservableCollection<PointsType> MobTrophyPoints
        {
            get
            {
                return this.points;
            }
            private set
            {
                this.points = value;
                this.RaisePropertyChangedEvent(nameof(this.MobTrophyPoints));
            }
        }

        /// <summary>
        /// Gets and sets the expanded data flag.
        /// </summary>
        public bool ExpandedData
        {
            get { return expandedData; }
            set
            {
                expandedData = value;
                RaisePropertyChangedEvent("ExpandedData");
            }
        }

        /// <summary>
        /// Add new points to the view model
        /// </summary>
        /// <param name="newPoints">points to add</param>
        public void AddPoints(PointsType newPoints)
        {
            MobTrophyPoints.Add(newPoints);
        }
    }
}