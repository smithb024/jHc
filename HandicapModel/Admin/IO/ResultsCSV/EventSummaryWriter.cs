namespace HandicapModel.Admin.IO.ResultsCSV
{
    using System;
    using System.IO;
    using CommonHandicapLib.Interfaces;
    using CommonHandicapLib.Messages;
    using HandicapModel.Interfaces;
    using CommonMessenger = NynaeveLib.Messenger.Messenger;

    public class EventSummaryWriter
    {
        /// <summary>
        /// Write the summary to a file.
        /// </summary>
        /// <param name="model">junior handicap model</param>
        /// <param name="folder">output folder</param>
        /// <param name="logger">application logger</param>
        /// <returns>success folder</returns>
        public static bool WriteEventSummaryTable(
            IModel model,
            string folder,
            IJHcLogger logger)
        {
            bool success = true;

            CommonMessenger.Default.Send(
                new HandicapProgressMessage(
                    "Printing event summary")); 

            try
            {
                using (StreamWriter writer = new StreamWriter(Path.GetFullPath(folder) +
                                                              Path.DirectorySeparatorChar +
                                                              model.CurrentSeason.Name +
                                                              model.CurrentEvent.Name +
                                                              ResultsPaths.summaryTable +
                                                              ResultsPaths.csvExtension))
                {
                    writer.WriteLine(model.CurrentEvent.Name +
                                     ResultsPaths.separator +
                                     model.CurrentEvent.Date);
                    writer.WriteLine();
                    writer.WriteLine("Number Of Runners" +
                                     ResultsPaths.separator +
                                     model.CurrentEvent.Summary.Runners);
                    writer.WriteLine("Number Of Girls" +
                                     ResultsPaths.separator +
                                     model.CurrentEvent.Summary.FemaleRunners);
                    writer.WriteLine("Number Of Boys" +
                                     ResultsPaths.separator +
                                     model.CurrentEvent.Summary.MaleRunners);
                    writer.WriteLine("Number Of Personal Bests" +
                                     ResultsPaths.separator +
                                     model.CurrentEvent.Summary.PBs);
                    writer.WriteLine("Number Of Season Bests" +
                                     ResultsPaths.separator +
                                     model.CurrentEvent.Summary.SBs);
                    writer.WriteLine("Number Of First Timers" +
                                     ResultsPaths.separator +
                                     model.CurrentEvent.Summary.FirstTimers);
                    writer.WriteLine("Fastest Girl" +
                                     ResultsPaths.separator +
                                     model.CurrentEvent.Summary.FastestGirl +
                                     ResultsPaths.separator +
                                     model.CurrentEvent.Summary.FastestGirlTime);
                    writer.WriteLine("Fastest Boy" +
                                     ResultsPaths.separator +
                                     model.CurrentEvent.Summary.FastestBoy +
                                     ResultsPaths.separator +
                                     model.CurrentEvent.Summary.FastestBoyTime);
                }
            }
            catch (Exception ex)
            {
                logger.WriteLog("Error, failed to print event summary: " + ex.ToString());

                CommonMessenger.Default.Send(
                    new HandicapErrorMessage(
                        "Failed to print event summary"));

                success = false;
            }

            return success;
        }
    }
}