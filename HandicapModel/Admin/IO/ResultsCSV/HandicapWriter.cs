namespace HandicapModel.Admin.IO.ResultsCSV
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using CommonHandicapLib.Interfaces;
    using CommonHandicapLib.Messages;
    using CommonHandicapLib.Types;
    using CommonLib.Types;
    using HandicapModel.Admin.Manage;
    using HandicapModel.AthletesModel;
    using HandicapModel.Interfaces;
    using CommonMessenger = NynaeveLib.Messenger.Messenger;

    public static class HandicapWriter
    {
        /// <summary>
        /// Write the handicaps to a file.
        /// </summary>
        /// <param name="model">junior handicap model</param>
        /// <param name="folder">output folder</param>
        /// <param name="normalisationConfigMngr">normalisation configuration manager</param>
        /// <param name="logger">application logger</param>
        /// <returns>success flag</returns>
        public static bool WriteHandicapTable(
            IModel model,
            string folder,
            INormalisationConfigMngr normalisationConfigMngr,
            IJHcLogger logger)
        {
            bool success = true;

            CommonMessenger.Default.Send(
               new HandicapProgressMessage(
                   "Printing handicap."));

            try
            {
                NormalisationConfigType hcConfiguration = 
                    normalisationConfigMngr.ReadNormalisationConfiguration();

                using (StreamWriter writer = new StreamWriter(Path.GetFullPath(folder) +
                                                               Path.DirectorySeparatorChar +
                                                               model.CurrentSeason.Name +
                                                               model.CurrentEvent.Name +
                                                               ResultsPaths.handicapTable +
                                                               ResultsPaths.csvExtension))
                {
                    List<AthleteDetails> athletes = new List<AthleteDetails>(model.Athletes.AthleteDetails);
                    athletes = athletes.OrderBy(athlete => athlete.Forename).ToList();
                    athletes = athletes.OrderBy(athlete => athlete.FamilyName).ToList();

                    foreach (AthleteDetails athlete in athletes)
                    {
                        if (!athlete.Active)
                        {
                            continue;
                        }

                        string number =
                          model.Athletes.GetAthleteRunningNumber(
                            athlete.Key);
                        TimeType newHandicap =
                          model.CurrentSeason.GetAthleteHandicap(
                            athlete.Key,
                            hcConfiguration);
                        string consented =
                            athlete.SignedConsent
                            ? "Y"
                            : string.Empty;

                        // Use default handicap, if the athlete is not registered for the current season.
                        if (newHandicap == null)
                        {
                            newHandicap = athlete.RoundedHandicap;
                        }

                        string entryString = athlete.Name +
                                             ResultsPaths.separator +
                                             number +
                                             ResultsPaths.separator +
                                             newHandicap +
                                             ResultsPaths.separator +
                                             athlete.Club +
                                             ResultsPaths.separator +
                                             consented;

                        writer.WriteLine(entryString);
                    }
                    success = true;
                }
            }
            catch (Exception ex)
            {
                logger.WriteLog("Error, failed to print handicap: " + ex.ToString());

                CommonMessenger.Default.Send(
                  new HandicapErrorMessage(
                      "Failed to print handicap"));

                success = false;
            }

            return success;
        }
    }
}