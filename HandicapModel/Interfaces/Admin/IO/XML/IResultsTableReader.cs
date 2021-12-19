namespace HandicapModel.Interfaces.Admin.IO.XML
{
    using System.Collections.Generic;
    using CommonLib.Types;
    using HandicapModel.Interfaces.SeasonModel.EventModel;

    /// <summary>
    /// The results table reader
    /// </summary>
    public interface IResultsTableReader
    {
        /// <summary>
        /// Save the points table
        /// </summary>
        /// <param name="seasonName">season name</param>
        /// <param name="eventName">event name</param>
        /// <param name="resultsTable">points table</param>
        bool SaveResultsTable(
            string seasonName,
            string eventName,
            List<IResultsTableEntry> resultsTable);

        /// <summary>
        /// Reads the athlete season details xml from file and decodes it.
        /// </summary>
        /// <param name="seasonName">season name</param>
        /// <param name="eventName">event name</param>
        /// <param name="date">event date</param>
        /// <returns>decoded athlete's details</returns>
        IEventResults LoadResultsTable(
            string seasonName,
            string eventName,
            DateType date);
    }
}
