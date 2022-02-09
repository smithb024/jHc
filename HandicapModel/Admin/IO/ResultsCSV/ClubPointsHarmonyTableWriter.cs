namespace HandicapModel.Admin.IO.ResultsCSV
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using CommonHandicapLib;
    using CommonHandicapLib.Interfaces;
    using CommonHandicapLib.Messages;
    using CommonLib.Types;
    using GalaSoft.MvvmLight.Messaging;
    using HandicapModel.Admin.IO;
    using HandicapModel.Interfaces;
    using HandicapModel.Interfaces.Admin.IO;
    using HandicapModel.SeasonModel;

    /// <summary>
    /// Static class used to write the harmony table to a file.
    /// </summary>
    public static class ClubPointsHarmonyTableWriter
    {
        /// <summary>
        /// Write the club points (harmony) table to a file
        /// </summary>
        /// <param name="model">junior handicap model</param>
        /// <param name="folder">output folder</param>
        /// <param name="eventData">event data wrapper</param>
        /// <param name="logger">application logger</param>
        /// <returns>success flag</returns>
        public static bool Write(
            IModel model,
            string folder,
            IEventData eventData,
            IJHcLogger logger)
        {
            bool success = true;
            List<DateType> eventDates = new List<DateType>();

            Messenger.Default.Send(
                new HandicapProgressMessage(
                    "Saving club points (harmony) table"));

            try
            {
                using (
                    StreamWriter writer =
                    new StreamWriter(
                        Path.GetFullPath(folder) +
                        Path.DirectorySeparatorChar +
                        model.CurrentSeason.Name +
                        model.CurrentEvent.Name +
                        ResultsPaths.clubHarmonyTable +
                        ResultsPaths.csvExtension))
                {
                    string titleString = "Club" + ResultsPaths.separator + "TotalPoints";

                    foreach (string eventName in model.CurrentSeason.Events)
                    {
                        titleString = titleString + ResultsPaths.separator + eventName;
                        eventDates.Add(eventData.LoadEventData(model.CurrentSeason.Name, eventName).EventDate);
                    }

                    writer.WriteLine(titleString);

                    foreach (ClubSeasonDetails club in model.CurrentSeason.Clubs)
                    {
                        if (club.ClubCompetition.TotalPoints > 0)
                        {
                            string entryString =
                                $"{club.Name}{ResultsPaths.separator}{club.HarmonyCompetition.TotalScore}";

                            foreach (DateType eventDate in eventDates)
                            {
                                if (club.ClubCompetition.Points.Exists(points => points.Date == eventDate))
                                {
                                    int eventPoints = club.HarmonyCompetition.Events.Find(points => points.Date == eventDate).Score;

                                    entryString = entryString + ResultsPaths.separator + eventPoints;
                                }
                                else
                                {
                                    entryString += ResultsPaths.separator;
                                }
                            }

                            writer.WriteLine(entryString);
                        }
                    }

                    success = true;
                }
            }
            catch (Exception ex)
            {
                logger.WriteLog("Error, failed to print club points table: " + ex.ToString());

                Messenger.Default.Send(
                    new HandicapErrorMessage(
                        "Failed to print club points table"));

                success = false;
            }

            return success;
        }
    }
}