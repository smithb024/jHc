namespace CommonHandicapLib.Interfaces.XML
{
    using CommonHandicapLib.Types;

    /// <summary>
    /// The series configuration reader.
    /// </summary>
    public interface ISeriesConfigReader
    {
        /// <summary>
        /// Save the configuration data.
        /// </summary>
        /// <param name="fileName">file name</param>
        /// <param name="configData">configuration data</param>
        /// <returns>success flag</returns>
        bool SaveSeriesConfigData(
          string fileName,
          SeriesConfigType configData);

        /// <summary>
        /// Gets the series configuration data
        /// </summary>
        /// <param name="fileName">file name</param>
        /// <returns>configuration data</returns>
        SeriesConfigType LoadSeriesConfigData(string fileName);
    }
}