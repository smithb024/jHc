namespace CommonHandicapLib.Interfaces.XML
{
    using Types;

    /// <summary>
    /// The normalisation configuration reader
    /// </summary>
    public interface INormalisationConfigReader
    {
        /// <summary>
        /// Save the normalisation configuration data
        /// </summary>
        /// <param name="fileName">name of the file to save to</param>
        /// <param name="configData">configuration data</param>
        /// <returns></returns>
        bool SaveNormalisationConfigData(
            string fileName,
            NormalisationConfigType configData);

        /// <summary>
        /// Gets the normalisation configuration data
        /// </summary>
        /// <param name="fileName">file name</param>
        /// <returns>configuration data</returns>
        NormalisationConfigType LoadNormalisationConfigData(string fileName);
    }
}