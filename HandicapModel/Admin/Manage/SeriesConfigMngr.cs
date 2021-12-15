namespace HandicapModel.Admin.Manage
{
    using System;
    using System.IO;

    using CommonHandicapLib;
    using CommonHandicapLib.Messages;
    using CommonHandicapLib.Types;
    using CommonHandicapLib.XML;
    using GalaSoft.MvvmLight.Messaging;
    using HandicapModel.Admin.IO;

    /// <summary>
    /// Series configuration manager.
    /// </summary>
    public static class SeriesConfigMngr
    {
        /// <summary>
        /// Creates and saves a default series configuration file.
        /// </summary>
        public static void SaveDefaultSeriesConfiguration()
        {
            try
            {
                if (!File.Exists(GeneralIO.SeriesConfigurationFile))
                {
                    SeriesConfigReader.SaveSeriesConfigData(GeneralIO.SeriesConfigurationFile,
                                                              new SeriesConfigType(string.Empty, false));
                    JHcLogger.Instance.WriteLog("Couldn't find Series config file. Created a new default one");
                }
            }
            catch (Exception ex)
            {
                JHcLogger.Instance.WriteLog("Failed to save default series config file: " + ex.ToString());
                Messenger.Default.Send(
                    new HandicapErrorMessage(
                        "Error creating default series config file"));
            }
        }

        /// <summary>
        /// CSaves a series configuration file.
        /// </summary>
        /// <param name="numberPrefix">number prefix</param>
        public static void SaveNormalisationConfiguration(
          string numberPrefix,
          bool allPostions)
        {
            try
            {
                SeriesConfigReader.SaveSeriesConfigData(
                  GeneralIO.SeriesConfigurationFile,
                  new SeriesConfigType(
                    numberPrefix,
                    allPostions));
            }
            catch (Exception ex)
            {
                JHcLogger.Instance.WriteLog("Failed to save series config file: " + ex.ToString());
                Messenger.Default.Send(
                    new HandicapErrorMessage(
                        "Error creating series config file"));
            }
        }

        /// <summary>
        /// Reads the series configuration details
        /// </summary>
        /// <returns>All series configuration details</returns>
        public static SeriesConfigType ReadSeriesConfiguration()
        {
            return SeriesConfigReader.LoadSeriesConfigData(
              GeneralIO.SeriesConfigurationFile);
        }

        /// <summary>
        /// Reads the series configuration details
        /// </summary>
        /// <returns>number prefix</returns>
        public static string ReadNumberPrefix()
        {
            return SeriesConfigReader.LoadSeriesConfigData(GeneralIO.SeriesConfigurationFile)?.NumberPrefix ?? string.Empty;
        }
    }
}