namespace HandicapModel.Admin.IO.ResultsCSV
{
    using System;
    using System.IO;

    using CommonHandicapLib;
    using CommonHandicapLib.Interfaces;
    using CommonHandicapLib.Messages;
    using CommonHandicapLib.Types;
    using GalaSoft.MvvmLight.Messaging;
    using HandicapModel.Admin.Manage;
    using HandicapModel.Interfaces;

    public static class NextRunnerWriter
    {
        /// <summary>
        /// Write the next runner to a file.
        /// </summary>
        /// <param name="model">junior handicap model</param>
        /// <param name="folder">output model</param>
        /// <param name="seriesConfigMngr">series configuraton manager</param>
        /// <param name="logger">application logger</param>
        /// <returns>success flag</returns>
        public static bool WriteNextRunnerTable(
            IModel model,
            string folder,
            ISeriesConfigMngr seriesConfigMngr,
            IJHcLogger logger)
        {
            bool success = true;

            Messenger.Default.Send(
               new HandicapProgressMessage(
                   "Printing next runner."));

            SeriesConfigType config = 
                seriesConfigMngr.ReadSeriesConfiguration();

            try
            {
                using (StreamWriter writer = new StreamWriter(Path.GetFullPath(folder) +
                                                               Path.DirectorySeparatorChar +
                                                               model.CurrentSeason.Name +
                                                               model.CurrentEvent.Name +
                                                               ResultsPaths.nextNewRunner +
                                                               ResultsPaths.csvExtension))
                {
                    writer.WriteLine(config?.NumberPrefix + model.Athletes.NextAvailableRaceNumber.ToString("000000"));
                }
            }
            catch (Exception ex)
            {
                logger.WriteLog("Error, failed to print next runner: " + ex.ToString());

                Messenger.Default.Send(
                  new HandicapErrorMessage(
                      "Failed to print next runner"));

                success = false;
            }

            return success;
        }
    }
}