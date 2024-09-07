namespace jHCVMUI.ViewModels.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using CommonHandicapLib.Messages;
    using CommonHandicapLib.Types;
    using HandicapModel.Interfaces;
    using jHCVMUI.ViewModels.Commands.Configuration;
    using jHCVMUI.ViewModels.ViewModels.Types;
    using CommonMessenger = NynaeveLib.Messenger.Messenger;

    /// <summary>
    /// View model which supports club configuration.
    /// </summary>
    public class ClubConfigurationViewModel : ViewModelBase
    {
        /// <summary>
        /// Junior handicap model.
        /// </summary>
        private IModel model;

        private int selectedIndex = -1;
        private string newClub = string.Empty;
        private bool saved = true;

        private ObservableCollection<ClubType> m_clubCollection = new ObservableCollection<ClubType>();

        /// <summary>
        ///   Creates a new instance of the ClubConfigurationViewModel class
        /// </summary>
        /// <param name="model">Junior handicap model</param>
        public ClubConfigurationViewModel(
            IModel model)
        {
            this.model = model;
            LoadClubInformation();

            ClubSaveCommand = new ClubConfigSaveCmd(this);
            ClubNewCommand = new ClubConfigNewCmd(this);
            ClubDeleteCommand = new ClubConfigDeleteCmd(this);
        }

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>ClubCollection</name>
        /// <date>24/02/15</date>
        /// <summary>
        /// Gets and sets the club collection
        /// </summary>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        public ObservableCollection<ClubType> ClubCollection
        {
            get { return m_clubCollection; }
            set
            {
                m_clubCollection = value;
                RaisePropertyChangedEvent("ClubCollection");
            }
        }

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>NewClub</name>
        /// <date>24/02/15</date>
        /// <summary>
        /// Gets and sets the new club
        /// </summary>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        public string NewClub
        {
            get { return newClub; }
            set
            {
                newClub = value;
                RaisePropertyChangedEvent("NewClub");
            }
        }

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>SelectedClubIndex</name>
        /// <date>24/02/15</date>
        /// <summary>
        /// Gets and sets the index of the club list
        /// </summary>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        public int SelectedClubIndex
        {
            get { return selectedIndex; }
            set
            {
                selectedIndex = value;
            }
        }

        public ICommand ClubSaveCommand
        {
            get;
            private set;
        }

        public ICommand ClubNewCommand
        {
            get;
            private set;
        }

        public ICommand ClubDeleteCommand
        {
            get;
            private set;
        }

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>SaveClubs</name>
        /// <date>01/03/15</date>
        /// <summary>
        /// Saves the current list to a file.
        /// </summary>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        public void SaveClubs()
        {
            foreach (ClubType club in ClubCollection)
            {
                switch (club.Status)
                {
                    case StatusType.Added:
                        this.model.CreateNewClub(club.Club);
                        break;
                    case StatusType.Deleted:
                        this.model.DeleteClub(club.Club);
                        break;
                    default:
                        break;
                }
            }

            this.model.SaveClubList();
            ResetSelectedIndex();
            LoadClubInformation();

            CommonMessenger.Default.Send(
                new HandicapProgressMessage(
                    "Clubs Saved"));
            saved = true;
        }

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>CanSave</name>
        /// <date>01/03/15</date>
        /// <summary>
        /// Returns true the data is not currently saved.
        /// </summary>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        public bool CanSave()
        {
            return !saved;
        }

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>AddNewClub</name>
        /// <date>24/02/15</date>
        /// <summary>
        /// Adds a new club and resets the NewClub property.
        /// </summary>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        public void AddNewClub()
        {
            ClubCollection.Add(new ClubType() { Club = NewClub, Status = StatusType.Added });
            NewClub = string.Empty;
            saved = false;
        }

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>CanAdd</name>
        /// <date>01/03/15</date>
        /// <summary>
        /// Returns true if the new club name field is not empty.
        /// </summary>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        public bool CanAdd()
        {
            return !string.IsNullOrWhiteSpace(NewClub);
        }

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>DeleteClub</name>
        /// <date>01/03/15</date>
        /// <summary>
        /// Removes the selected club and resets the index.
        /// </summary>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        public void DeleteClub()
        {
            if (ClubCollection[SelectedClubIndex].Status == StatusType.Added)
            {
                ClubCollection.RemoveAt(SelectedClubIndex);
            }
            else
            {
                ClubCollection[SelectedClubIndex].Status = StatusType.Deleted;
            }

            ResetSelectedIndex();
            saved = false;
        }

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>CanDelete</name>
        /// <date>01/03/15</date>
        /// <summary>
        /// Returns true if the selected index is valid.
        /// </summary>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        public bool CanDelete()
        {
            if (SelectedClubIndex >= 0 && SelectedClubIndex < ClubCollection.Count)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>ResetSelectedIndex/</name>
        /// <date>31/03/15</date>
        /// <summary>
        /// Provides a method for clearing the selected index.
        /// </summary>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        private void ResetSelectedIndex()
        {
            SelectedClubIndex = -1;
        }

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>LoadClubInformation</name>
        /// <date>01/04/15</date>
        /// <summary>
        /// Loads the club information via the business library and adds it to the club collection.
        /// </summary>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        private void LoadClubInformation()
        {
            ClubCollection = new ObservableCollection<ClubType>();
            foreach (string club in this.model.GetClubList())
            {
                ClubCollection.Add(new ClubType() { Club = club });
            }
        }
    }
}