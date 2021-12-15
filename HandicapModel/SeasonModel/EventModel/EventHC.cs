namespace HandicapModel.SeasonModel.EventModel
{
    using System;
    using System.Collections.Generic;
    using CommonHandicapLib;
    using CommonHandicapLib.Types;
    using CommonLib.Types;
    using HandicapModel.Admin.IO;
    using HandicapModel.Admin.IO.TXT;
    using HandicapModel.Admin.IO.XML;
    using HandicapModel.Common;
    using HandicapModel.Interfaces.Common;
    using HandicapModel.Interfaces.SeasonModel.EventModel;

    /// <summary>
    /// A single event.
    /// </summary>
    public class EventHC : IHandicapEvent
    {
        /// <summary>
        /// Results table
        /// </summary>
        private IEventResults resultsTable;

        /// <summary>
        /// Creates a new instance of the Handicap Event class.
        /// </summary>
        public EventHC()
        {
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
                EventMiscData misc = EventData.LoadEventData(seasonName, eventName);
                this.Date = misc.EventDate;

                ResultsTable = ResultsTableReader.LoadResultsTable(seasonName, eventName, misc.EventDate);

                ISummary summary =
                    SummaryData.LoadSummaryData(
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
                JHcLogger.Instance.WriteLog($"Failed to load new event: {ex}");
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
                RawEventIO.SaveRawEventData(
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
                RawEventIO.LoadRawEventData(
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
            ResultsTableReader.SaveResultsTable(
                this.SeasonName,
                this.Name,
                this.ResultsTable.Entries);

        }

        /// <summary>
        /// Save the current event summary data.
        /// </summary>
        public void SaveEventSummary()
        {
            SummaryData.SaveSummaryData(
                this.SeasonName, 
                this.Name, 
                this.Summary);
        }
    }
}