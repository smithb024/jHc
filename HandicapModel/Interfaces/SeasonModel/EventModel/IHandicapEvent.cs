namespace HandicapModel.Interfaces.SeasonModel.EventModel
{
    using CommonLib.Types;
    using HandicapModel.Common;
    using System;
    using System.Collections.Generic;
    using HandicapModel.Interfaces.Common;

    /// <summary>
    /// Interface for the Event Model, this describes the full set of results for a single event.
    /// </summary>
    public interface IHandicapEvent
    {
        /// <summary>
        /// Event which is used to inform interested parties that there has been a change to the
        /// results.
        /// </summary>
        event EventHandler ResultsChangedEvent;

        /// <summary>
        /// Gets the date of the event.
        /// </summary>
        DateType Date { get; }

        /// <summary>
        /// Gets the event name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the compelted results table.
        /// </summary>
        IEventResults ResultsTable { get; }

        /// <summary>
        /// Gets the event summary information.
        /// </summary>
        ISummary Summary { get; }
        //SummaryDelegate EventSummaryCallback { get; set; }
        //ResultsDelegate ResultsCallback { get; set; }
        
        /// <summary>
        /// Load the raw results.
        /// </summary>
        /// <returns>raw results</returns>
        List<IRaw> LoadRawResults();

        /// <summary>
        /// Save the current event.
        /// </summary>
        void SaveEventSummary();

        /// <summary>
        /// Save the raw results.
        /// </summary>
        /// <param name="results">raw results to save</param>
        /// <returns></returns>
        bool SaveRawResults(List<IRaw> results);

        /// <summary>
        /// Save the results table.
        /// </summary>
        void SaveResultsTable();

        /// <summary>
        /// Set the results.
        /// </summary>
        /// <param name="results">new set of results.</param>
        void SetResultsTable(IEventResults results);

        ///// <summary>
        ///// 
        ///// </summary>
        //void SummaryTableUpdated();
    }
}