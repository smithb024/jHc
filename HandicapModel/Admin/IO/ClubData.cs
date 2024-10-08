﻿namespace HandicapModel.Admin.IO
{
    using System.Collections.Generic;
    using System.IO;
    using CommonHandicapLib.Interfaces;
    using CommonHandicapLib.Messages;
    using HandicapModel.Admin.IO.TXT;
    using HandicapModel.Admin.IO.XML;
    using HandicapModel.ClubsModel;
    using HandicapModel.Interfaces.Admin.IO;
    using HandicapModel.Interfaces.Admin.IO.XML;
    using HandicapModel.Interfaces.SeasonModel;
    using CommonMessenger = NynaeveLib.Messenger.Messenger;

    /// <summary>
    /// Club data
    /// </summary>
    public class ClubData : IClubData
    {
        /// <summary>
        /// The instance of the logger.
        /// </summary>
        private readonly IJHcLogger logger;

        /// <summary>
        /// The general IO manager.
        /// </summary>
        private readonly IGeneralIo generalIo;

        /// <summary>
        /// The club data reader.
        /// </summary>
        private IClubDataReader clubDataReader;

        /// <summary>
        /// The club season data reader.
        /// </summary>
        private IClubSeasonDataReader clubSeasonDataReader;

        /// <summary>
        /// The root directory.
        /// </summary>
        private string rootDirectory;

        /// <summary>
        /// The path to all the season data.
        /// </summary>
        private string dataPath;

        /// <summary>
        /// Initialises a new instance of the <see cref="ClubData"/> class.
        /// </summary>
        /// <param name="generalIo">general IO manager</param>
        /// <param name="logger">application logger</param>
        public ClubData(
            IGeneralIo generalIo,
            IJHcLogger logger)
        {
            this.logger = logger;
            this.generalIo = generalIo;

            this.rootDirectory = RootIO.LoadRootFile();
            this.dataPath = $"{this.rootDirectory}{Path.DirectorySeparatorChar}{IOPaths.dataPath}{Path.DirectorySeparatorChar}";

            CommonMessenger.Default.Register<ReinitialiseRoot>(this, this.ReinitialiseRoot);

            this.clubDataReader =
                new ClubDataReader(
                    this.logger);
            this.clubSeasonDataReader =
                new ClubSeasonDataReader(
                    this.logger);
        }

        /// <summary>
        /// Saves the club list
        /// </summary>
        /// <param name="fileName">file name</param>
        /// <param name="clubList">list of clubs</param>
        public bool SaveClubData(Clubs clubList)
        {
            return this.clubDataReader.SaveClubData(
                this.generalIo.ClubGlobalDataFile, 
                clubList);
        }

        /// <summary>
        /// Loads the club list from the data file and returns it.
        /// </summary>
        /// <param name="fileName">file name</param>
        public Clubs LoadClubData()
        {
            return this.clubDataReader.LoadClubData(
                this.generalIo.ClubGlobalDataFile);
        }

        /// <summary>
        /// Saves the season club details.
        /// </summary>
        /// <param name="seasonName">season name</param>
        /// <param name="seasons">season details to save</param>
        /// <returns>success flag</returns>
        public bool SaveClubSeasonData(
            string seasonName,
            List<IClubSeasonDetails> seasons)
        {
            return this.clubSeasonDataReader.SaveClubSeasonData(
              this.dataPath + seasonName + Path.DirectorySeparatorChar + IOPaths.clubDataFile,
              seasons);
        }

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <summary>
        /// Loads the season club details.
        /// </summary>
        /// <param name="seasonName">season name</param>
        /// <returns>season club details</returns>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        public List<IClubSeasonDetails> LoadClubSeasonData(string seasonName)
        {
            return this.clubSeasonDataReader.LoadClubSeasonData(
              this.dataPath + seasonName + Path.DirectorySeparatorChar + IOPaths.clubDataFile);
        }

        /// <summary>
        /// Reinitialise the data path value from the file.
        /// </summary>
        /// <param name="message">reinitialise message</param>
        private void ReinitialiseRoot(ReinitialiseRoot message)
        {
            this.rootDirectory = RootIO.LoadRootFile();
            this.dataPath = $"{this.rootDirectory}{Path.DirectorySeparatorChar}{IOPaths.dataPath}{Path.DirectorySeparatorChar}";
        }
    }
}