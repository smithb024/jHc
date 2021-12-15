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
    /// Normalisation configuration manager.
    /// </summary>
    public static class NormalisationConfigMngr
    {
        /// <summary>
        /// Creates and saves a default results configuration file.
        /// </summary>
        public static void SaveDefaultNormalisationConfiguration()
        {
            try
            {
                if (!File.Exists(GeneralIO.NormalisationConfigurationFile))
                {
                    NormalisationConfigReader.SaveNormalisationConfigData(
                      GeneralIO.NormalisationConfigurationFile,
                      new NormalisationConfigType(
                        true,
                        20,
                        5,
                        30));
                    JHcLogger.Instance.WriteLog("Couldn't find normalisation config file. Created a new default one");
                }
            }
            catch (Exception ex)
            {
                JHcLogger.Instance.WriteLog("Failed to save default normalisation config file: " + ex.ToString());
                Messenger.Default.Send(
                    new HandicapErrorMessage(
                        "Error creating default normalisation config file"));
            }
        }

        /// <summary>
        /// Creates and saves a default results configuration file.
        /// </summary>
        /// <param name="useHandicap">use handicap times</param>
        /// <param name="handicapTime">time the handicap is measured against</param>
        /// <param name="minimumHandicap">minimum handicap time</param>
        /// <param name="handicapInterval">interval between handicaps</param>
        public static void SaveNormalisationConfiguration(
          bool useHandicap,
          int handicapTime,
          int minimumHandicap,
          int handicapInterval)
        {
            try
            {
                NormalisationConfigReader.SaveNormalisationConfigData(GeneralIO.NormalisationConfigurationFile,
                                                                      new NormalisationConfigType(useHandicap,
                                                                                                  handicapTime,
                                                                                                  minimumHandicap,
                                                                                                  handicapInterval));
            }
            catch (Exception ex)
            {
                JHcLogger.Instance.WriteLog("Failed to save normalisation config file: " + ex.ToString());
                Messenger.Default.Send(
                    new HandicapErrorMessage(
                        "Error saving normalisation config file"));
            }
        }

        /// <summary>
        /// Creates and saves a default normalisation configuration file.
        /// </summary>
        /// <param name="normalisationConfig">normalisation configuration details</param>
        public static void SaveResultsConfiguration(NormalisationConfigType normalisationConfig)
        {
            try
            {
                NormalisationConfigReader.SaveNormalisationConfigData(GeneralIO.ResultsConfigurationFile,
                                                                      normalisationConfig);
            }
            catch (Exception ex)
            {
                JHcLogger.Instance.WriteLog("Failed to save normalisation config file: " + ex.ToString());
                Messenger.Default.Send(
                    new HandicapErrorMessage(
                        "Error creating normalisation config file"));
            }
        }

        /// <summary>
        /// Reads the normalisation configuration details
        /// </summary>
        /// <returns>All normalisation configuration details</returns>
        public static NormalisationConfigType ReadNormalisationConfiguration()
        {
            return NormalisationConfigReader.LoadNormalisationConfigData(GeneralIO.NormalisationConfigurationFile);
        }

        /// <summary>
        /// Reads the normalisation configuration details
        /// </summary>
        /// <returns>use handicap flag</returns>
        public static bool ReadUseHandicap()
        {
            return NormalisationConfigReader.LoadNormalisationConfigData(GeneralIO.NormalisationConfigurationFile).UseCalculatedHandicap;
        }

        /// <summary>
        /// Reads the normalisation configuration details
        /// </summary>
        /// <returns>handicap time</returns>
        public static int ReadHandicapTime()
        {
            return NormalisationConfigReader.LoadNormalisationConfigData(GeneralIO.NormalisationConfigurationFile).HandicapTime;
        }

        /// <summary>
        /// Reads the normalisation configuration details
        /// </summary>
        /// <returns>minimum handicap time</returns>
        public static int ReadMinimumHandicapTime()
        {
            return NormalisationConfigReader.LoadNormalisationConfigData(GeneralIO.NormalisationConfigurationFile).MinimumHandicap;
        }

        /// <summary>
        /// Reads the normalisation configuration details
        /// </summary>
        /// <returns>handicap interval</returns>
        public static int ReadHandicapInterval()
        {
            return NormalisationConfigReader.LoadNormalisationConfigData(GeneralIO.NormalisationConfigurationFile).HandicapInterval;
        }
    }
}