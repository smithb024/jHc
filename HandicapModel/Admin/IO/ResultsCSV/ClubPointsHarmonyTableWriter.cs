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
    using CommonLib.Types;
    using GalaSoft.MvvmLight.Messaging;
    using HandicapModel;
    using HandicapModel.Admin.IO;
    using HandicapModel.Admin.Manage;
    using HandicapModel.Interfaces;
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
        /// <returns>success flag</returns>
        public static bool Write(
            IModel model,
            string folder)
        {
            bool success = true;
            List<DateType> eventDates = new List<DateType>();

            Messenger.Default.Send(
                new HandicapProgressMessage(
                    "Saving club points (harmony) table"));

            try
            {
                using (StreamWriter writer =
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
                        eventDates.Add(EventData.LoadEventData(model.CurrentSeason.Name, eventName).EventDate);
                    }

                    writer.WriteLine(titleString);

                    foreach (ClubSeasonDetails club in model.CurrentSeason.Clubs)
                    {
                        if (club.ClubCompetition.TotalPoints > 0)
                        {
                            string entryString =
                                $"{club.Name}{ResultsPaths.separator}{club.HarmonyCompetition.TotalPoints}";

                            foreach (DateType eventDate in eventDates)
                            {
                                if (club.ClubCompetition.Points.Exists(points => points.Date == eventDate))
                                {
                                    int eventPoints = club.HarmonyCompetition.Events.Find(points => points.Date == eventDate).TotalPoints;

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
                Logger.GetInstance().WriteLog("Error, failed to print club points table: " + ex.ToString());

                Messenger.Default.Send(
                    new HandicapErrorMessage(
                        "Failed to print club points table"));

                success = false;
            }

            return success;
        }
    }
}