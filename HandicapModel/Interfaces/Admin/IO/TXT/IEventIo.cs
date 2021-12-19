namespace HandicapModel.Interfaces.Admin.IO.TXT
{
    using System.Collections.Generic;

    /// <summary>
    /// Event specific IO.
    /// </summary>
    public interface IEventIo
    {
        /// <summary>
        /// Returns a list of all events.
        /// </summary>
        /// <param name="season">event's season</param>
        /// <returns>list of all events in the season</returns>
        List<string> GetEvents(string season);

        /// <summary>
        /// Creates a directory for a new event
        /// </summary>
        /// <param name="eventName">new event</param>
        /// <param name="seasonName">event's season</param>
        /// <returns>success flag</returns>
        bool CreateNewEvent(
            string seasonName,
            string eventName);

        /// <summary>
        /// Loads the current event name.
        /// </summary>
        /// <returns>current event</returns>
        string LoadCurrentEvent(string seasonName);

        /// <summary>
        /// Saves the current event name.
        /// </summary>
        /// <param name="season">current event</param>
        /// <returns>success flag</returns>
        bool SaveCurrentEvent(
            string seasonName,
            string eventName);
    }
}
