namespace HandicapModel.Admin.Manage
{
    using CommonHandicapLib.Types;

    public interface INormalisationConfigMngr
    {
        /// <summary>
        /// Creates and saves a default results configuration file.
        /// </summary>
        void SaveDefaultNormalisationConfiguration();

        /// <summary>
        /// Creates and saves a default results configuration file.
        /// </summary>
        /// <param name="useHandicap">use handicap times</param>
        /// <param name="handicapTime">time the handicap is measured against</param>
        /// <param name="minimumHandicap">minimum handicap time</param>
        /// <param name="handicapInterval">interval between handicaps</param>
        void SaveNormalisationConfiguration(
          bool useHandicap,
          int handicapTime,
          int minimumHandicap,
          int handicapInterval);

        /// <summary>
        /// Creates and saves a default normalisation configuration file.
        /// </summary>
        /// <param name="normalisationConfig">normalisation configuration details</param>
        void SaveResultsConfiguration(NormalisationConfigType normalisationConfig);

        /// <summary>
        /// Reads the normalisation configuration details
        /// </summary>
        /// <returns>All normalisation configuration details</returns>
        NormalisationConfigType ReadNormalisationConfiguration();

        /// <summary>
        /// Reads the normalisation configuration details
        /// </summary>
        /// <returns>use handicap flag</returns>
        bool ReadUseHandicap();

        /// <summary>
        /// Reads the normalisation configuration details
        /// </summary>
        /// <returns>handicap time</returns>
        int ReadHandicapTime();

        /// <summary>
        /// Reads the normalisation configuration details
        /// </summary>
        /// <returns>minimum handicap time</returns>
        int ReadMinimumHandicapTime();

        /// <summary>
        /// Reads the normalisation configuration details
        /// </summary>
        /// <returns>handicap interval</returns>
        int ReadHandicapInterval();
    }
}