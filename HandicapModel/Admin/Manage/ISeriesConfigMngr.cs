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

    public interface ISeriesConfigMngr
    {/// <summary>
     /// Creates and saves a default series configuration file.
     /// </summary>
        void SaveDefaultSeriesConfiguration();

        /// <summary>
        /// Saves a series configuration file.
        /// </summary>
        /// <param name="numberPrefix">number prefix</param>
        void SaveNormalisationConfiguration(
          string numberPrefix,
          bool allPostions);

        /// <summary>
        /// Reads the series configuration details
        /// </summary>
        /// <returns>All series configuration details</returns>
        SeriesConfigType ReadSeriesConfiguration();

        /// <summary>
        /// Reads the series configuration details
        /// </summary>
        /// <returns>number prefix</returns>
        string ReadNumberPrefix();
    }
}