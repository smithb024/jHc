namespace HandicapModel.Admin.IO.ResultsCSV
{
    using System;
    using System.IO;
    using CommonHandicapLib;
    using CommonHandicapLib.Messages;
    using GalaSoft.MvvmLight.Messaging;
    using HandicapModel.Interfaces;

    public class EventSummaryWriter
    {
        /// <summary>
        /// Write the summary to a file.
        /// </summary>
        /// <param name="model">junior handicap model</param>
        /// <param name="folder">output folder</param>
        /// <returns>success folder</returns>
        public static bool WriteEventSummaryTable(
            IModel model,
            string folder)
        {
            bool success = true;

            Messenger.Default.Send(
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
                JHcLogger.GetInstance().WriteLog("Error, failed to print event summary: " + ex.ToString());

                Messenger.Default.Send(
                    new HandicapErrorMessage(
                        "Failed to print event summary"));

                success = false;
            }

            return success;
        }
    }
}