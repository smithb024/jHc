namespace jHCVMUI.ViewModels.ViewModels
{

    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using HandicapModel.Interfaces;
    using jHCVMUI.ViewModels.ViewModels.Types.Clubs;

    /// <summary>
    /// Used to present club summary data to the user.
    /// </summary>
    public class ClubSummaryViewModel : ViewModelBase
    {
        /// <summary>
        /// Junior handicap model.
        /// </summary>
        private IModel model;

        private ObservableCollection<ClubSummary> clubCollection = new ObservableCollection<ClubSummary>();

        /// <summary>
        /// Initialises a new instance of the <see cref="ClubSummaryViewModel"/> class.
        /// </summary>
        /// <param name="model">junior handicap model</param>
        public ClubSummaryViewModel(
            IModel model)
        {
            this.model = model;

            LoadClubInformation();
        }

        /// <summary>
        /// Gets and sets the collection of data to be presented to the user.
        /// </summary>
        public ObservableCollection<ClubSummary> ClubCollection
        {
            get
            {
                return clubCollection;
            }
            set
            {
                clubCollection = value;
                RaisePropertyChangedEvent("ClubCollection");
            }
        }

        /// <summary>
        /// Get all information to present.
        /// </summary>
        public void LoadClubInformation()
        {
            List<string> orderedList = this.model.Clubs.ClubDetails.OrderBy(club => club).ToList();

            ClubCollection = new ObservableCollection<ClubSummary>();
            foreach (string club in orderedList)
            {
                int numberRegistered = 
                    this.model.Athletes.GetNumberRegisteredToClub(
                        club);
                ClubSummary summary =
                    new ClubSummary(
                        club,
                        numberRegistered);

                ClubCollection.Add(summary);
            }
        }
    }
}