namespace HandicapModel.Interfaces.Admin.IO
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using HandicapModel.Admin.IO.XML;
    using HandicapModel.ClubsModel;
    using HandicapModel.Interfaces.SeasonModel;
    using HandicapModel.SeasonModel;

    /// <summary>
    /// The club data interface
    /// </summary>
    public interface IClubData
    {
        /// <summary>
        /// Saves the club list
        /// </summary>
        /// <param name="fileName">file name</param>
        /// <param name="clubList">list of clubs</param>
        bool SaveClubData(Clubs clubList);

        /// <summary>
        /// Loads the club list from the data file and returns it.
        /// </summary>
        /// <param name="fileName">file name</param>
        Clubs LoadClubData();

        /// <summary>
        /// Saves the season club details.
        /// </summary>
        /// <param name="seasonName">season name</param>
        /// <param name="seasons">season details to save</param>
        /// <returns>success flag</returns>
        bool SaveClubSeasonData(
            string seasonName,
            List<IClubSeasonDetails> seasons);

        /// <summary>
        /// Loads the season club details.
        /// </summary>
        /// <param name="seasonName">season name</param>
        /// <returns>season club details</returns>
        List<IClubSeasonDetails> LoadClubSeasonData(string seasonName);
    }
}
