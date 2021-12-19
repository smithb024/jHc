namespace HandicapModel.Admin.IO.TXT
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using CommonHandicapLib.Interfaces;
    using CommonHandicapLib.Messages;
    using Interfaces.Admin.IO.TXT;
    using GalaSoft.MvvmLight.Messaging;

    /// <summary>
    /// Class responsible to managing all the season specific data.
    /// </summary>
    public class SeasonIO : ISeasonIO
    {
        /// <summary>
        /// The instance of the logger.
        /// </summary>
        private readonly IJHcLogger logger;

        /// <summary>
        /// The root directory.
        /// </summary>
        string rootDirectory;

        /// <summary>
        /// The path to all the season data.
        /// </summary>
        string dataPath;

        /// <summary>
        /// Initialises a new instance of the <see cref="SeasonIO"/> class.
        /// </summary>
        /// <param name="logger">application logger</param>
        public SeasonIO(
            IJHcLogger logger)
        {
            this.logger = logger;
            this.rootDirectory = RootIO.LoadRootFile();
            this.dataPath = $"{this.rootDirectory}{Path.DirectorySeparatorChar}{IOPaths.dataPath}";
        }

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>GetSeasons</name>
        /// <date>21/03/15</date>
        /// <summary>
        /// Returns a list of all seasons.
        /// </summary>
        /// <returns>list of all seasons</returns>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        public List<string> GetSeasons()
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
                this.logger.WriteLog("Can't read seasons data: " + ex.ToString());

                Messenger.Default.Send(
                    new HandicapErrorMessage(
                        "Can't read Seasons"));
            }

            return seasons;
        }

        /// <summary>
        /// Gets the name of the last selected season.
        /// </summary>
        /// <returns>current season</returns>
        public string LoadCurrentSeason()
        {
            string seasonFilePath = 
                $"{this.dataPath}{Path.DirectorySeparatorChar}{IOPaths.currentSeason}";
            string currentSeason = string.Empty;

            try
            {
               
                if (File.Exists(seasonFilePath))
                {
                    using (StreamReader reader = new StreamReader(seasonFilePath))
                    {
                        currentSeason = reader.ReadLine();
                    }
                }
                else
                {
                    this.logger.WriteLog(
                        $"Error, file - {seasonFilePath} - does not exist.");
                }
            }
            catch (Exception ex)
            {
                this.logger.WriteLog("Error, failed to read current season: " + ex.ToString());
            }

            return currentSeason;
        }

        /// <summary>
        /// Saves the current season.
        /// </summary>
        /// <param name="season">current season</param>
        /// <returns>success flag</returns>
        public bool SaveCurrentSeason(string season)
        {
            string seasonFilePath =
                $"{this.dataPath}{Path.DirectorySeparatorChar}{IOPaths.currentSeason}";
            bool success = false;

            try
            {
                using (StreamWriter writer = new StreamWriter(seasonFilePath, false))
                {
                    writer.Write(season);
                    success = true;
                }
            }
            catch (Exception ex)
            {
                this.logger.WriteLog("Error, failed to save current season: " + ex.ToString());
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
        public bool CreateNewSeason(string seasonName)
        {
            string seasonFilePath =
                $"{this.dataPath}{Path.DirectorySeparatorChar}{seasonName}";

            try
            {
                Directory.CreateDirectory(seasonFilePath);
            }
            catch (Exception ex)
            {
                this.logger.WriteLog("Failed to create season directory: " + ex.ToString());

                return false;
            }

            return true;
        }
    }
}