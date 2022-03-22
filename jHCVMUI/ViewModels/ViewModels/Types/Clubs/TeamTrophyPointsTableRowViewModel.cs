namespace jHCVMUI.ViewModels.ViewModels.Types.Clubs
{
    using System.Linq;
    using System.Collections.Generic;
    using jHCVMUI.ViewModels.ViewModels;

    public class TeamTrophyPointsTableRowViewModel : ViewModelBase
    {
        /// <summary>
        /// The number of results which
        /// </summary>
        private const int NumberOfResultsWhichCount = 3;

        /// <summary>
        /// Indicates whether the row has been expanded.
        /// </summary>
        private bool expandedData;

        /// <summary>
        /// The total number of points scored by the team.
        /// </summary>
        private int totalPoints;

        /// <summary>
        /// Initialises a new instance of the <see cref="TeamTrophyPointsTableRowViewModel"/> class.
        /// </summary>
        /// <param name="name">team name</param>
        /// <param name="points">total team points.</param>
        public TeamTrophyPointsTableRowViewModel(
            string name,
            int points)
        {
            this.expandedData = false;
            this.ClubName = name;
            this.Points = new List<TeamTrophyPointsTypeViewModel>();
        }

        /// <summary>
        /// Gets the club name
        /// </summary>
        public string ClubName { get; }

        /// <summary>
        /// Gets the total points awarded to the club.
        /// </summary>
        public int TotalPoints
        {
            get
            {
                return this.totalPoints;
            }

            private set
            {
                if (this.totalPoints == value)
                {
                    return;
                }

                this.totalPoints = value;
                this.RaisePropertyChangedEvent(nameof(this.TotalPoints));
            }
        }

        /// <summary>
        /// Gets the number of points which count towards the <see cref="TotalPoints"/>.
        /// </summary>
        public int PointsCounter => 
            Points.Count > NumberOfResultsWhichCount
            ?  NumberOfResultsWhichCount
            : Points.Count;

        /// <summary>
        /// Gets the collection of Team Trophy points
        /// </summary>
        public List<TeamTrophyPointsTypeViewModel> Points { get; private set; }

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
                RaisePropertyChangedEvent(nameof(ExpandedData));
            }
        }

        /// <summary>
        /// Add a new set of points to the data. Recalculate the totals.
        /// </summary>
        /// <param name="newPoints">new points set</param>
        public void AddPoints(TeamTrophyPointsTypeViewModel newPoints)
        {
            if (this.Points == null)
            {
                this.Points = new List<TeamTrophyPointsTypeViewModel>();
            }

            this.Points.Add(newPoints);
            int totalPoints = 0;

            // Calculate the total points. 
            // Not all points scored by the team count. The top (NumberOfResultsWhichCount) count.
            if (Points.Count > NumberOfResultsWhichCount)
            {
                List<TeamTrophyPointsTypeViewModel> workingList =
                    new List<TeamTrophyPointsTypeViewModel>(
                        this.Points);

                workingList =
                    new List<TeamTrophyPointsTypeViewModel>(
                    workingList.OrderByDescending(
                        order => order.Score));

                for (int index = 0; index < NumberOfResultsWhichCount; ++index)
                {
                    totalPoints += workingList[index].Score;
                }
            }
            else
            {
                foreach (TeamTrophyPointsTypeViewModel point in this.Points)
                {
                    totalPoints += point.Score;
                }
            }

            this.TotalPoints = totalPoints;
        }
    }
}