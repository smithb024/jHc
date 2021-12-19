namespace HandicapModel.Admin.Manage
{
    using System;
    using System.IO;
    using CommonHandicapLib.Interfaces;
    using CommonHandicapLib.Interfaces.XML;
    using CommonHandicapLib.Messages;
    using CommonHandicapLib.Types;
    using GalaSoft.MvvmLight.Messaging;
    using HandicapModel.Interfaces.Admin.IO;

    /// <summary>
    /// Normalisation configuration manager.
    /// </summary>
    public class NormalisationConfigMngr : INormalisationConfigMngr
    {
        /// <summary>
        /// The general IO manager.
        /// </summary>
        private IGeneralIo generalIo;

        /// <summary>
        /// The application logger
        /// </summary>
        private IJHcLogger logger;

        /// <summary>
        /// The normalisation configuration reader
        /// </summary>
        private INormalisationConfigReader reader;

        /// <summary>
        /// Initialises a new instance of the <see cref="NormalisationConfigMngr"/> class
        /// </summary>
        /// <param name="generalIo">general IO manager</param>
        /// <param name="reader">normalisation config reader</param>
        /// <param name="logger">application logger</param>
        public NormalisationConfigMngr(
            IGeneralIo generalIo,
            INormalisationConfigReader reader,
            IJHcLogger logger)
        {
            this.generalIo = generalIo;
            this.reader = reader;
            this.logger = logger;
        }

        /// <summary>
        /// Creates and saves a default results configuration file.
        /// </summary>
        public void SaveDefaultNormalisationConfiguration()
        {
            try
            {
                if (!File.Exists(this.generalIo.NormalisationConfigurationFile))
                {
                    this.reader.SaveNormalisationConfigData(
                      this.generalIo.NormalisationConfigurationFile,
                      new NormalisationConfigType(
                        true,
                        20,
                        5,
                        30));
                    this.logger.WriteLog("Couldn't find normalisation config file. Created a new default one");
                }
            }
            catch (Exception ex)
            {
                this.logger.WriteLog("Failed to save default normalisation config file: " + ex.ToString());
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
        public void SaveNormalisationConfiguration(
          bool useHandicap,
          int handicapTime,
          int minimumHandicap,
          int handicapInterval)
        {
            try
            {
                this.reader.SaveNormalisationConfigData(
                    this.generalIo.NormalisationConfigurationFile,
                    new NormalisationConfigType(
                        useHandicap,
                        handicapTime,
                        minimumHandicap,
                        handicapInterval));
            }
            catch (Exception ex)
            {
                this.logger.WriteLog("Failed to save normalisation config file: " + ex.ToString());
                Messenger.Default.Send(
                    new HandicapErrorMessage(
                        "Error saving normalisation config file"));
            }
        }

        /// <summary>
        /// Creates and saves a default normalisation configuration file.
        /// </summary>
        /// <param name="normalisationConfig">normalisation configuration details</param>
        public void SaveResultsConfiguration(
            NormalisationConfigType normalisationConfig)
        {
            try
            {
                this.reader.SaveNormalisationConfigData(
                    this.generalIo.ResultsConfigurationFile,
                    normalisationConfig);
            }
            catch (Exception ex)
            {
                this.logger.WriteLog("Failed to save normalisation config file: " + ex.ToString());
                Messenger.Default.Send(
                    new HandicapErrorMessage(
                        "Error creating normalisation config file"));
            }
        }

        /// <summary>
        /// Reads the normalisation configuration details
        /// </summary>
        /// <returns>All normalisation configuration details</returns>
        public NormalisationConfigType ReadNormalisationConfiguration()
        {
            return this.reader.LoadNormalisationConfigData(
                this.generalIo.NormalisationConfigurationFile);
        }

        /// <summary>
        /// Reads the normalisation configuration details
        /// </summary>
        /// <returns>use handicap flag</returns>
        public bool ReadUseHandicap()
        {
            return this.reader.LoadNormalisationConfigData(
                this.generalIo.NormalisationConfigurationFile).UseCalculatedHandicap;
        }

        /// <summary>
        /// Reads the normalisation configuration details
        /// </summary>
        /// <returns>handicap time</returns>
        public int ReadHandicapTime()
        {
            return this.reader.LoadNormalisationConfigData(
                this.generalIo.NormalisationConfigurationFile).HandicapTime;
        }

        /// <summary>
        /// Reads the normalisation configuration details
        /// </summary>
        /// <returns>minimum handicap time</returns>
        public int ReadMinimumHandicapTime()
        {
            return this.reader.LoadNormalisationConfigData(
                this.generalIo.NormalisationConfigurationFile).MinimumHandicap;
        }

        /// <summary>
        /// Reads the normalisation configuration details
        /// </summary>
        /// <returns>handicap interval</returns>
        public int ReadHandicapInterval()
        {
            return this.reader.LoadNormalisationConfigData(
                this.generalIo.NormalisationConfigurationFile).HandicapInterval;
        }
    }
}