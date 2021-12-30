namespace HandicapModel.Admin.IO
{
    using System.Collections.Generic;
    using System.IO;
    using CommonHandicapLib.Interfaces;
    using CommonHandicapLib.Messages;
    using HandicapModel.Admin.IO.TXT;
    using HandicapModel.Admin.IO.XML;
    using HandicapModel.Admin.Manage;
    using HandicapModel.AthletesModel;
    using HandicapModel.Interfaces.Admin.IO;
    using HandicapModel.Interfaces.Admin.IO.XML;
    using HandicapModel.SeasonModel;
    using GalaSoft.MvvmLight.Messaging;

    /// <summary>
    /// Athlete data
    /// </summary>
    public class AthleteData : IAthleteData
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
        /// The athlete data reader.
        /// </summary>
        private IAthleteDataReader athleteDataReader;

        /// <summary>
        /// The athlete season data reader.
        /// </summary>
        private IAthleteSeasonDataReader athleteSeasonDataReader;

        /// <summary>
        /// The path to all the season data.
        /// </summary>
        private string dataPath;

        /// <summary>
        /// Initialises a new instance of the <see cref="AthleteData"/> class. 
        /// </summary>
        /// <param name="generalIo">general IO manager</param>
        /// <param name="logger">application logger</param>
        public AthleteData(
            IGeneralIo generalIo,
            INormalisationConfigMngr normalisationConfigManager,
            ISeriesConfigMngr seriesConfigManager,
            IJHcLogger logger)
        {
            this.generalIo = generalIo;
            this.logger = logger;

            this.athleteDataReader =
                new AthleteDataReader(
                    normalisationConfigManager,
                    seriesConfigManager,
                    this.logger);
            this.athleteSeasonDataReader =
                new AthleteSeasonDataReader(
                    this.logger);

            string rootDirectory = RootIO.LoadRootFile();
            this.dataPath = $"{rootDirectory}{Path.DirectorySeparatorChar}{IOPaths.dataPath}{Path.DirectorySeparatorChar}";
            Messenger.Default.Register<ReinitialiseRoot>(this, this.ReinitialiseRoot);
        }

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>SaveAthleteData</name>
        /// <date>22/02/15</date>
        /// <summary>
        /// Saves the athlete details list.
        /// </summary>
        /// <param name="athleteDetailsList">athletes to save</param>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        public bool SaveAthleteData(Athletes athleteDetailsList)
        {
            return this.athleteDataReader.SaveAthleteData(
                this.generalIo.AthletesGlobalDataFile, 
                athleteDetailsList);
        }

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>ReadAthleteData</name>
        /// <date>22/02/15</date>
        /// <summary>
        /// Reads the athlete details.
        /// </summary>
        /// <param name="fileName">name of xml file</param>
        /// <returns>decoded athlete's details</returns>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        public Athletes ReadAthleteData()
        {
            return this.athleteDataReader.ReadAthleteData(
                this.generalIo.AthletesGlobalDataFile);
        }

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>SaveAthleteSeasonData</name>
        /// <date>03/04/15</date>
        /// <summary>
        /// Saves the season athlete details.
        /// </summary>
        /// <param name="seasonName">season name</param>
        /// <param name="seasons">season details to save</param>
        /// <returns>success flag</returns>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        public bool SaveAthleteSeasonData(
            string seasonName,
            List<AthleteSeasonDetails> seasons)
        {
            return this.athleteSeasonDataReader.SaveAthleteSeasonData(
              this.dataPath + seasonName + Path.DirectorySeparatorChar + IOPaths.athleteDataFile,
              seasons);
        }

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>LoadAthleteSeasonData</name>
        /// <date>03/04/15</date>
        /// <summary>
        /// Loads the season athlete details.
        /// </summary>
        /// <param name="seasonName">season name</param>
        /// <returns>season athlete details</returns>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        public List<AthleteSeasonDetails> LoadAthleteSeasonData(
          string seasonName,
          IResultsConfigMngr resultsConfigurationManager)
        {
            return this.athleteSeasonDataReader.LoadAthleteSeasonData(
              this.dataPath + seasonName + Path.DirectorySeparatorChar + IOPaths.athleteDataFile,
              resultsConfigurationManager);
        }

        /// <summary>
        /// A reinitialise root message has been received.
        /// </summary>
        /// <param name="message">reinitialise root message</param>
        public void ReinitialiseRoot(ReinitialiseRoot message)
        {
            string rootDirectory = RootIO.LoadRootFile();
            this.dataPath = $"{rootDirectory}{Path.DirectorySeparatorChar}{IOPaths.dataPath}{Path.DirectorySeparatorChar}";
        }
    }
}