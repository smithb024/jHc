namespace HandicapModel.Admin.IO
{
    using System.IO;
    using CommonHandicapLib.Interfaces;
    using CommonHandicapLib.Messages;
    using HandicapModel.Admin.IO.TXT;
    using HandicapModel.Interfaces.Admin.IO;
    using GalaSoft.MvvmLight.Messaging;

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
        /// The root directory.
        /// </summary>
        private string rootDirectory;

        /// <summary>
        /// The path to all the season data.
        /// </summary>
        private string dataPath;

        /// <summary>
        /// The path to the configuration data
        /// </summary>
        private string configurationPath;

        /// <summary>
        /// Initialises a new instance of the <see cref="GeneralIO"/> class.
        /// </summary>
        /// <param name="logger"></param>
        public GeneralIO(
            IJHcLogger logger)
        {
            this.logger = logger;

            this.rootDirectory = RootIO.LoadRootFile();
            this.dataPath = $"{this.rootDirectory}{Path.DirectorySeparatorChar}{IOPaths.dataPath}{Path.DirectorySeparatorChar}";
            this.configurationPath = $"{this.rootDirectory}{Path.DirectorySeparatorChar}{IOPaths.configurationPath}{Path.DirectorySeparatorChar}";

            Messenger.Default.Register<ReinitialiseRoot>(this, this.ReinitialiseRoot);
        }

        /// <summary>
        /// Gets the global athlete's data file
        /// </summary>
        public string AthletesGlobalDataFile =>
            this.configurationPath + IOPaths.athleteDataFile;

        /// <summary>
        /// Gets the global club's data file
        /// </summary>
        public string ClubGlobalDataFile =>
            this.configurationPath + IOPaths.clubDataFile;

        /// <summary>
        /// Gets the results configuration file.
        /// </summary>
        public string ResultsConfigurationFile =>
            this.configurationPath + IOPaths.resultsConfiguration;

        /// <summary>
        /// Gets the normalisation configuration file.
        /// </summary>
        public string NormalisationConfigurationFile =>
            this.configurationPath + IOPaths.normalisationConfiguration;

        /// <summary>
        /// Gets the series configuration file.
        /// </summary>
        public string SeriesConfigurationFile =>
            this.configurationPath + IOPaths.seriesConfiguration; 

        /// <summary>
        /// Gets a value which indicates whether the data folder exits.
        /// </summary>
        public bool DataFolderExists => Directory.Exists(this.dataPath);

        /// <summary>
        /// Gets a value which indicates whether the configuration folder exits.
        /// </summary>
        public bool ConfigurationFolderExists => Directory.Exists(this.configurationPath);

        /// <summary>
        /// Creates the data folder if one does not exist.
        /// </summary>
        public void CreateDataFolder()
        {
            if (!Directory.Exists(this.dataPath))
            {
                this.logger.WriteLog("Data directory missing - new one created");
                Directory.CreateDirectory(this.dataPath);
            }
        }

        /// <summary>
        /// Creates the data folder if one does not exist.
        /// </summary>
        public void CreateConfigurationFolder()
        {
            if (!Directory.Exists(this.configurationPath))
            {
                this.logger.WriteLog("Configuration directory missing - new one created");
                Directory.CreateDirectory(this.configurationPath);
            }
        }

        /// <summary>
        /// Reinitialise the data path value from the file.
        /// </summary>
        /// <param name="message">reinitialise message</param>
        private void ReinitialiseRoot(ReinitialiseRoot message)
        {
            this.rootDirectory = RootIO.LoadRootFile();
            this.dataPath = $"{this.rootDirectory}{Path.DirectorySeparatorChar}{IOPaths.dataPath}{Path.DirectorySeparatorChar}";
            this.configurationPath = $"{this.rootDirectory}{Path.DirectorySeparatorChar}{IOPaths.configurationPath}{Path.DirectorySeparatorChar}";
        }
    }
}