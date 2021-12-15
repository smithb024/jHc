namespace jHCVMUI.ViewModels.ViewModels
{
    using System.Windows;
    using System.Windows.Forms;
    using System.Windows.Input;

    using CommonHandicapLib;

    using HandicapModel;
    using HandicapModel.Admin.IO;
    using HandicapModel.Admin.Manage;
    using HandicapModel.Interfaces.Admin.IO;

    using NynaeveLib.Commands;
    using NynaeveLib.DialogService;

    using Common;
    using CommonHandicapLib.Interfaces;
    using jHCVMUI.ViewModels.Config;
    using DataEntry;
    using jHCVMUI.ViewModels.Labels;
    using jHCVMUI.ViewModels.Primary;
    using jHCVMUI.ViewModels.Primary.DataPanes;

    using jHCVMUI.Views.Configuration;
    using jHCVMUI.Views.DataEntry;
    using jHCVMUI.Views.Labels;
    using jHCVMUI.Views.Summary;
    using jHCVMUI.Views.Windows;
    using HandicapModel.Interfaces;
    using GalaSoft.MvvmLight.Messaging;
    using CommonHandicapLib.Messages;

    /// <summary>
    /// Primary display view model.
    /// </summary>
    public class PrimaryDisplayViewModel : ViewModelBase
    {
        /// <summary>
        /// The instance of the logger.
        /// </summary>
        private readonly IJHcLogger logger;

        /// <summary>
        /// The instance of the general IO manager.
        /// </summary>
        private readonly IGeneralIo generalIo;

        private ClubConfigurationDialog m_clubConfigDialog = null;
        private AthleteConfigurationDialog m_athleteConfigDialog = null;
        private SummaryDialog m_summaryDialog = null;
        private AthleteSummaryDialog m_athleteSummaryDialog = null;
        private ClubSummaryDialog clubSummaryDialog = null;
        private SummaryViewModel summaryViewModel = null;

        private ClubConfigurationViewModel m_clubConfigViewModel = null;
        private ClubSummaryViewModel clubSummaryViewModel = null;
        private AthleteConfigurationViewModel m_athleteConfigViewModel = null;
        private AthleteSummaryViewModel m_athleteSummaryViewModel = null;
        private SeasonPaneViewModel seasonPaneViewModel = null;
        private EventPaneViewModel eventPaneViewModel = null;
        private DataPaneViewModel dataPaneViewModel = null;

        private string progressInfo = string.Empty;
        private string errorInfo = string.Empty;

        /// <summary>
        /// The junior handicap model.
        /// </summary>
        private IModel model;

        /// <summary>
        /// Results configuration manager
        /// </summary>
        private IResultsConfigMngr resultsConfigurationManager;

        /// <summary>
        /// Business layer manager.
        /// </summary>
        private IBLMngr businessLayerManager;

        /// <summary>
        /// Creates a new instance of the <see cref="PrimaryDisplayViewModel"/> class
        /// </summary>
        /// <param name="model">application model</param>
        /// <param name="businessLayerManager">business layer manager</param>
        /// <param name="resultsConfigurationManager">results configuration manager</param>
        /// <param name="generalIo">general IO manager</param>
        /// <param name="logger">application logger</param>
        public PrimaryDisplayViewModel(
            IModel model,
            IBLMngr businessLayerManager,
            IResultsConfigMngr resultsConfigurationManager,
            IGeneralIo generalIo,
            IJHcLogger logger)
        {
            this.logger = logger;
            this.logger.WriteLog("HandicapMainViewModel created");
            this.model = model;
            this.resultsConfigurationManager = resultsConfigurationManager;
            this.businessLayerManager = businessLayerManager;
            this.generalIo = generalIo;

            Messenger.Default.Register<HandicapErrorMessage>(this, this.PopulateErrorInformation);
            Messenger.Default.Register<HandicapProgressMessage>(this, this.PopulateProgressInformation);

            this.InitialiseViewModels();
            this.InitialiseOpenAppCommands();
        }

        ///// <summary>
        ///// Gets or sets the main display season pane
        ///// </summary>
        //public SeasonPaneViewModel MainDisplaySeasonPane
        //{
        //  get { return seasonPaneViewModel; }
        //  set
        //  {
        //    seasonPaneViewModel = value;
        //    RaisePropertyChangedEvent("MainDisplaySeasonPane");
        //  }
        //}

        ///// <summary>
        ///// Gets or sets the main display event pane
        ///// </summary>
        //public EventPaneViewModel MainDisplayEventPane
        //{
        //  get { return eventPaneViewModel; }
        //  set
        //  {
        //    eventPaneViewModel = value;
        //    RaisePropertyChangedEvent("MainDisplayEventPane");
        //  }
        //}

        ///// <summary>
        ///// Gets or sets the main display event pane
        ///// </summary>
        //public DataPaneViewModel MainDisplayDataPane
        //{
        //  get { return dataPaneViewModel; }
        //  set
        //  {
        //    dataPaneViewModel = value;
        //    RaisePropertyChangedEvent("MainDisplayDataPane");
        //  }
        //}

        /// <summary>
        /// Gets and sets the current progress information.
        /// </summary>
        public string ProgressInformation
        {
            get { return progressInfo; }
            set
            {
                progressInfo = value;
                RaisePropertyChangedEvent("ProgressInformation");
            }
        }

        /// <summary>
        /// Gets and sets the current error information.
        /// </summary>
        public string ErrorInformation
        {
            get { return errorInfo; }
            set
            {
                errorInfo = value;
                RaisePropertyChangedEvent("ErrorInformation");
            }
        }

        /// <summary>
        /// Gets a value indicating whether the location is valid or not.
        /// </summary>
        public bool LocationValid =>
            this.generalIo.DataFolderExists && this.generalIo.ConfigurationFolderExists;

        public ICommand CreateNewSeriesCommand { get; private set; }
        public ICommand LoadNewSeriesCommand { get; private set; }
        public ICommand OpenClubCommand { get; private set; }
        public ICommand OpenClubSummaryCommand { get; private set; }
        public ICommand OpenAthleteCommand { get; private set; }
        public ICommand OpenSummaryCommand { get; private set; }
        public ICommand OpenAthleteSummaryCommand { get; private set; }
        public ICommand OpenSeriesConfigCommand { get; private set; }
        public ICommand OpenNormalisationConfigCommand { get; private set; }
        public ICommand OpenResultConfigCommand { get; private set; }
        public ICommand PrintAllCommand { get; private set; }
        public ICommand PrintRaceLabelsCommand { get; private set; }

        public ICommand OpenTimeEntryEditorComamnd { get; private set; }
        public ICommand OpenPositionEditorCommand { get; private set; }
        public ICommand OpenStopwatchP610EEditorCommand { get; private set; }

        public ICommand OpenOPN200Command { get; private set; }
        public ICommand OpenParkrunTimerCommand { get; private set; }
        public ICommand OpenStopwatch610Command { get; private set; }

        public bool CanClubDialogUpdate
        {
            get { return true; }
        }

        public bool CanAthleteDialogUpdate
        {
            get { return true; }
        }

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>OpenClubRegistrationDialog</name>
        /// <date>31/01/15</date>
        /// <summary>
        /// Opens the club registration dialog
        /// </summary>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        public void OpenClubRegistrationDialog()
        {
            if (m_clubConfigDialog == null)
            {
                m_clubConfigDialog = new ClubConfigurationDialog();
            }

            m_clubConfigDialog.Unloaded -= new System.Windows.RoutedEventHandler(CloseClubRegistrationDialog);
            m_clubConfigDialog.Unloaded += new System.Windows.RoutedEventHandler(CloseClubRegistrationDialog);

            m_clubConfigViewModel = new ClubConfigurationViewModel(this.model);
            m_clubConfigDialog.DataContext = m_clubConfigViewModel;

            m_clubConfigDialog.Show();
            m_clubConfigDialog.Activate();
        }

        /// <summary>
        /// Closes the club registration dialog
        /// </summary>
        public void CloseClubRegistrationDialog(object sender, System.Windows.RoutedEventArgs e)
        {
            m_clubConfigDialog = null;
            m_clubConfigViewModel = null;
        }

        /// <summary>
        /// Opens the athlete registration dialog
        /// </summary>
        public void OpenAthleteRegistrationDialog()
        {
            if (m_athleteConfigDialog == null)
            {
                m_athleteConfigDialog = new AthleteConfigurationDialog();
            }

            m_athleteConfigDialog.Unloaded -= new System.Windows.RoutedEventHandler(CloseAthleteRegistrationDialog);
            m_athleteConfigDialog.Unloaded += new System.Windows.RoutedEventHandler(CloseAthleteRegistrationDialog);

            m_athleteConfigViewModel = new AthleteConfigurationViewModel(this.model);
            m_athleteConfigDialog.DataContext = m_athleteConfigViewModel;

            m_athleteConfigDialog.Show();
            m_athleteConfigDialog.Activate();
        }

        /// <summary>
        /// Closes the athlete registration dialog
        /// </summary>
        public void CloseAthleteRegistrationDialog(object sender, System.Windows.RoutedEventArgs e)
        {
            m_athleteConfigDialog = null;
        }

        /// <summary>
        /// Opens the athlete registration dialog
        /// </summary>
        public void OpenSummaryDialog()
        {
            if (m_summaryDialog == null)
            {
                m_summaryDialog = new SummaryDialog();
            }

            m_summaryDialog.Unloaded -= new System.Windows.RoutedEventHandler(CloseSummaryDialog);
            m_summaryDialog.Unloaded += new System.Windows.RoutedEventHandler(CloseSummaryDialog);

            summaryViewModel = 
                new SummaryViewModel(
                    this.model.GlobalSummary);
            m_summaryDialog.DataContext = summaryViewModel;

            m_summaryDialog.Show();
            m_summaryDialog.Activate();
        }

        /// <summary>
        /// Closes the athlete registration dialog
        /// </summary>
        public void CloseSummaryDialog(object sender, System.Windows.RoutedEventArgs e)
        {
            m_summaryDialog = null;
        }

        /// <summary>
        /// Opens the athlete summary dialog
        /// </summary>
        public void OpenAthleteSummaryDialog()
        {
            if (m_athleteSummaryDialog == null)
            {
                m_athleteSummaryDialog = new AthleteSummaryDialog();
            }

            m_athleteSummaryDialog.Unloaded -= new RoutedEventHandler(CloseAthleteSummaryDialog);
            m_athleteSummaryDialog.Unloaded += new RoutedEventHandler(CloseAthleteSummaryDialog);

            m_athleteSummaryDialog.DataContext =
              new AthleteSummaryViewModel(
                  this.model,
                this.resultsConfigurationManager);

            m_athleteSummaryDialog.Show();
            m_athleteSummaryDialog.Activate();
        }

        /// <summary>
        /// Closes the athlete summary dialog
        /// </summary>
        public void CloseAthleteSummaryDialog(object sender, System.Windows.RoutedEventArgs e)
        {
            m_athleteSummaryDialog = null;
        }

        /// <summary>
        /// Opens the athlete summary dialog
        /// </summary>
        public void OpenClubSummaryDialog()
        {
            if (clubSummaryDialog == null)
            {
                clubSummaryDialog = new ClubSummaryDialog();
            }

            clubSummaryDialog.Unloaded -= new RoutedEventHandler(CloseClubSummaryDialog);
            clubSummaryDialog.Unloaded += new RoutedEventHandler(CloseClubSummaryDialog);

            clubSummaryViewModel = new ClubSummaryViewModel(this.model);
            clubSummaryDialog.DataContext = clubSummaryViewModel;

            clubSummaryDialog.Show();
            clubSummaryDialog.Activate();
        }

        /// <summary>
        /// Closes the athlete summary dialog
        /// </summary>
        public void CloseClubSummaryDialog(object sender, System.Windows.RoutedEventArgs e)
        {
            clubSummaryDialog = null;
        }

        /// <summary>
        /// Open the series configuration value as a dialog.
        /// </summary>
        public void OpenSeriesConfigDialog()
        {
            SeriesConfigurationDialog seriesConfigDialog = new SeriesConfigurationDialog();
            SeriesConfigViewModel seriesConfigViewModel = new SeriesConfigViewModel();

            seriesConfigDialog.DataContext = seriesConfigViewModel;

            seriesConfigDialog.ShowDialog();

            seriesConfigDialog = null;
            seriesConfigViewModel = null;
        }

        /// <summary>
        /// Open the normalisation configuration value as a dialog.
        /// </summary>
        public void OpenNormalisationConfigDialog()
        {
            NormalisationConfigDialog normalisationConfigDialog = new NormalisationConfigDialog();
            NormalisationConfigViewModel normalisationConfigViewModel = new NormalisationConfigViewModel();

            normalisationConfigDialog.DataContext = normalisationConfigViewModel;

            normalisationConfigDialog.ShowDialog();

            normalisationConfigDialog = null;
            normalisationConfigViewModel = null;
        }

        /// <summary>
        /// Open the results configuration value as a dialog.
        /// </summary>
        public void OpenResultsConfigDialog()
        {
            ResultsConfigDialog resultsConfigDialog = new ResultsConfigDialog();
            ResultsConfigViewModel resultsConfigViewModel =
              new ResultsConfigViewModel(
                this.resultsConfigurationManager);

            resultsConfigDialog.DataContext = resultsConfigViewModel;

            resultsConfigDialog.ShowDialog();

            resultsConfigDialog = null;
            resultsConfigViewModel = null;
        }

        /// <summary>
        /// Print all data to disk.
        /// </summary>
        public void PrintAll()
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.businessLayerManager.PrintAll(dialog.SelectedPath);
            }
            else
            {
                this.CancelPrint();
            }
        }

        /// <summary>
        /// Choose a folder to save race labels to, then open the dialog.
        /// </summary>
        public void PrintRaceLabels()
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.OpenRaceLabelsDialog(dialog.SelectedPath);
            }
            else
            {
                this.CancelPrint();
            }
        }

        /// <summary>
        /// Open the print race labels to the 
        /// </summary>
        public void OpenRaceLabelsDialog(string folder)
        {
            LabelGenerationDialog labelDialog =
                new LabelGenerationDialog();
            LabelGenerationViewModel labelViewModel =
                new LabelGenerationViewModel(
                    this.model,
                    folder);

            labelDialog.DataContext = labelViewModel;

            labelDialog.ShowDialog();
        }

        /// <summary>
        /// Cancel the print command
        /// </summary>
        public void CancelPrint()
        {
            HandicapProgressMessage progress = new HandicapProgressMessage("Print cancelled");
            Messenger.Default.Send(progress);
        }

        public void OpenTimeEntryEditorDialog()
        {
            TimeEntryDialogViewModel dialogViewModel = new TimeEntryDialogViewModel();

            DialogService service = new DialogService();

            service.ShowDialog(
              new TimeEntryDialog()
              {
                  DataContext = dialogViewModel
              });
        }

        public void OpenPositionEditorDialog()
        {
            PositionEditorDialogViewModel dialogViewModel =
                new PositionEditorDialogViewModel(
                    this.model);

            DialogService service = new DialogService();

            service.ShowDialog(
              new PositionEditorDialog()
              {
                  DataContext = dialogViewModel
              });
        }

        public void OpenStopwatchP610EEditorDialog()
        {
            StopwatchP610EditorDialogViewModel dialogViewModel =
              new StopwatchP610EditorDialogViewModel();

            DialogService service = new DialogService();

            service.ShowDialog(
              new StopwatchP610EditorDialog()
              {
                  DataContext = dialogViewModel
              });
        }

        /// <summary>
        /// Populate the error information
        /// </summary>
        /// <param name="errorInformation">error information</param>
        public void PopulateErrorInformation(HandicapErrorMessage errorInformation)
        {
            this.ErrorInformation = errorInformation.ErroMessage;
        }

        /// <summary>
        /// Populate the progress information
        /// </summary>
        /// <param name="progressInformaion">progress information</param>
        public void PopulateProgressInformation(HandicapProgressMessage progressInformaion)
        {
            this.ProgressInformation = progressInformaion.ProgressMessage;
        }

        /// <summary>
        /// Requests that the progress information label in the model is cleared.
        /// </summary>
        public void ClearModelProgess()
        {
            this.businessLayerManager.SetProgressInformation(string.Empty);
        }

        /// <summary>
        /// Gets a new location and load it.
        /// </summary>
        public void LoadNewSeries()
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.SelectedPath = this.businessLayerManager.ModelRootDirectory;
            DialogResult result = dialog.ShowDialog();

            this.businessLayerManager.SaveRootDirectory(dialog.SelectedPath);
            this.InitialiseViewModels();

            HandicapProgressMessage progress = new HandicapProgressMessage("New Series Loaded");
            Messenger.Default.Send(progress);
        }

        /// <summary>
        /// Create new series data in the current folder.
        /// </summary>
        public void CreateNewSeries()
        {
            this.generalIo.CreateConfigurationFolder();
            this.generalIo.CreateDataFolder();
            this.InitialiseViewModels();
            SeriesConfigMngr.ReadSeriesConfiguration();

            HandicapProgressMessage progress = new HandicapProgressMessage("New Series Created");
            Messenger.Default.Send(progress);
        }

        /// <summary>
        /// 
        /// </summary>
        private void SeasonUpdated()
        {
            //MainDisplayEventPane.SelectCurrentEvent(this.businessLayerManager.LoadCurrentEvent());
        }

        /// <summary>
        /// Run all initialisation tasks.
        /// </summary>
        private void InitialiseViewModels()
        {
            if (this.resultsConfigurationManager == null)
            {
                this.logger.WriteLog(
                  "PrimaryDisplayViewModel failed to initalise, null results config manager");
                return;
            }

            //if (this.LocationValid)
            //{
                this.businessLayerManager.InitialiseModel();
            //}
            //else
            //{
            //    this.businessLayerManager.SimpleInitialiseModel();
            //}

            //this.model.ErrorCallback = new InformationDelegate(PopulateErrorInformation);
            //this.model.ProgressCallback = new InformationDelegate(PopulateProgressInformation);

            // Ensure any information which may have been missed is updated to the view model.
            //PopulateErrorInformation(this.model.ErrorInformation);
            //PopulateProgressInformation(this.model.ProgressInformation);

            //MainDisplaySeasonPane =
            //  new SeasonPaneViewModel(
            //    this.businessLayerManager);
            //MainDisplayEventPane =
            //  new EventPaneViewModel(
            //    this.businessLayerManager);
            //MainDisplayDataPane = new DataPaneViewModel();

            //MainDisplaySeasonPane.SeasonUpdatedCallback = new CallDelegate(SeasonUpdated);

            this.CreateNewSeriesCommand =
              new SimpleCommand(
                this.CreateNewSeries,
                this.IsLocationNotValid);
            this.LoadNewSeriesCommand =
              new SimpleCommand(
                this.LoadNewSeries);
            this.OpenClubCommand =
              new SimpleCommand(
                this.OpenClubRegistrationDialog,
                this.IsLocationValid);
            this.OpenClubSummaryCommand =
              new SimpleCommand(
                this.OpenClubSummaryDialog,
                this.IsLocationValid);
            this.OpenAthleteCommand =
              new SimpleCommand(
                this.OpenAthleteRegistrationDialog,
                this.IsLocationValid);
            this.OpenSummaryCommand =
              new SimpleCommand(
                this.OpenSummaryDialog,
                this.IsLocationValid);
            this.OpenAthleteSummaryCommand =
              new SimpleCommand(
                this.OpenAthleteSummaryDialog,
                this.IsLocationValid);
            this.OpenSeriesConfigCommand =
              new SimpleCommand(
                this.OpenSeriesConfigDialog,
                this.IsLocationValid);
            this.OpenNormalisationConfigCommand =
              new SimpleCommand(
                this.OpenNormalisationConfigDialog,
                this.IsLocationValid);
            this.OpenResultConfigCommand =
              new SimpleCommand(
                this.OpenResultsConfigDialog,
                this.IsLocationValid);
            this.PrintAllCommand =
              new SimpleCommand(
                this.PrintAll,
                this.IsLocationValid);
            this.PrintRaceLabelsCommand =
              new SimpleCommand(
                this.PrintRaceLabels,
                this.IsLocationValid);

            this.OpenTimeEntryEditorComamnd =
              new SimpleCommand(
                this.OpenTimeEntryEditorDialog,
                this.IsLocationValid);
            this.OpenPositionEditorCommand =
              new SimpleCommand(
                this.OpenPositionEditorDialog,
                this.IsLocationValid);
            this.OpenStopwatchP610EEditorCommand =
              new SimpleCommand(
                this.OpenStopwatchP610EEditorDialog,
                this.IsLocationValid);

            //MainDisplaySeasonPane.InitialiseSeasonPane();
            //MainDisplayEventPane.InitialiseEventPane();
            //MainDisplayDataPane.InitialiseDataPane();
        }

        /// <summary>
        /// Set up the commands used to open the COTS applications.
        /// </summary>
        /// <remarks>
        /// These are hard coded and locations are predetermined. It's not good practice, but will
        /// have to do for now. I've looked at a dynamic solution, but couldn't get it to bind onto
        /// the menu items. Needs more investigation work.
        /// </remarks>
        private void InitialiseOpenAppCommands()
        {
            // TODO, consider dynamic solution.
            this.OpenOPN200Command =
              OpenCotsFactory.CreateCommand(
                "OPN200x");
            this.OpenParkrunTimerCommand =
              OpenCotsFactory.CreateCommand(
                "parkrun timer DL");
            this.OpenStopwatch610Command =
              OpenCotsFactory.CreateCommand(
                "610P_EN");
        }

        /// <summary>
        /// Returns a value which indicates whether the location is valid.
        /// </summary>
        /// <returns>is location valid flag</returns>
        private bool IsLocationValid()
        {
            return this.LocationValid;
        }

        /// <summary>
        /// Returns a value which indicates whether the location is not valid.
        /// </summary>
        /// <returns>is location not valid flag</returns>
        private bool IsLocationNotValid()
        {
            return !this.LocationValid;
        }
    }
}