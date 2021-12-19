namespace HandicapModel.Interfaces.Admin.IO.TXT
{
    using System.Collections.Generic;

    /// <summary>
    /// Interface for a class responsible to managing all the season specific data.
    /// </summary>
    public interface ISeasonIO
    {
        /// <summary>
        /// Returns a list of all seasons.
        /// </summary>
        /// <returns>list of all seasons</returns>
        List<string> GetSeasons();

        /// <summary>
        /// Gets the name of the last selected season.
        /// </summary>
        /// <returns>current season</returns>
        string LoadCurrentSeason();

        /// <summary>
        /// Saves the current season.
        /// </summary>
        /// <param name="season">current season</param>
        /// <returns>success flag</returns>
        bool SaveCurrentSeason(string season);
        
        /// <summary>
        /// Creates a directory for a new season
        /// </summary>
        /// <param name="seasonName">new season</param>
        bool CreateNewSeason(string seasonName);
    }
}
