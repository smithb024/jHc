namespace HandicapModel.Interfaces.Admin.IO.TXT
{
    using System.Collections.Generic;
    using HandicapModel.Interfaces.SeasonModel.EventModel;

    /// <summary>
    /// Raw event data specific IO.
    /// </summary>
    public interface IRawEventIo
    {
        /// <summary>
        /// Saves the raw results data for the indicated event
        /// </summary>
        /// <param name="seasonName">season name</param>
        /// <param name="eventName">event name</param>
        /// <param name="rawData">raw data to save</param>
        /// <returns></returns>
        bool SaveRawEventData(
            string seasonName,
            string eventName,
            List<IRaw> rawData);

        /// <summary>
        /// Loads the raw event data for the indicated event.
        /// </summary>
        /// <param name="seasonName">season name</param>
        /// <param name="eventName">event name</param>
        /// <returns>raw event results data</returns>
        List<IRaw> LoadRawEventData(
             string seasonName,
             string eventName);
    }
}