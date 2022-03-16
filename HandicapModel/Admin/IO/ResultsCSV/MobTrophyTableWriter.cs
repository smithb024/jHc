namespace HandicapModel.Admin.IO.ResultsCSV
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using CommonHandicapLib.Interfaces;
    using CommonHandicapLib.Messages;
    using CommonLib.Types;
    using GalaSoft.MvvmLight.Messaging;
    using HandicapModel.Admin.IO;
    using HandicapModel.Interfaces;
    using HandicapModel.Interfaces.Admin.IO;
    using HandicapModel.Interfaces.Common;
    using HandicapModel.SeasonModel;

    public static class MobTrophyTableWriter
    {
        /// <summary>
        /// Write the Mob Trophy points table to a file
        /// </summary>
        /// <param name="model">junior handicap model</param>
        /// <param name="folder">output folder</param>
        /// <param name="eventData">event data wrapper</param>
        /// <param name="logger">application logger</param>
        /// <returns>success flag</returns>
        public static bool WriteMobTrophyPointsTable(
            IModel model,
            string folder,
            IEventData eventData,
            IJHcLogger logger)
        {
            bool success = true;
            List<DateType> eventDates = new List<DateType>();

            Messenger.Default.Send(
                new HandicapProgressMessage(
                    "Saving Mob Trophy points table"));

            // Export the overall season table.
            try
            {
                using (StreamWriter writer = new StreamWriter(Path.GetFullPath(folder) +
                                                              Path.DirectorySeparatorChar +
                                                              model.CurrentSeason.Name +
                                                              model.CurrentEvent.Name +
                                                              ResultsPaths.mobTrophyPointsTable +
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
                                $"{club.Name}{ResultsPaths.separator}{club.ClubCompetition.TotalPoints}";

                            foreach (DateType eventDate in eventDates)
                            {
                                if (club.ClubCompetition.Points.Exists(points => points.Date == eventDate))
                                {
                                    int eventPoints = club.ClubCompetition.Points.Find(points => points.Date == eventDate).TotalPoints;

                                    entryString = entryString + ResultsPaths.separator + eventPoints;
                                }
                                else
                                {
                                    entryString = entryString + ResultsPaths.separator;
                                }
                            }

                            writer.WriteLine(entryString);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.WriteLog("Error, failed to print mob trophy points table: " + ex.ToString());

                Messenger.Default.Send(
                    new HandicapErrorMessage(
                        "Failed to print mob trophy points table"));

                success = false;
            }

            // Export the table for the current event.
            try
            {
                using (
                    StreamWriter writer =
                    new StreamWriter(
                        Path.GetFullPath(folder) +
                        Path.DirectorySeparatorChar +
                        model.CurrentSeason.Name +
                        model.CurrentEvent.Name +
                        ResultsPaths.mobTrophyPointsTableCurrentEvent +
                        ResultsPaths.csvExtension))
                {
                    string titleString = 
                        "Club" + ResultsPaths.separator +
                        "Total Points" + ResultsPaths.separator + 
                        "Finishing Points" + ResultsPaths.separator + 
                        "Position Points" + ResultsPaths.separator + 
                        "Season Best Points";

                    writer.WriteLine(titleString);

                    foreach (ClubSeasonDetails club in model.CurrentSeason.Clubs)
                    {
                        if (club.ClubCompetition.TotalPoints > 0)
                        {
                            ICommonPoints currentEventPoints =
                                club.ClubCompetition.Points.Find(
                                    e => e.Date == model.CurrentEvent.Date);

                            if (currentEventPoints == null)
                            {
                                continue;
                            }

                            string entryString =
                                club.Name + ResultsPaths.separator +
                                currentEventPoints.TotalPoints + ResultsPaths.separator +
                                currentEventPoints.FinishingPoints + ResultsPaths.separator +
                                currentEventPoints.PositionPoints + ResultsPaths.separator +
                                currentEventPoints.BestPoints;
                            writer.WriteLine(entryString);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.WriteLog("Error, failed to print mob trophy points table: " + ex.ToString());

                Messenger.Default.Send(
                    new HandicapErrorMessage(
                        "Failed to print mob trophy points table"));

                success = false;
            }

            return success;
        }
    }
}