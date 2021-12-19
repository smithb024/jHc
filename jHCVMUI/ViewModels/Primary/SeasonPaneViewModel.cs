namespace jHCVMUI.ViewModels.Primary
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;

    using CommonHandicapLib.Interfaces;
    using HandicapModel;
    using HandicapModel.Admin.IO;
    using HandicapModel.Admin.Manage;
    using HandicapModel.Interfaces.Admin.IO;
    using jHCVMUI.ViewModels.ViewModels;
    using jHCVMUI.ViewModels.Commands.Main;
    using jHCVMUI.Views.Configuration;
    using jHCVMUI.Views.Windows;

    using NynaeveLib.Commands;
    using HandicapModel.Interfaces;

    public delegate void CallDelegate();

    /// <summary>
    /// View model which supports the season view.
    /// </summary>
    public class SeasonPaneViewModel : ViewModelBase
    {
        /// <summary>
        /// Junior handicap model.
        /// </summary>
        private readonly IModel model;

        /// <summary>
        /// The general IO manager;
        /// </summary>
        private readonly IGeneralIo generalIo;

        /// <summary>
        /// The instance of the logger.
        /// </summary>
        private readonly IJHcLogger logger;

        private AthleteRegisterToSeasonDialog m_athleteSeasonRegDialog = null;
        private AthleteSeasonSummaryDialog m_athleteSeasonSummaryDialog = null;

        private AthleteRegisterToSeasonViewModel m_athleteSeasonRegViewModel = null;
        private AthleteSeasonSummaryViewModel m_athleteSeasonSummaryViewModel = null;

        private ObservableCollection<string> m_seasons = new ObservableCollection<string>();
        private int currentSeasonIndex = 0;
        private bool m_newSeasonAdditionEnabled = false;
        private string m_newSeason = string.Empty;

        private IBLMngr businessLayerManager;

        /// <summary>
        /// Initialises a new instance of the <see cref="SeasonPaneViewModel"/> class.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="businessLayerManager"></param>
        /// <param name="generalIo">general IO manager</param>
        /// <param name="logger">program logger</param>
        public SeasonPaneViewModel(
            IModel model,
            IBLMngr businessLayerManager,
            IGeneralIo generalIo,
            IJHcLogger logger)
        {
            this.logger = logger;
            this.model = model;
            this.businessLayerManager = businessLayerManager;
            this.generalIo = generalIo;
            //model.SeasonsCallback = new SeasonsDelegate(PopulateSeasons);

            OpenAthleteSeasonNewRegCommand =
              new SimpleCommand(
                this.OpenAthleteSeasonRegDialog,
                () => false);
            OpenAthleteSeasonSummaryCommand =
              new SimpleCommand(
                this.OpenAthleteSeasonSummaryDialog,
                this.IsLocationValid);
            this.NewSeasonCommand =
              new SimpleCommand(
                this.EnableNewSeasonFields,
                this.IsLocationValid);
            this.AddSeasonCommand =
              new SimpleCommand(
                this.AddNewSeason,
                this.NewSeasonValid);
            CancelSeasonCommand =
              new SimpleCommand(
                this.CancelNewSeasonFields);

            this.InitialiseSeasonPane();
        }

        /// <summary>
        /// Gets a value indicating whether the location is valid or not.
        /// </summary>
        public bool LocationValid =>
            this.generalIo.DataFolderExists && this.generalIo.ConfigurationFolderExists;

        /// <summary>
        /// Gets or sets seasons list
        /// </summary>
        public ObservableCollection<string> Seasons
        {
            get { return m_seasons; }
            set
            {
                m_seasons = value;
                RaisePropertyChangedEvent("Seasons");
            }
        }

        /// <summary>
        /// Gets and sets the index of the season list
        /// </summary>
        public int SelectedSeasonIndex
        {
            get
            {
                return currentSeasonIndex;
            }

            set
            {
                if (currentSeasonIndex == value)
                {
                    return;
                }

                currentSeasonIndex = value;
                RaisePropertyChangedEvent(nameof(this.SelectedSeasonIndex));

                this.LoadSeason(this.Seasons[value]);
            }
        }

        /// <summary>
        /// Gets and sets new season controls enabled flag.
        /// </summary>
        public bool NewSeasonAdditionEnabled
        {
            get { return m_newSeasonAdditionEnabled; }
            set
            {
                m_newSeasonAdditionEnabled = value;
                RaisePropertyChangedEvent("NewSeasonAdditionEnabled");
            }
        }

        /// <summary>
        /// Gets and sets the new season.
        /// </summary>
        public string NewSeason
        {
            get { return m_newSeason; }
            set
            {
                m_newSeason = value;
                RaisePropertyChangedEvent("NewSeason");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public CallDelegate SeasonUpdatedCallback
        {
            get;
            set;
        }

        public ICommand OpenAthleteSeasonNewRegCommand { get; private set; }
        public ICommand OpenAthleteSeasonSummaryCommand { get; private set; }
        public ICommand NewSeasonCommand { get; private set; }
        public ICommand AddSeasonCommand { get; private set; }
        public ICommand CancelSeasonCommand { get; private set; }

        /// <summary>
        /// Initialise this view model.
        /// </summary>
        public void InitialiseSeasonPane()
        {
            this.PopulateSeasons(this.model.Seasons);

            string currentSeason = this.businessLayerManager.LoadCurrentSeason();
            this.SelectCurrentSeason(currentSeason);
        }

        /// <summary>
        /// Create Directory, add to seasons list and select.
        /// Clear down the additions section.
        /// </summary>
        public void AddNewSeason()
        {
            string newSeasonName = this.NewSeason;

            if (!this.businessLayerManager.CreateNewSeason(newSeasonName))
            {
                this.businessLayerManager.SetProgressInformation($"Failed to create season {newSeasonName}");
                return;
            }

            this.Seasons.Add(newSeasonName);
            this.SelectCurrentSeason(this.NewSeason);

            this.NewSeason = string.Empty;
            this.NewSeasonAdditionEnabled = false;

            this.businessLayerManager.SetProgressInformation($"{newSeasonName} created");
        }

        /// <summary>
        /// Ensure that the season is valid
        /// - It isn't blank.
        /// - It doesn't already exist.
        /// </summary>
        public bool NewSeasonValid()
        {
            if (NewSeason == string.Empty)
            {
                return false;
            }

            return !Seasons.Any(season => season == NewSeason);
        }

        /// <summary>
        /// Takes a list of all available seasons and adds them to the Seasons collection.
        /// </summary>
        /// <param name="seasons">seasons list</param>
        public void PopulateSeasons(List<string> seasons)
        {
            Seasons = new ObservableCollection<string>();
            Seasons.Add(string.Empty);
            foreach (string season in seasons)
            {
                Seasons.Add(season);
            }
        }

        /// <summary>
        /// Finds the indicated season and sets the index for it.
        /// </summary>
        /// <param name="currentSeason">season to find</param>
        private void SelectCurrentSeason(string currentSeason)
        {
            for (int seasonIndex = 0; seasonIndex < m_seasons.Count(); ++seasonIndex)
            {
                if (Seasons[seasonIndex] == currentSeason)
                {
                    this.SelectedSeasonIndex = seasonIndex;
                    break;
                }
            }
        }

        /// <summary>
        /// Requests that a season is loaded into memory.
        /// </summary>
        /// <param name="seasons">seasons list</param>
        private void LoadSeason(string seasonName)
        {
            if (this.SelectedSeasonIndex >= 0)
            {
                this.logger.WriteLog("Load season " + seasonName);

                this.businessLayerManager.LoadNewSeason(seasonName);

                if (this.SeasonUpdatedCallback != null)
                {
                    this.SeasonUpdatedCallback.Invoke();
                }
                //SelectCurrentEvent(BLMngr.LoadCurrentEvent());
            }
        }

        /// <summary>
        /// Requests that the progress information label in the model is cleared.
        /// </summary>
        public void ClearModelProgess()
        {
            this.businessLayerManager.SetProgressInformation(string.Empty);
        }

        /// <summary>
        /// Opens the athlete registration dialog
        /// </summary>
        public void OpenAthleteSeasonRegDialog()
        {
            if (m_athleteSeasonRegDialog == null)
            {
                m_athleteSeasonRegDialog = new AthleteRegisterToSeasonDialog();
            }

            m_athleteSeasonRegDialog.Unloaded -= new System.Windows.RoutedEventHandler(CloseAthleteSeasonRegDialog);
            m_athleteSeasonRegDialog.Unloaded += new System.Windows.RoutedEventHandler(CloseAthleteSeasonRegDialog);

            m_athleteSeasonRegViewModel = new AthleteRegisterToSeasonViewModel(this.model);
            m_athleteSeasonRegDialog.DataContext = m_athleteSeasonRegViewModel;

            try
            {
                m_athleteSeasonRegDialog.Show();
                m_athleteSeasonRegDialog.Activate();
            }
            catch (Exception ex)
            {
                this.logger.WriteLog(ex.ToString());
            }
        }

        /// <summary>
        /// Closes the athlete registration dialog
        /// </summary>
        public void CloseAthleteSeasonRegDialog(object sender, System.Windows.RoutedEventArgs e)
        {
            m_athleteSeasonRegDialog = null;
        }

        /// <summary>
        /// Opens the athlete summary dialog
        /// </summary>
        public void OpenAthleteSeasonSummaryDialog()
        {
            if (m_athleteSeasonSummaryDialog == null)
            {
                m_athleteSeasonSummaryDialog = new AthleteSeasonSummaryDialog();
            }

            m_athleteSeasonSummaryDialog.Unloaded -= new RoutedEventHandler(CloseAthleteSeasonSummaryDialog);
            m_athleteSeasonSummaryDialog.Unloaded += new RoutedEventHandler(CloseAthleteSeasonSummaryDialog);

            m_athleteSeasonSummaryViewModel = new AthleteSeasonSummaryViewModel(this.model);
            m_athleteSeasonSummaryDialog.DataContext = m_athleteSeasonSummaryViewModel;

            m_athleteSeasonSummaryDialog.Show();
            m_athleteSeasonSummaryDialog.Activate();
        }

        /// <summary>
        /// Closes the athlete summary dialog
        /// </summary>
        public void CloseAthleteSeasonSummaryDialog(object sender, System.Windows.RoutedEventArgs e)
        {
            m_athleteSeasonSummaryDialog = null;
        }


        /// <summary>
        /// Provide the ability to create a new season.
        /// </summary>
        private void EnableNewSeasonFields()
        {
            this.NewSeasonAdditionEnabled = true;
            this.ClearModelProgess();
        }

        /// <summary>
        /// Provide the ability to create a new season.
        /// </summary>
        private void CancelNewSeasonFields()
        {
            this.NewSeasonAdditionEnabled = false;
        }

        /// <summary>
        /// Returns a value which indicates whether the location is valid.
        /// </summary>
        /// <returns>is location valid flag</returns>
        private bool IsLocationValid()
        {
            return this.LocationValid;
        }
    }
}