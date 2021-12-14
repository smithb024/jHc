namespace HandicapModel.Admin.Manage
{
    using System.Collections.Generic;
    using System.IO;

    using CommonHandicapLib;
    using CommonHandicapLib.Types;

    using CommonLib.Types;

    using Event;

    using HandicapModel.Admin.IO;
    using HandicapModel.Admin.IO.ResultsCSV;
    using HandicapModel.Admin.IO.TXT;

    using HandicapModel.Common;
    using HandicapModel.SeasonModel;
    using HandicapModel.SeasonModel.EventModel;

    public delegate void SummaryDelegate(Summary summary);

    public delegate void SeasonsDelegate(List<string> seasonNames);
    public delegate void EventsDelegate(List<string> eventNames);

    public delegate void AthletesDelegate();
    public delegate void ClubsDelegate();

    public delegate void ResultsDelegate(EventResults results);
    public delegate void AthleteSeasonDelegate(List<AthleteSeasonDetails> athletes);
    public delegate void ClubSeasonDelegate(List<ClubSeasonDetails> clubs);

    public delegate void StringDelegate(string myString);
    public delegate void DateDelegate(DateType date);
    public delegate void TimeDelegate(TimeType time);

    public delegate void InformationDelegate(string information);

    /// <summary>
    /// Interface which describes the Business Layer Manager
    /// </summary>
    public interface IBLMngr
    {
        /// <summary>
        /// Gets the root directory of the model.
        /// </summary>
        string ModelRootDirectory { get; }

        /// <summary>
        /// Initialise the model 
        /// </summary>
        void InitialiseModel();

        /// <date>21/03/15</date>
        /// <summary>
        /// Creates a directory for a new season
        /// </summary>
        /// <param name="seasonName">new season</param>
        /// <returns>success flag</returns>
        bool CreateNewSeason(string seasonName);

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>LoadNewSeason</name>
        /// <date>21/03/15</date>
        /// <summary>
        /// Loads a new season into memory.
        /// </summary>
        /// <param name="seasonName">season to load</param>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        void LoadNewSeason(string seasonName);

        /// <summary>
        /// Create a new event
        /// </summary>
        /// <param name="eventName">event name</param>
        /// <param name="date">date of the event</param>
        /// <returns>success flag</returns>
        bool CreateNewEvent(string eventName, DateType date);

        /// <summary>
        /// Load a new event
        /// </summary>
        /// <param name="eventName">event name</param>
        void LoadNewEvent(string eventName);

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>LoadCurrentSeason</name>
        /// <date>15/04/15</date>
        /// <summary>
        /// Loads the current season.
        /// </summary>
        /// <remarks>
        /// The current season is stored in a text file to make it persistent from one running instance of the 
        /// application to the next.
        /// </remarks>
        /// <returns>returns the name of the current season</returns>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        string LoadCurrentSeason();

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>LoadCurrentSeason</name>
        /// <date>15/04/15</date>
        /// <summary>
        /// Loads the current season.
        /// </summary>
        /// <remarks>
        /// The current season is stored in a text file to make it persistent from one running instance of the 
        /// application to the next.
        /// </remarks>
        /// <returns>returns the name of the current season</returns>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        string LoadCurrentEvent();

        /// <summary>
        /// Caclulate the results for the current event.
        /// </summary>
        void CalculateResults();

        /// <summary>
        /// Remove the results for the current event.
        /// </summary>
        void DeleteResults();

        /// <summary>
        /// Save the root directory for the root of the model directory.
        /// </summary>
        /// <param name="rootDirectory">root directory to save</param>
        void SaveRootDirectory(string rootDirectory);

        /// <summary>
        /// Set the progress information field in the model.
        /// </summary>
        /// <param name="progress">string to add</param>
        void SetProgressInformation(string progress);

        /// <summary>
        /// Print a full set of information if the folder exists.
        /// </summary>
        /// <param name="folder">folder to save to</param>
        void PrintAll(string folder);
    }
}