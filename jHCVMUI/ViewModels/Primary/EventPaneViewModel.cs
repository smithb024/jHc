namespace jHCVMUI.ViewModels.Primary
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;

    using CommonHandicapLib.Interfaces;
    using CommonHandicapLib.Messages;
    using CommonLib.Types;
    using GalaSoft.MvvmLight.Messaging;
    using HandicapModel.Admin.Manage;
    using HandicapModel.Interfaces;
    using HandicapModel.Interfaces.Admin.IO;
    using HandicapModel.Interfaces.Admin.IO.TXT;
    using jHCVMUI.ViewModels.ViewModels;
    using jHCVMUI.Views.Windows.Results;

    using NynaeveLib.Commands;

    public class EventPaneViewModel : ViewModelBase
    {
        /// <summary>
        /// General IO manager
        /// </summary>
        private readonly IGeneralIo generalIo;

        /// <summary>
        /// Common IO manager
        /// </summary>
        private readonly ICommonIo commonIo;

        /// <summary>
        /// aplication logger
        /// </summary>
        private readonly IJHcLogger logger;

        private EventRawResultsDlg m_eventRawResultsDialog = null;
        private ImportEventRawResultDialog eventImportResultsDialog = null;

        private EventRawResultsViewModel m_eventRawResultsViewModel = null;

        private ObservableCollection<string> m_events = new ObservableCollection<string>();
        private int m_currentEventIndex = 0;
        private bool m_newEventAdditionEnabled = false;
        private string m_newEvent = string.Empty;
        private DateType m_newEventDate = new DateType(DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year);

        private string runResultsButtonName = string.Empty;

        /// <summary>
        /// Junior handicap model.
        /// </summary>
        private IModel model;

        /// <summary>
        /// Business layer manager.
        /// </summary>
        private IBLMngr businessLayerManager;

        /// <summary>
        /// Initialise a new instance of the <see cref="EventPaneViewModel"/> class.
        /// </summary>
        /// <param name="model">junior handicap model</param>
        /// <param name="businessLayerManager">business layer manager</param>
        /// <param name="generalIo">general IO manager</param>
        /// <param name="commonIo">common IO manager</param>
        /// <param name="logger">application logger</param>
        public EventPaneViewModel(
            IModel model,
            IBLMngr businessLayerManager,
            IGeneralIo generalIo,
            ICommonIo commonIo,
            IJHcLogger logger)
        {
            this.model = model;
            this.businessLayerManager = businessLayerManager;
            this.generalIo = generalIo;
            this.commonIo = commonIo;
            this.logger = logger;

            model.CurrentSeason.HandicapEventsChanged += this.ModelEventsChanged;
            this.PopulateEvents();
            //model.CurrentSeason.EventsCallback = new EventsDelegate(PopulateEvents);

            // TODO, this.IsLocationValid has been copied from PrimaryDisplayViewModel. Can this be rationalised.
            NewEventCommand =
              new SimpleCommand(
                this.EnableNewEventFields,
                this.IsLocationValid);
            AddEventCommand =
              new SimpleCommand(
                this.AddNewEvent,
                this.NewEventValid);
            CancelEventCommand =
              new SimpleCommand(
                this.CancelNewEventFields);

            OpenEventRawResultsDlgCommand =
              new SimpleCommand(
                this.OpenEventRawResultsDialog,
                this.IsLocationValid);
            OpenEventImportResultsDlgCommand =
              new SimpleCommand(
                this.OpenEventImportResultsDialog,
                this.IsLocationValid);
            CalculateResultsCommand =
              new SimpleCommand(
                this.CalculateResults,
                this.CanCalculateResults);

            this.InitialiseEventPane();

            this.LoadEvent();
        }

        /// <summary>
        /// Gets a value indicating whether the location is valid or not.
        /// </summary>
        public bool LocationValid => 
            this.generalIo.DataFolderExists && this.generalIo.ConfigurationFolderExists;

        /// <summary>
        /// Gets and sets seasons list
        /// </summary>
        public ObservableCollection<string> Events
        {
            get { return m_events; }
            set
            {
                m_events = value;
                RaisePropertyChangedEvent("Events");
            }
        }

        /// <summary>
        /// Gets and sets the index of the season list
        /// </summary>
        public int SelectedEventIndex
        {
            get { return m_currentEventIndex; }
            set
            {
                m_currentEventIndex = value;
                this.LoadEvent();

                this.UpdateResultsButton();
                this.RaisePropertyChangedEvent(nameof(this.SelectedEventIndex));
            }
        }

        /// <summary>
        /// Gets and sets new event controls enabled flag.
        /// </summary>
        public bool NewEventAdditionEnabled
        {
            get { return m_newEventAdditionEnabled; }
            set
            {
                m_newEventAdditionEnabled = value;
                RaisePropertyChangedEvent("NewEventAdditionEnabled");
            }
        }

        /// <summary>
        /// Gets and sets the name of the currently selected event.
        /// </summary>
        public string NewEvent
        {
            get { return m_newEvent; }
            set
            {
                m_newEvent = value;
                RaisePropertyChangedEvent("NewEvent");
            }
        }

        /// <summary>
        /// Gets and sets the day of the new event.
        /// </summary>
        public int NewEventDay
        {
            get { return m_newEventDate.Day; }
            set
            {
                m_newEventDate.Day = value;
                RaisePropertyChangedEvent("NewEventDay");
            }
        }

        /// <summary>
        /// Gets and sets the name of the currently selected event.
        /// </summary>
        public int NewEventMonth
        {
            get { return m_newEventDate.Month; }
            set
            {
                m_newEventDate.Month = value;
                RaisePropertyChangedEvent("NewEventMonth");
            }
        }

        /// <summary>
        /// Gets and sets the name of the currently selected event.
        /// </summary>
        public int NewEventYear
        {
            get { return m_newEventDate.Year; }
            set
            {
                m_newEventDate.Year = value;
                RaisePropertyChangedEvent("NewEventYear");
            }
        }

        /// <summary>
        /// Gets and sets the date of the new event.
        /// </summary>
        public DateType NewEventDate
        {
            get { return m_newEventDate; }
            set { m_newEventDate = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string RunResultsButtonName
        {
            get { return runResultsButtonName; }
            set
            {
                runResultsButtonName = value;
                RaisePropertyChangedEvent("RunResultsButtonName");
            }
        }

        public ICommand CalculateResultsCommand { get; private set; }
        public ICommand OpenEventRawResultsDlgCommand { get; private set; }
        public ICommand OpenEventImportResultsDlgCommand { get; private set; }
        public ICommand NewEventCommand { get; private set; }
        public ICommand AddEventCommand { get; private set; }
        public ICommand CancelEventCommand { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public void InitialiseEventPane()
        {
            SelectCurrentEvent(this.businessLayerManager.LoadCurrentEvent());
            UpdateResultsButton();
        }

        /// <summary>
        /// Provide the ability to create a new event.
        /// </summary>
        private void EnableNewEventFields()
        {
            this.NewEventAdditionEnabled = true;
            this.ClearModelProgess();
        }

        /// <summary>
        /// Provide the ability to create a new event.
        /// </summary>
        private void CancelNewEventFields()
        {
            this.NewEventAdditionEnabled = false;
        }

        /// <summary>
        /// Create Directory, add to seasons list and select.
        /// Clear down the additions section.
        /// </summary>
        public void AddNewEvent()
        {
            if (this.businessLayerManager.CreateNewEvent(NewEvent, NewEventDate))
            {
                SelectCurrentEvent(NewEvent);

                NewEvent = string.Empty;
                NewEventAdditionEnabled = false;

                this.businessLayerManager.SetProgressInformation("Event created");
            }
        }

        /// <summary>
        /// Requests that an event is loaded into memory.
        /// </summary>
        private void LoadEvent()
        {
            if (this.SelectedEventIndex >= 0 &&
                this.SelectedEventIndex < this.Events.Count)
            {
                string eventName = this.Events[this.SelectedEventIndex];

                this.logger.WriteLog("Load event " + eventName);

                LoadNewEventMessage message =
                    new LoadNewEventMessage(
                        eventName);

                Messenger.Default.Send(message);
            }
        }

        /// <summary>
        /// Ensure that the event is valid
        /// - It isn't blank.
        /// - It doesn't already exist.
        /// </summary>
        public bool NewEventValid()
        {
            if (NewEvent == string.Empty)
            {
                return false;
            }

            return !Events.Any(newEvent => newEvent == NewEvent);
        }

        ///// <summary>
        ///// Takes a list of all available events and adds them to the Events collection.
        ///// </summary>
        ///// <param name="events">seasons list</param>
        //public void PopulateEvents(List<string> events)
        //{
        //    Events = new ObservableCollection<string>();
        //    Events.Add(string.Empty);
        //    foreach (string newEvent in events)
        //    {
        //        Events.Add(newEvent);
        //    }
        //}

        /// <summary>
        /// Takes a list of all available events and adds them to the Events collection.
        /// </summary>
        /// <param name="events">seasons list</param>
        public void ModelEventsChanged(object sender, EventArgs e)
        {
            this.PopulateEvents();
        }

        /// <summary>
        /// Finds the indicated season and sets the index for it.
        /// </summary>
        /// <param name="currentSeason">season to find</param>
        public void SelectCurrentEvent(string currentEvent)
        {
            if (currentEvent != string.Empty)
            {
                for (int eventIndex = 0; eventIndex < Events.Count(); ++eventIndex)
                {
                    if (Events[eventIndex] == currentEvent)
                    {
                        SelectedEventIndex = eventIndex;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Calculate the results for the currently selected event.
        /// </summary>
        public void CalculateResults()
        {
            if (this.model.CurrentEvent.ResultsTable.Entries.Count > 0)
            {
                this.businessLayerManager.DeleteResults();
            }
            else
            {
                this.businessLayerManager.CalculateResults();
            }

            UpdateResultsButton();
        }

        /// <summary>
        /// Ensure that the event is in a position where results can be calculated. TODO
        /// </summary>
        /// <returns>can calculate results flag</returns>
        public bool CanCalculateResults()
        {
            return this.LocationValid;
        }

        /// <summary>
        /// Requests that the progress information label in the model is cleared.
        /// </summary>
        public void ClearModelProgess()
        {
            this.businessLayerManager.SetProgressInformation(string.Empty);
        }

        /// <summary>
        /// Work out the label on the rsults button.
        /// </summary>
        private void UpdateResultsButton()
        {
            if (this.model.CurrentEvent.ResultsTable.Entries.Count > 0)
            {
                RunResultsButtonName = "Delete Results";
            }
            else
            {
                RunResultsButtonName = "Calculate Results";
            }
        }

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <summary>
        /// Opens the athlete registration dialog
        /// </summary>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        public void OpenEventRawResultsDialog()
        {
            if (m_eventRawResultsDialog == null)
            {
                m_eventRawResultsDialog = new EventRawResultsDlg();
            }

            m_eventRawResultsDialog.Unloaded -= new RoutedEventHandler(CloseEventRawResultsDialog);
            m_eventRawResultsDialog.Unloaded += new RoutedEventHandler(CloseEventRawResultsDialog);

            m_eventRawResultsViewModel =
                new EventRawResultsViewModel(
                    this.model.CurrentEvent,
                    this.model.Athletes,
                    this.commonIo,
                    this.logger);
            m_eventRawResultsDialog.DataContext = m_eventRawResultsViewModel;

            // Close the import dialog if on display. These should be mutually exclusive.
            if (eventImportResultsDialog != null)
            {
                eventImportResultsDialog.Close();
            }

            m_eventRawResultsDialog.Show();
            m_eventRawResultsDialog.Activate();
        }

        /// <summary>
        /// Opens the dialog used to import raw results from a text file.
        /// </summary>
        public void OpenEventImportResultsDialog()
        {
            if (eventImportResultsDialog == null)
            {
                eventImportResultsDialog = new ImportEventRawResultDialog();
            }

            eventImportResultsDialog.Unloaded -= new RoutedEventHandler(CloseEventImportResultsDialog);
            eventImportResultsDialog.Unloaded += new RoutedEventHandler(CloseEventImportResultsDialog);

            m_eventRawResultsViewModel =
                new EventRawResultsViewModel(
                    this.model.CurrentEvent,
                    this.model.Athletes,
                    this.commonIo,
                    this.logger);
            eventImportResultsDialog.DataContext = m_eventRawResultsViewModel;

            // Close the raw imput dialog if on display. These should be mutually exclusive.
            if (m_eventRawResultsDialog != null)
            {
                m_eventRawResultsDialog.Close();
            }

            eventImportResultsDialog.Show();
            eventImportResultsDialog.Activate();
        }

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <summary>
        /// Closes the event raw results dialog
        /// </summary>
        /// <param name="sender">window object</param>
        /// <param name="e">Event arguments</param>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        public void CloseEventRawResultsDialog(object sender, System.Windows.RoutedEventArgs e)
        {
            m_eventRawResultsDialog = null;
        }

        /// <summary>
        /// Closes the event import results dialog.
        /// </summary>
        /// <param name="sender">window object</param>
        /// <param name="e">event arguments</param>
        public void CloseEventImportResultsDialog(object sender, System.Windows.RoutedEventArgs e)
        {
            eventImportResultsDialog = null;
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
        /// Refresh and populate <see cref="Events"/> from the latest information in the model.
        /// </summary>
        private void PopulateEvents()
        {
            this.Events = new ObservableCollection<string>();
            this.Events.Add(string.Empty);
            foreach (string newEvent in this.model.CurrentSeason.Events)
            {
                this.Events.Add(newEvent);
            }
        }
    }
}