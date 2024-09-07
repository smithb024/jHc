namespace HandicapModel.Admin.IO.ResultsCSV
{
    using System;
    using System.IO;
    using CommonHandicapLib.Interfaces;
    using CommonHandicapLib.Messages;
    using HandicapModel.Interfaces;
    using HandicapModel.SeasonModel;
    using CommonMessenger = NynaeveLib.Messenger.Messenger;

    public static class PointsTableWriter
    {
        /// <summary>
        /// Write the results to a comma separated file
        /// </summary>
        /// <param name="model">junior handicap model</param>
        /// <param name="folder">folder to save the file to</param>
        /// <param name="logger">application logger</param>
        /// <returns>success flag</returns>
        public static bool SavePointsTable(
            IModel model,
            string folder,
            IJHcLogger logger)
        {
            bool success = false;

            CommonMessenger.Default.Send(
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
                logger.WriteLog("Error, failed to print points table: " + ex.ToString());

                CommonMessenger.Default.Send(
                    new HandicapErrorMessage(
                        "Failed to print points table"));

                success = false;
            }

            return success;

        }
    }
}