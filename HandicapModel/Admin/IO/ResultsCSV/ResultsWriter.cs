namespace HandicapModel.Admin.IO.ResultsCSV
{
    using System;
    using System.IO;
    using CommonHandicapLib;
    using CommonHandicapLib.Messages;
    using GalaSoft.MvvmLight.Messaging;
    using HandicapModel.Interfaces;
    using HandicapModel.SeasonModel.EventModel;

    public static class ResultsWriter
    {
        /// <summary>
        /// Write the results to a comma separated file
        /// </summary>
        /// <param name="model">junior handicap model</param>
        /// <param name="folder">folder to save the file to</param>
        /// <returns>success flag</returns>
        public static bool WriteResultsTable(
            IModel model,
            string folder)
        {
            bool success = false;

            Messenger.Default.Send(
                new HandicapProgressMessage(
                    "Printing results."));

            try
            {
                using (StreamWriter writer = new StreamWriter(Path.GetFullPath(folder) +
                                                               Path.DirectorySeparatorChar +
                                                               model.CurrentSeason.Name +
                                                               model.CurrentEvent.Name +
                                                               ResultsPaths.resultsTable +
                                                               ResultsPaths.csvExtension))
                {
                    foreach (ResultsTableEntry entry in model.CurrentEvent.ResultsTable.Entries)
                    {
                        string pbString = entry.PB ? "Y" : string.Empty;
                        string sbString = entry.SB ? "Y" : string.Empty;

                        string entryString = entry.Name +
                                             ResultsPaths.separator +
                                             entry.Time +
                                             ResultsPaths.separator +
                                             entry.RunningTime +
                                             ResultsPaths.separator +
                                             entry.Handicap +
                                             ResultsPaths.separator +
                                             entry.RunningOrder.ToString() +
                                             ResultsPaths.separator +
                                             pbString +
                                             ResultsPaths.separator +
                                             sbString +
                                             ResultsPaths.separator +
                                             entry.Points.TotalPoints +
                                             ResultsPaths.separator +
                                             entry.HarmonyPoints +
                                             ResultsPaths.separator +
                                             entry.Notes +
                                             ResultsPaths.separator +
                                             entry.RaceNumber +
                                             ResultsPaths.separator +
                                             entry.Club +
                                             ResultsPaths.separator +
                                             entry.AgeGrading;

                        writer.WriteLine(entryString);
                    }
                    success = true;
                }
            }
            catch (Exception ex)
            {
                JHcLogger.GetInstance().WriteLog("Error, failed to print results: " + ex.ToString());

                Messenger.Default.Send(
                    new HandicapErrorMessage(
                        "Failed to print results"));

                success = false;
            }

            return success;

        }
    }
}