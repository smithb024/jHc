namespace HandicapModel.SeasonModel.EventModel
{
    using System;
    using System.Collections.Generic;
    using CommonHandicapLib.Interfaces;
    using CommonHandicapLib.Messages;
    using CommonHandicapLib.Types;
    using CommonLib.Types;
    using HandicapModel.Common;
    using HandicapModel.Interfaces.Admin.IO;
    using HandicapModel.Interfaces.Admin.IO.TXT;
    using HandicapModel.Interfaces.Admin.IO.XML;
    using HandicapModel.Interfaces.Common;
    using HandicapModel.Interfaces.SeasonModel.EventModel;
    using CommonMessenger = NynaeveLib.Messenger.Messenger;

    /// <summary>
    /// A single model event.
    /// </summary>
    public class EventHC : IHandicapEvent
    {
        /// <summary>
        /// The event data wrapper.
        /// </summary>
        private readonly IEventData eventData;

        /// <summary>
        /// The summary data wrapper
        /// </summary>
        private readonly ISummaryData summaryData;

        /// <summary>
        /// The results table reader.
        /// </summary>
        private readonly IResultsTableReader resultsTableReader;

        /// <summary>
        /// Raw event IO manager
        /// </summary>
        private readonly IRawEventIo rawEventIo;

        /// <summary>
        /// The instance of the logger.
        /// </summary>
        private readonly IJHcLogger logger;

        /// <summary>
        /// Results table
        /// </summary>
        private IEventResults resultsTable;

        /// <summary>
        /// Summary of the current event.
        /// </summary>
        private ISummary summary;

        /// <summary>
        /// The name of the current season.
        /// </summary>
        private string seasonName;

        /// <summary>
        /// Initialises a new instance of the <see cref="EventHC"/> class
        /// </summary>
        /// <param name="eventData">event data</param>
        /// <param name="summaryData">summary data</param>
        /// <param name="resultsTableReader">results table reader</param>
        /// <param name="rawEventIo">raw events IO manager</param>
        /// <param name="logger">application logger</param>
        public EventHC(
            IEventData eventData,
            ISummaryData summaryData,
            IResultsTableReader resultsTableReader,
            IRawEventIo rawEventIo,
            IJHcLogger logger)
        {
            this.eventData = eventData;
            this.summaryData = summaryData;
            this.resultsTableReader = resultsTableReader;
            this.rawEventIo = rawEventIo;
            this.logger = logger;

            this.Name = string.Empty;
            this.seasonName = string.Empty;
            this.Date = new DateType();
            this.summary = new Summary();
            this.resultsTable = new EventResults();

            CommonMessenger.Default.Register<LoadNewSeasonMessage>(this, this.NewSeasonSelected);
            CommonMessenger.Default.Register<LoadNewEventMessage>(this, this.LoadNewEvent);
        }

        /// <summary>
        /// Event which is used to inform interested parties that there has been a change to the
        /// results.
        /// </summary>
        public event EventHandler ResultsChangedEvent;

        /// <summary>
        /// Event which is used to inform interested parties that there has been a change to this
        /// event's summary.
        /// </summary>
        public event EventHandler SummaryChangedEvent;

        /// <summary>
        /// Gets the name of this event.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the date of this event.
        /// </summary>
        public DateType Date { get; private set; }

        /// <summary>
        /// Gets the summary of this event.
        /// </summary>
        public ISummary Summary
        {
            get
            {
                return this.summary;
            }

            private set
            {
                if (this.summary != value)
                {
                    this.summary = value;
                    this.SummaryChangedEvent?.Invoke(this, new EventArgs());
                }
            }
        }

        /// <summary>
        /// Gets the results table.
        /// </summary>
        public IEventResults ResultsTable
        {
            get { return this.resultsTable; 
            }

            private set
            {
                if (this.resultsTable != value)
                {
                    this.resultsTable = value;
                    this.ResultsChangedEvent?.Invoke(this, new EventArgs());
                }
            }
        }

        /// <summary>
        /// Loads a  new event into the model.
        /// </summary>
        /// <param name="message">load new event message</param>
        private void LoadNewEvent(
            LoadNewEventMessage message)
        {
            bool success = true;
            this.Name = message.Name;

            try
            {
                if (string.IsNullOrEmpty(this.Name))
                {
                    this.ResultsTable = new EventResults();
                    this.Summary = new Summary();
                    this.Date = new DateType();
                }
                else
                {
                    EventMiscData misc =
                        this.eventData.LoadEventData(
                            this.seasonName,
                            this.Name);
                    this.Date = misc.EventDate;

                    this.ResultsTable =
                        this.resultsTableReader.LoadResultsTable(
                            this.seasonName,
                            this.Name,
                            misc.EventDate);
                    
                    this.Summary =
                        this.summaryData.LoadSummaryData(
                            this.seasonName,
                            this.Name);
                }
            }
            catch (Exception ex)
            {
                this.logger.WriteLog($"Failed to load new event ({this.Name}): {ex}");
            }

            if (success)
            {
                CommonMessenger.Default.Send(
                 new HandicapProgressMessage(
                     $"New Event Loaded - {this.Name}"));

                this.logger.WriteLog($"Event, loaded {this.Name}");
            }
        }

        /// <summary>
        /// Save the raw results.
        /// </summary>
        /// <param name="results">raw results to save</param>
        public bool SaveRawResults(List<IRaw> results)
        {
            bool suceess =
                this.rawEventIo.SaveRawEventData(
                    this.seasonName,
                    this.Name,
                    results);

            return suceess;
        }

        /// <summary>
        /// Load the raw results.
        /// </summary>
        /// <returns>raw results</returns>
        public List<IRaw> LoadRawResults()
        {
            List<IRaw> results =
                this.rawEventIo.LoadRawEventData(
                this.seasonName,
                this.Name);

            return results;
        }

        /// <summary>
        /// Set the results and save them.
        /// </summary>
        /// <param name="results">results to save</param>
        public void SetResultsTable(IEventResults results)
        {
            this.ResultsTable = results;
        }

        /// <summary>
        /// Save the current results.
        /// </summary>
        public void SaveResultsTable()
        {
            this.resultsTableReader.SaveResultsTable(
                this.seasonName,
                this.Name,
                this.ResultsTable.Entries);

        }

        /// <summary>
        /// Save the current event summary data.
        /// </summary>
        public void SaveEventSummary()
        {
            this.summaryData.SaveSummaryData(
                this.seasonName, 
                this.Name, 
                this.Summary);
        }

        /// <summary>
        /// Save the name of the currently selected season
        /// </summary>
        /// <param name="message">the load new season message</param>
        private void NewSeasonSelected(LoadNewSeasonMessage message)
        {
            this.seasonName = message.Name;
        }
    }
}