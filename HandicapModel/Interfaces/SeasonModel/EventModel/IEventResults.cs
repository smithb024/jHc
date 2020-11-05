namespace HandicapModel.Interfaces.SeasonModel.EventModel
{
    using System.Collections.Generic;

    /// <summary>
    /// Interface describing a table which contains a full set of completed event results.
    /// </summary>
    public interface IEventResults
    {
        /// <summary>
        /// Gets a list of all result entries.
        /// </summary>
        List<IResultsTableEntry> Entries { get; }

        /// <summary>
        /// Add a new entry to the list.
        /// </summary>
        /// <param name="entry">entry to add</param>
        void AddEntry(IResultsTableEntry entry);

        /// <summary>
        /// Sort by running time, then go through the list applying the speed order.
        /// </summary>
        /// <remarks>
        /// If two athletes have the same time, then they have the same speed order.
        /// </remarks>
        void ApplySpeedOrder();

        /// <summary>
        /// Order the results table by the total time taken.
        /// </summary>
        void OrderByFinishingTime();

        /// <summary>
        /// Order the results tabke by the time spent running.
        /// </summary>
        void OrderByRunningTime();
    }
}