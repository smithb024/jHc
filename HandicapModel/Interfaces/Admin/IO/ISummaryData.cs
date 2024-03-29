﻿namespace HandicapModel.Interfaces.Admin.IO
{
    using HandicapModel.Interfaces.Common;

    /// <summary>
    /// Summary data interface
    /// </summary>
    public interface ISummaryData
    {
        /// <summary>
        /// Saves the summary details
        /// </summary>
        /// <param name="summaryDetails">details to save</param>
        /// <returns>success flag</returns>
        bool SaveSummaryData(ISummary summaryDetails);

        /// <summary>
        /// Saves the summary details
        /// </summary>
        /// <param name="seasonName">season name</param>
        /// <param name="summaryDetails">details to save</param>
        /// <returns>success flag</returns>
        bool SaveSummaryData(
            string seasonName,
            ISummary summaryDetails);

        /// <summary>
        /// Saves the summary details
        /// </summary>
        /// <param name="seasonName">season name</param>
        /// <param name="eventName">event name</param>
        /// <param name="summaryDetails">details to save</param>
        /// <returns>success flag</returns>
        bool SaveSummaryData(
            string seasonName,
            string eventName,
            ISummary summaryDetails);

        /// <summary>
        /// Reads the summary details.
        /// </summary>
        /// <param name="seasonName">season name</param>
        /// <returns>decoded summary data</returns>
        ISummary LoadSummaryData();

        /// <summary>
        /// Reads the summary details.
        /// </summary>
        /// <param name="seasonName">season name</param>
        /// <returns>decoded summary data</returns>
        ISummary LoadSummaryData(string seasonName);

        /// <summary>
        /// Reads the summary details.
        /// </summary>
        /// <param name="seasonName">season name</param>
        /// <returns>decoded summary data</returns>
        ISummary LoadSummaryData(
            string seasonName,
            string eventName);
    }
}