namespace jHCVMUI.ViewModels.ViewModels.Types.Clubs
{
    using System.Collections.Generic;
    using jHCVMUI.ViewModels.ViewModels;

    public class TeamHarmonyPointsTableRowViewModel : ViewModelBase
    {
        /// <summary>
        /// Indicates whether the row has been expanded.
        /// </summary>
        private bool expandedData;

        /// <summary>
        /// Initialises a new instance of the <see cref="TeamHarmonyPointsTableRowViewModel"/> class.
        /// </summary>
        /// <param name="name">team name</param>
        /// <param name="points">total team points.</param>
        public TeamHarmonyPointsTableRowViewModel(
            string name,
            int points)
        {
            this.expandedData = false;
            this.ClubName = name;
            this.TotalPoints = points;
            this.Points = new List<HarmonyPointsTypeViewModel>();
        }

        /// <summary>
        /// Gets the club name
        /// </summary>
        public string ClubName { get; }

        /// <summary>
        /// Gets the total points awarded to the club.
        /// </summary>
        public int TotalPoints { get; }

        /// <summary>
        /// Gets the collection of harmony points
        /// </summary>
        public List<HarmonyPointsTypeViewModel> Points { get; }

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
    }
}
