namespace HandicapModel.Interfaces.Admin.IO
{
    /// <summary>
    /// General IO management
    /// </summary>
    public interface IGeneralIo
    {
        /// <summary>
        /// Gets the global athlete's data file
        /// </summary>
        string AthletesGlobalDataFile { get; }

        /// <summary>
        /// Gets the global club's data file
        /// </summary>
        string ClubGlobalDataFile { get; }

        /// <summary>
        /// Gets the results configuration file.
        /// </summary>
        string ResultsConfigurationFile { get; }

        /// <summary>
        /// Gets the normalisation configuration file.
        /// </summary>
        string NormalisationConfigurationFile { get; }

        /// <summary>
        /// Gets the series configuration file.
        /// </summary>
        string SeriesConfigurationFile { get; }

        /// <summary>
        /// Gets a value which indicates whether the data folder exits.
        /// </summary>
        bool DataFolderExists { get; }

        /// <summary>
        /// Gets a value which indicates whether the configuration folder exits.
        /// </summary>
        bool ConfigurationFolderExists { get; }

        /// <summary>
        /// Creates the data folder if one does not exist.
        /// </summary>
        void CreateDataFolder();

        /// <summary>
        /// Creates the data folder if one does not exist.
        /// </summary>
        void CreateConfigurationFolder();
    }
}
