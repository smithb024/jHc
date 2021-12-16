namespace HandicapModel.SeasonModel.EventModel
{
    using System;
    using System.Collections.Generic;
    using CommonHandicapLib.Interfaces;
    using CommonHandicapLib.Types;
    using CommonLib.Types;
    using HandicapModel.Common;
    using HandicapModel.Interfaces.Admin.IO;
    using HandicapModel.Interfaces.Admin.IO.TXT;
    using HandicapModel.Interfaces.Admin.IO.XML;
    using HandicapModel.Interfaces.Common;
    using HandicapModel.Interfaces.SeasonModel.EventModel;

    /// <summary>
    /// A single event.
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
            this.SeasonName = string.Empty;
            this.Date = new DateType();
            this.Summary = new Summary();
            this.resultsTable = new EventResults();
        }

        /// <summary>
        /// Event which is used to inform interested parties that there has been a change to the
        /// results.
        /// </summary>
        public event EventHandler ResultsChangedEvent;

        /// <summary>
        /// Gets the name of this event.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the name of this event's parent season.
        /// </summary>
        public string SeasonName { get; private set; }

        /// <summary>
        /// Gets the date of this event.
        /// </summary>
        public DateType Date { get; private set; }

        /// <summary>
        /// Gets the summary of this event.
        /// </summary>
        public ISummary Summary { get; }

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
        /// Loads the given event.
        /// </summary>
        /// <param name="seasonName">season name</param>
        /// <param name="eventName">new event name</param>
        /// <returns>success flag</returns>
        public bool LoadNewEvent(
            string seasonName,
            string eventName)
        {
            bool success = true;

            try
            {
                EventMiscData misc = this.eventData.LoadEventData(seasonName, eventName);
                this.Date = misc.EventDate;

                ResultsTable = 
                    this.resultsTableReader.LoadResultsTable(
                        seasonName, 
                        eventName, 
                        misc.EventDate);

                ISummary summary =
                    this.summaryData.LoadSummaryData(
                        seasonName, 
                        eventName);

                this.Summary.UpdateSummary(
                    summary.MaleRunners,
                    summary.FemaleRunners,
                    summary.SBs,
                    summary.PBs,
                    summary.FirstTimers);
                this.Summary.LoadFastest(
                    summary.FastestBoys,
                    summary.FastestGirls);
            }
            catch (Exception ex)
            {
                this.logger.WriteLog($"Failed to load new event: {ex}");
            }

            this.Name = eventName;
            this.SeasonName = seasonName;

            return success;
        }

        /// <summary>
        /// Save the raw results.
        /// </summary>
        /// <param name="results">raw results to save</param>
        public bool SaveRawResults(List<IRaw> results)
        {
            bool suceess =
                this.rawEventIo.SaveRawEventData(
                    this.SeasonName,
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
                this.SeasonName, 
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
                this.SeasonName,
                this.Name,
                this.ResultsTable.Entries);

        }

        /// <summary>
        /// Save the current event summary data.
        /// </summary>
        public void SaveEventSummary()
        {
            this.summaryData.SaveSummaryData(
                this.SeasonName, 
                this.Name, 
                this.Summary);
        }
    }
}