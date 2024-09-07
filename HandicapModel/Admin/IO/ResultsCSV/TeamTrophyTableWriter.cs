namespace HandicapModel.Admin.IO.ResultsCSV
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using CommonHandicapLib.Interfaces;
    using CommonHandicapLib.Messages;
    using CommonLib.Types;
    using HandicapModel.Admin.IO;
    using HandicapModel.Interfaces;
    using HandicapModel.Interfaces.Admin.IO;
    using HandicapModel.Interfaces.Common;
    using HandicapModel.Interfaces.SeasonModel;
    using HandicapModel.SeasonModel;
    using CommonMessenger = NynaeveLib.Messenger.Messenger;

    /// <summary>
    /// Static class used to write the Team Trophy table to a file.
    /// </summary>
    public static class TeamTrophyTableWriter
    {
        /// <summary>
        /// The number of results which
        /// </summary>
        private const int NumberOfResultsWhichCount = 3;

        /// <summary>
        /// Write the Team Trophy table to a file.
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
            CommonMessenger.Default.Send(
                new HandicapProgressMessage(
                    "Saving Team Trophy points table"));

            string folderPath =
                Path.GetFullPath(folder) +
                Path.DirectorySeparatorChar +
                model.CurrentSeason.Name +
                model.CurrentEvent.Name;

            bool overallSuccess =
                TeamTrophyTableWriter.WriteOverallSeasonTable(
                    model,
                    folderPath,
                    eventData,
                    logger);

            bool currentSuccess =
                TeamTrophyTableWriter.WriteCurrentEventTable(
                    model,
                    folderPath,
                    logger);

            return overallSuccess && currentSuccess;
        }

        /// <summary>
        /// Save the Team Trophy to the file.
        /// </summary>
        /// <param name="model">junior handicap model</param>
        /// <param name="folderPath">path of the folder to save the table to</param>
        /// <param name="eventData">event data wrapper</param>
        /// <param name="logger">application logger</param>
        /// <returns>success flag</returns>
        private static bool WriteOverallSeasonTable(
            IModel model,
            string folderPath,
            IEventData eventData,
            IJHcLogger logger)
        {
            bool success = true;
            List<DateType> eventDates = new List<DateType>();

            try
            {
                string outPath =
                    $"{folderPath}{ResultsPaths.teamTrophyPointsTable}{ResultsPaths.csvExtension}";

                using (
                    StreamWriter writer =
                    new StreamWriter(
                        outPath))
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
                        int totalScore =
                            TeamTrophyTableWriter.CalculateTotalScore(
                                club.TeamTrophy.Events);

                        if (totalScore > 0)
                        {
                            string entryString =
                                $"{club.Name}{ResultsPaths.separator}{totalScore}";

                            foreach (DateType eventDate in eventDates)
                            {
                                if (club.MobTrophy.Points.Exists(points => points.Date == eventDate))
                                {

                                    ITeamTrophyEvent foundEvent =
                                        club.TeamTrophy.Events.Find(
                                            points =>
                                            points.Date == eventDate);

                                    int eventPoints =
                                        foundEvent != null
                                        ? foundEvent.Score
                                        : 0;

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
                }
            }
            catch (Exception ex)
            {
                logger.WriteLog("Error, failed to print club points table: " + ex.ToString());

                CommonMessenger.Default.Send(
                    new HandicapErrorMessage(
                        "Failed to print club points table"));

                success = false;
            }

            return success;
        }

        /// <summary>
        /// Save the Team Trophy current event table.
        /// </summary>
        /// <param name="model">junior handicap model</param>
        /// <param name="folderPath">path of the folder to save the table to</param>
        /// <param name="logger">application logger</param>
        /// <returns>success flag</returns>
        private static bool WriteCurrentEventTable(
            IModel model,
            string folderPath,
            IJHcLogger logger)
        {
            bool success = true;

            try
            {
                string outPath =
                    $"{folderPath}{ResultsPaths.teamTrophyPointsTableCurrentEvent}{ResultsPaths.csvExtension}";

                using (
                    StreamWriter writer =
                    new StreamWriter(
                        outPath))
                {
                    string titleString = "Club" + ResultsPaths.separator + "Score" + ResultsPaths.separator + "Points";

                    writer.WriteLine(titleString);

                    foreach (ClubSeasonDetails club in model.CurrentSeason.Clubs)
                    {
                        if (club.MobTrophy.TotalPoints > 0)
                        {
                            ITeamTrophyEvent foundEvent =
                                club.TeamTrophy.Events.Find(
                                    e => e.Date == model.CurrentEvent.Date);

                            if (foundEvent == null)
                            {
                                continue;
                            }

                            string entryString =
                                $"{club.Name}{ResultsPaths.separator}{foundEvent.Score}{ResultsPaths.separator}{foundEvent.TotalAthletePoints}";

                            foreach (ICommonTeamTrophyPoints commonPoints in foundEvent.Points)
                            {
                                entryString = entryString + ResultsPaths.separator + commonPoints.Point;
                            }

                            writer.WriteLine(entryString);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.WriteLog("Error, failed to print club points table: " + ex.ToString());

                CommonMessenger.Default.Send(
                    new HandicapErrorMessage(
                        "Failed to print club points table"));

                success = false;
            }

            return success;
        }

        /// <summary>
        /// Calculate the score of the team, from all valid scores.
        /// </summary>
        /// <param name="events">All events the team has been involved in</param>
        /// <returns>total score</returns>
        private static int CalculateTotalScore(
            List<ITeamTrophyEvent> events)
        {
            int totalScore = 0;
            List<int> scores = new List<int>();

            foreach(ITeamTrophyEvent teamtrophyEvent in events)
            {
                scores.Add(teamtrophyEvent.Score);
            }

            scores = scores.OrderByDescending(s => s).ToList();

            for (int index = 0; index < NumberOfResultsWhichCount; ++index)
            {
                if (index < scores.Count)
                {
                    totalScore += scores[index];
                }
            }

            return totalScore;
        }
    }
}