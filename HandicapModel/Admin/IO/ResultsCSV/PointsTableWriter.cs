namespace HandicapModel.Admin.IO.ResultsCSV
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CommonHandicapLib;
    using CommonHandicapLib.Messages;
    using GalaSoft.MvvmLight.Messaging;
    using HandicapModel;
    using HandicapModel.Admin.Manage;
    using HandicapModel.Interfaces;
    using HandicapModel.SeasonModel;

    public static class PointsTableWriter
    {
        /// <summary>
        /// Write the results to a comma separated file
        /// </summary>
        /// <param name="model">junior handicap model</param>
        /// <param name="folder">folder to save the file to</param>
        /// <returns>success flag</returns>
        public static bool SavePointsTable(
            IModel model,
            string folder)
        {
            bool success = false;

            Messenger.Default.Send(
                new HandicapProgressMessage(
                    "Saving results."));

            try
            {
                using (StreamWriter writer = new StreamWriter(Path.GetFullPath(folder) +
                                                               Path.DirectorySeparatorChar +
                                                               model.CurrentSeason.Name +
                                                               model.CurrentEvent.Name +
                                                               ResultsPaths.pointsTable +
                                                               ResultsPaths.csvExtension))
                {
                    foreach (AthleteSeasonDetails athlete in model.CurrentSeason.Athletes)
                    {
                        if (athlete.Points.TotalPoints > 0)
                        {
                            double averagePoints = 0;
                            if (athlete.NumberOfAppearances > 0)
                            {
                                averagePoints = (double)athlete.Points.TotalPoints / (double)athlete.NumberOfAppearances;
                            }

                            string entryString = athlete.Name +
                                                 ResultsPaths.separator +
                                                 athlete.Points.TotalPoints.ToString() +
                                                 ResultsPaths.separator +
                                                 model.Athletes.GetPB(athlete.Key).ToString() +
                                                 ResultsPaths.separator +
                                                 athlete.SB.ToString() +
                                                 ResultsPaths.separator +
                                                 athlete.NumberOfAppearances.ToString() +
                                                 ResultsPaths.separator +
                                                 averagePoints.ToString("0.##");

                            writer.WriteLine(entryString);
                        }
                    }

                    success = true;
                }
            }
            catch (Exception ex)
            {
                JHcLogger.GetInstance().WriteLog("Error, failed to print points table: " + ex.ToString());

                Messenger.Default.Send(
                    new HandicapErrorMessage(
                        "Failed to print points table"));

                success = false;
            }

            return success;

        }
    }
}