namespace jHCVMUI.ViewModels.Primary
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;

    using CommonHandicapLib.Interfaces;
    using CommonHandicapLib.Messages;
    using CommonLib.Types;
    using HandicapModel.Admin.Manage;
    using HandicapModel.Interfaces;
    using HandicapModel.Interfaces.Admin.IO;
    using HandicapModel.Interfaces.Admin.IO.TXT;
    using jHCVMUI.ViewModels.ViewModels;
    using jHCVMUI.ViewModels.ViewModels.Windows.Results;
    using jHCVMUI.Views.Windows.Results;
    using NynaeveLib.Commands;
    using CommonMessenger = NynaeveLib.Messenger.Messenger;

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
        /// Event IO manager.
        /// </summary>
        private readonly IEventIo eventIo;

        /// <summary>
        /// aplication logger
        /// </summary>
        private readonly IJHcLogger logger;

        /// <summary>
        /// Junior handicap model.
        /// </summary>
        private readonly IModel model;

        /// <summary>
        /// Business layer manager.
        /// </summary>
        private readonly IBLMngr businessLayerManager;

        /// <summary>
        /// The instance of the raw results dialog.
        /// </summary>
        private EventRawResultsDlg eventRawResultsDialog = null;

        /// <summary>
        /// The instance of the import raw results dialog.
        /// </summary>
        private ImportEventRawResultDialog eventImportResultsDialog = null;

        /// <summary>
        /// The collection of event names.
        /// </summary>
        private ObservableCollection<string> events = new ObservableCollection<string>();

        /// <summary>
        /// The index of the currently selected event.
        /// </summary>
        private int currentEventIndex = 0;

        /// <summary>
        /// Indicates whether it is possible to add a new event. 
        /// </summary>
        /// <remarks>
        /// The add new event controls are controlled unless specifically shown.
        /// </remarks>
        private bool newEventAdditionEnabled = false;

        /// <summary>
        /// The name of a proposed new event.
        /// </summary>
        private string newEvent = string.Empty;

        /// <summary>
        /// The initial suggestion for the date of a new event.
        /// </summary>
        private DateType newEventDate;

        /// <summary>
        /// The name of the results button.
        /// </summary>
        private string runResultsButtonName = string.Empty;

        /// <summary>
        /// The name of the current season.
        /// </summary>
        private string seasonName;

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
            IEventIo eventIo,
            IJHcLogger logger)
        {
            this.model = model;
            this.businessLayerManager = businessLayerManager;
            this.generalIo = generalIo;
            this.commonIo = commonIo;
            this.eventIo = eventIo;
            this.logger = logger;

            this.seasonName = this.model.CurrentSeason.Name;

            this.NewEventDate =
                new DateType(
                    DateTime.Now.Day,
                    DateTime.Now.Month, 
                    DateTime.Now.Year);

            // TODO, this.IsLocationValid has been copied from PrimaryDisplayViewModel. Can this be rationalised.
            this.NewEventCommand =
              new SimpleCommand(
                this.EnableNewEventFields,
                this.IsLocationValid);
            this.AddEventCommand =
              new SimpleCommand(
                this.AddNewEvent,
                this.NewEventValid);
            this.CancelEventCommand =
              new SimpleCommand(
                this.CancelNewEventFields);

            this.OpenEventRawResultsDlgCommand =
              new SimpleCommand(
                this.OpenEventRawResultsDialog,
                this.IsLocationValid);
            this.OpenEventImportResultsDlgCommand =
              new SimpleCommand(
                this.OpenEventImportResultsDialog,
                this.IsLocationValid);
            this.CalculateResultsCommand =
              new SimpleCommand(
                this.CalculateResults,
                this.CanCalculateResults);

            this.InitialiseEventPane();

            CommonMessenger.Default.Register<LoadNewSeasonMessage>(this, this.NewSeasonSelected);
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
            get => this.events; 
            set
            {
                this.events = value;
                this.RaisePropertyChangedEvent(nameof(this.Events));
            }
        }

        /// <summary>
        /// Gets and sets the index of the season list
        /// </summary>
        public int SelectedEventIndex
        {
            get => this.currentEventIndex;
            set
            {
                this.currentEventIndex = value;
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
            get => this.newEventAdditionEnabled; 
            set
            {
                this.newEventAdditionEnabled = value;
                this.RaisePropertyChangedEvent(nameof(this.NewEventAdditionEnabled));
            }
        }

        /// <summary>
        /// Gets and sets the name of the currently selected event.
        /// </summary>
        public string NewEvent
        {
            get => this.newEvent; 
            set
            {
                this.newEvent = value;
                this.RaisePropertyChangedEvent(nameof(this.NewEvent));
            }
        }

        /// <summary>
        /// Gets and sets the day of the new event.
        /// </summary>
        public int NewEventDay
        {
            get => this.newEventDate.Day;
            set
            {
                this.newEventDate.Day = value;
                this.RaisePropertyChangedEvent(nameof(this.NewEventDay));
            }
        }

        /// <summary>
        /// Gets and sets the name of the currently selected event.
        /// </summary>
        public int NewEventMonth
        {
            get => this.newEventDate.Month;
            set
            {
                this.newEventDate.Month = value;
                this.RaisePropertyChangedEvent(nameof(this.NewEventMonth));
            }
        }

        /// <summary>
        /// Gets and sets the name of the currently selected event.
        /// </summary>
        public int NewEventYear
        {
            get => this.newEventDate.Year;
            set
            {
                this.newEventDate.Year = value;
                this.RaisePropertyChangedEvent(nameof(this.NewEventYear));
            }
        }

        /// <summary>
        /// Gets and sets the date of the new event.
        /// </summary>
        public DateType NewEventDate
        {
            get => this.newEventDate; 
            set => this.newEventDate = value; 
        }

        /// <summary>
        /// 
        /// </summary>
        public string RunResultsButtonName
        {
            get => this.runResultsButtonName;
            set
            {
                this.runResultsButtonName = value;
                this.RaisePropertyChangedEvent(nameof(this.RunResultsButtonName));
            }
        }

        /// <summary>
        /// Gets the button which is used to calculate the results.
        /// </summary>
        public ICommand CalculateResultsCommand { get; private set; }
        public ICommand OpenEventRawResultsDlgCommand { get; private set; }
        public ICommand OpenEventImportResultsDlgCommand { get; private set; }
        public ICommand NewEventCommand { get; private set; }
        public ICommand AddEventCommand { get; private set; }
        public ICommand CancelEventCommand { get; private set; }

        /// <summary>
        /// Initialise the controls on the view model.
        /// </summary>
        public void InitialiseEventPane()
        {
            string currentEventName = this.businessLayerManager.LoadCurrentEvent();

            this.PopulateEvents();
            this.SelectCurrentEvent(currentEventName);
            this.UpdateResultsButton();
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
            string newEventName = this.NewEvent;

            if (!this.businessLayerManager.CreateNewEvent(this.NewEvent, this.NewEventDate))
            {
                this.businessLayerManager.SetProgressInformation($"Failed to create season {newEventName}");
                return;
            }

            this.Events.Add(newEventName);
            SelectCurrentEvent(newEventName);

            this.NewEvent = string.Empty;
            this.NewEventAdditionEnabled = false;

            this.businessLayerManager.SetProgressInformation($"{newEventName} created");
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

                CommonMessenger.Default.Send(message);
            }
        }

        /// <summary>
        /// Ensure that the event is valid
        /// - It isn't blank.
        /// - It doesn't already exist.
        /// </summary>
        public bool NewEventValid()
        {
            if (this.NewEvent == string.Empty)
            {
                return false;
            }

            return !this.Events.Any(newEvent => newEvent == this.NewEvent);
        }

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
            for (int eventIndex = 0; eventIndex < this.Events.Count(); ++eventIndex)
            {
                if (this.Events[eventIndex] == currentEvent)
                {
                    this.SelectedEventIndex = eventIndex;
                    break;
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

            this.UpdateResultsButton();
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
                this.RunResultsButtonName = "Delete Results";
            }
            else
            {
                this.RunResultsButtonName = "Calculate Results";
            }
        }

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <summary>
        /// Opens the athlete registration dialog
        /// </summary>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        public void OpenEventRawResultsDialog()
        {
            if (this.eventRawResultsDialog == null)
            {
                this.eventRawResultsDialog = new EventRawResultsDlg();
            }

            this.eventRawResultsDialog.Unloaded -= new RoutedEventHandler(this.CloseEventRawResultsDialog);
            this.eventRawResultsDialog.Unloaded += new RoutedEventHandler(this.CloseEventRawResultsDialog);

            EventRawResultsDlgViewModel viewModel =
                new EventRawResultsDlgViewModel(
                    this.model.CurrentEvent,
                    this.model.Athletes);
            this.eventRawResultsDialog.DataContext = viewModel;

            // Close the import dialog if on display. These should be mutually exclusive.
            if (this.eventImportResultsDialog != null)
            {
                this.eventImportResultsDialog.Close();
            }

            this.eventRawResultsDialog.Show();
            this.eventRawResultsDialog.Activate();
        }

        /// <summary>
        /// Opens the dialog used to import raw results from a text file.
        /// </summary>
        public void OpenEventImportResultsDialog()
        {
            if (this.eventImportResultsDialog == null)
            {
                this.eventImportResultsDialog = new ImportEventRawResultDialog();
            }

            this.eventImportResultsDialog.Unloaded -= new RoutedEventHandler(this.CloseEventImportResultsDialog);
            this.eventImportResultsDialog.Unloaded += new RoutedEventHandler(this.CloseEventImportResultsDialog);

            ImportEventRawResultDialogViewModel viewModel =
                new ImportEventRawResultDialogViewModel(
                    this.model.CurrentEvent,
                    this.model.Athletes,
                    this.commonIo,
                    this.logger);
            this.eventImportResultsDialog.DataContext = viewModel;

            // Close the raw imput dialog if on display. These should be mutually exclusive.
            if (this.eventRawResultsDialog != null)
            {
                this.eventRawResultsDialog.Close();
            }

            this.eventImportResultsDialog.Show();
            this.eventImportResultsDialog.Activate();
        }

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <summary>
        /// Closes the event raw results dialog
        /// </summary>
        /// <param name="sender">window object</param>
        /// <param name="e">Event arguments</param>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        public void CloseEventRawResultsDialog(object sender, RoutedEventArgs e)
        {
            this.eventRawResultsDialog = null;
        }

        /// <summary>
        /// Closes the event import results dialog.
        /// </summary>
        /// <param name="sender">window object</param>
        /// <param name="e">event arguments</param>
        public void CloseEventImportResultsDialog(object sender, RoutedEventArgs e)
        {
            this.eventImportResultsDialog = null;
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

            List<string> events =
                this.eventIo.GetEvents(
                    this.seasonName);
            foreach (string newEvent in events)
            {
                this.Events.Add(newEvent);
            }
        }


        /// <summary>
        /// A new season has been selected, reinitialise the view model to reflect the new season.
        /// </summary>
        /// <param name="message">the load new season message</param>
        private void NewSeasonSelected(LoadNewSeasonMessage message)
        {
            this.seasonName = message.Name;
            this.InitialiseEventPane();
        }
    }
}