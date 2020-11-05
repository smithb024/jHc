namespace HandicapModel.Admin.IO.TXT
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using CommonHandicapLib;
    using CommonHandicapLib.Messages;
    using GalaSoft.MvvmLight.Messaging;

    public static class SeasonIO
    {
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>GetSeasons</name>
        /// <date>21/03/15</date>
        /// <summary>
        /// Returns a list of all seasons.
        /// </summary>
        /// <returns>list of all seasons</returns>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        public static List<string> GetSeasons()
        {
            List<string> seasons = new List<string>();

            try
            {
                string[] seasonsArray = Directory.GetDirectories(RootPath.DataPath);
                foreach (string season in seasonsArray)
                {
                    seasons.Add(season.Substring(season.LastIndexOf('\\') + 1));
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.WriteLog("Can't read seasons data: " + ex.ToString());

                Messenger.Default.Send(
                    new HandicapErrorMessage(
                        "Can't read Seasons"));
            }

            return seasons;
        }

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>LoadCurrentSeason</name>
        /// <date>21/03/15</date>
        /// <summary>
        /// Gets the name of the last selected season.
        /// </summary>
        /// <returns>current season</returns>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        public static string LoadCurrentSeason()
        {
            try
            {
                if (File.Exists(RootPath.DataPath + IOPaths.currentSeason))
                {
                    using (StreamReader reader = new StreamReader(RootPath.DataPath + IOPaths.currentSeason))
                    {
                        return reader.ReadLine();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger logger = Logger.GetInstance();
                logger.WriteLog("Error, failed to read current season: " + ex.ToString());
            }

            return string.Empty;
        }

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>SaveCurrentSeason</name>
        /// <date>21/03/15</date>
        /// <summary>
        /// Saves the current season.
        /// </summary>
        /// <param name="season">current season</param>
        /// <returns>success flag</returns>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        public static bool SaveCurrentSeason(string season)
        {
            bool success = false;

            try
            {
                using (StreamWriter writer = new StreamWriter(RootPath.DataPath + IOPaths.currentSeason, false))
                {
                    writer.Write(season);
                    success = true;
                }
            }
            catch (Exception ex)
            {
                Logger logger = Logger.GetInstance();
                logger.WriteLog("Error, failed to save current season: " + ex.ToString());
            }

            return success;
        }

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>CreateNewSeason</name>
        /// <date>21/03/15</date>
        /// <summary>
        /// Creates a directory for a new season
        /// </summary>
        /// <param name="seasonName">new season</param>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        public static bool CreateNewSeason(string seasonName)
        {
            try
            {
                Directory.CreateDirectory(RootPath.DataPath + seasonName);
            }
            catch (Exception ex)
            {
                Logger logger = Logger.GetInstance();
                logger.WriteLog("Failed to create season directory: " + ex.ToString());

                return false;
            }

            return true;
        }
    }
}