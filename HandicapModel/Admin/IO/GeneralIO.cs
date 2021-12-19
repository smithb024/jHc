namespace HandicapModel.Admin.IO
{
    using System.IO;
    using CommonHandicapLib.Interfaces;
    using HandicapModel.Interfaces.Admin.IO;

    /// <summary>
    /// Provides any global io methods.
    /// </summary>
    public class GeneralIO : IGeneralIo
    {
        /// <summary>
        /// The application logger.
        /// </summary>
        private IJHcLogger logger;

        /// <summary>
        /// Initialises a new instance of the <see cref="GeneralIO"/> class.
        /// </summary>
        /// <param name="logger"></param>
        public GeneralIO(
            IJHcLogger logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Gets the global athlete's data file
        /// </summary>
        public string AthletesGlobalDataFile =>
            RootPath.ConfigurationPath + Path.DirectorySeparatorChar + IOPaths.athleteDataFile;

        /// <summary>
        /// Gets the global club's data file
        /// </summary>
        public string ClubGlobalDataFile =>
            RootPath.ConfigurationPath + Path.DirectorySeparatorChar + IOPaths.clubDataFile;

        /// <summary>
        /// Gets the results configuration file.
        /// </summary>
        public string ResultsConfigurationFile =>
            RootPath.ConfigurationPath + Path.DirectorySeparatorChar + IOPaths.resultsConfiguration;

        /// <summary>
        /// Gets the normalisation configuration file.
        /// </summary>
        public string NormalisationConfigurationFile =>
            RootPath.ConfigurationPath + Path.DirectorySeparatorChar + IOPaths.normalisationConfiguration;

        /// <summary>
        /// Gets the series configuration file.
        /// </summary>
        public string SeriesConfigurationFile =>
            RootPath.ConfigurationPath + Path.DirectorySeparatorChar + IOPaths.seriesConfiguration; 

        /// <summary>
        /// Gets a value which indicates whether the data folder exits.
        /// </summary>
        public bool DataFolderExists => Directory.Exists(RootPath.DataPath);

        /// <summary>
        /// Gets a value which indicates whether the configuration folder exits.
        /// </summary>
        public bool ConfigurationFolderExists => Directory.Exists(RootPath.ConfigurationPath);

        /// <summary>
        /// Creates the data folder if one does not exist.
        /// </summary>
        public void CreateDataFolder()
        {
            if (!Directory.Exists(RootPath.DataPath))
            {
                this.logger.WriteLog("Data directory missing - new one created");
                Directory.CreateDirectory(RootPath.DataPath);
            }
        }

        /// <summary>
        /// Creates the data folder if one does not exist.
        /// </summary>
        public void CreateConfigurationFolder()
        {
            if (!Directory.Exists(RootPath.ConfigurationPath))
            {
                this.logger.WriteLog("Configuration directory missing - new one created");
                Directory.CreateDirectory(RootPath.ConfigurationPath);
            }
        }
    }
}