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
    /// Series configuration manager.
    /// </summary>
    public class SeriesConfigMngr : ISeriesConfigMngr
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
        private ISeriesConfigReader reader;

        /// <summary>
        /// Initialises a new instance of the <see cref="SeriesConfigMngr"/> class
        /// </summary>
        /// <param name="generalIo">general IO manager</param>
        /// <param name="reader">series config reader</param>
        /// <param name="logger">application logger</param>
        public SeriesConfigMngr(
            IGeneralIo generalIo,
            ISeriesConfigReader reader,
            IJHcLogger logger)
        {
            this.generalIo = generalIo;
            this.reader = reader;
            this.logger = logger;
        }

        /// <summary>
        /// Creates and saves a default series configuration file.
        /// </summary>
        public void SaveDefaultSeriesConfiguration()
        {
            try
            {
                if (!File.Exists(this.generalIo.SeriesConfigurationFile))
                {
                    this.reader.SaveSeriesConfigData(
                        this.generalIo.SeriesConfigurationFile,
                        new SeriesConfigType(string.Empty, false));
                    this.logger.WriteLog("Couldn't find Series config file. Created a new default one");
                }
            }
            catch (Exception ex)
            {
                this.logger.WriteLog("Failed to save default series config file: " + ex.ToString());
                Messenger.Default.Send(
                    new HandicapErrorMessage(
                        "Error creating default series config file"));
            }
        }

        /// <summary>
        /// CSaves a series configuration file.
        /// </summary>
        /// <param name="numberPrefix">number prefix</param>
        public void SaveNormalisationConfiguration(
          string numberPrefix,
          bool allPostions)
        {
            try
            {
                this.reader.SaveSeriesConfigData(
                  this.generalIo.SeriesConfigurationFile,
                  new SeriesConfigType(
                    numberPrefix,
                    allPostions));
            }
            catch (Exception ex)
            {
                this.logger.WriteLog("Failed to save series config file: " + ex.ToString());
                Messenger.Default.Send(
                    new HandicapErrorMessage(
                        "Error creating series config file"));
            }
        }

        /// <summary>
        /// Reads the series configuration details
        /// </summary>
        /// <returns>All series configuration details</returns>
        public SeriesConfigType ReadSeriesConfiguration()
        {
            return this.reader.LoadSeriesConfigData(
              this.generalIo.SeriesConfigurationFile);
        }

        /// <summary>
        /// Reads the series configuration details
        /// </summary>
        /// <returns>number prefix</returns>
        public string ReadNumberPrefix()
        {
            return this.reader.LoadSeriesConfigData(
                this.generalIo.SeriesConfigurationFile)?.NumberPrefix ?? string.Empty;
        }
    }
}