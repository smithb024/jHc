namespace HandicapModel.Admin.IO.XML
{
    using CommonHandicapLib.Interfaces;
    using CommonHandicapLib.Messages;
    using CommonHandicapLib.XML.SummaryData;
    using CommonLib.Types;
    using HandicapModel.Common;
    using HandicapModel.Interfaces.Admin.IO.XML;
    using HandicapModel.Interfaces.Common;
    using NynaeveLib.XML;
    using System;
    using System.IO;
    using CommonMessenger = NynaeveLib.Messenger.Messenger;

    /// <summary>
    /// Summary data reader
    /// </summary>
    public class SummaryDataReader : ISummaryDataReader
    {
        /// <summary>
        /// The instance of the logger.
        /// </summary>
        private readonly IJHcLogger logger;

        /// <summary>
        /// Initialises a new instance of the <see cref="SummaryDataReader"/> class.
        /// </summary>
        /// <param name="logger">The program logger</param>
        public SummaryDataReader(IJHcLogger logger)
        {
            this.logger = logger;
        }

        /// ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>SaveSummaryData</name>
        /// <date>31/01/15</date>
        /// <summary>
        /// Contructs the xml and writes it to a data file
        /// </summary>
        /// ---------- ---------- ---------- ---------- ---------- ----------
        public bool SaveSummaryData(
            string fileName,
            ISummary summaryDetails)
        {
            bool success = true;
            SummaryDataRoot saveData = new SummaryDataRoot();

            try
            {
                FastestRunnersRoot fastestRunners = new FastestRunnersRoot();
                FastestGirlsRoot fastestGirls = 
                    new FastestGirlsRoot
                    {
                        Appearances = new FastestTimes()
                    };
                FastestBoysRoot fastestBoys = 
                    new FastestBoysRoot()
                    {
                        Appearances = new FastestTimes()
                    };

                foreach (IAthleteTime athleteTime in summaryDetails.FastestGirls)
                {
                    FastestTime fastestTime =
                        new FastestTime()
                        {
                            Key = athleteTime.Key,
                            Name = athleteTime.Name,
                            Date = athleteTime.Date.ToString(),
                            Minutes = athleteTime.Time.Minutes,
                            Seconds = athleteTime.Time.Seconds
                        };
                    fastestGirls.Appearances.Add(fastestTime);
                }

                foreach (IAthleteTime athleteTime in summaryDetails.FastestBoys)
                {
                    FastestTime fastestTime =
                        new FastestTime()
                        {
                            Key = athleteTime.Key,
                            Name = athleteTime.Name,
                            Date = athleteTime.Date.ToString(),
                            Minutes = athleteTime.Time.Minutes,
                            Seconds = athleteTime.Time.Seconds
                        };
                    fastestBoys.Appearances.Add(fastestTime);
                }

                fastestRunners.Girls = fastestGirls;
                fastestRunners.Boys = fastestBoys;
                saveData.FastestRunners = fastestRunners;

                XmlFileIo.WriteXml<SummaryDataRoot>(
                    saveData,
                    fileName);

            }
            catch (Exception ex)
            {
                this.logger.WriteLog("Error writing Athlete data " + ex.ToString());
            }

            return success;
        }

        /// ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>ReadCompleteSummaryData</name>
        /// <date>31/01/15</date>
        /// <summary>
        /// Reads the athlete details xml from file and decodes it.
        /// </summary>
        /// <param name="fileName">name of xml file</param>
        /// <returns>decoded summary data</returns>
        /// ---------- ---------- ---------- ---------- ---------- ----------
        public ISummary ReadCompleteSummaryData(string fileName)
        {
            ISummary summaryData;
            SummaryDataRoot deserialisedSummary;

            if (!File.Exists(fileName))
            {
                string error = $"Summary file missing, one created - {fileName}";
                CommonMessenger.Default.Send(
                    new HandicapErrorMessage(
                        error));
                this.logger.WriteLog(error);
                this.SaveSummaryData(
                    fileName, 
                    new Summary());
            }

            try
            {
                deserialisedSummary =
                    XmlFileIo.ReadXml<SummaryDataRoot>(
                        fileName);

                summaryData =
                    new Summary(
                        deserialisedSummary.MaleRunners,
                        deserialisedSummary.FemaleRunners,
                        deserialisedSummary.TotalYBs,
                        deserialisedSummary.TotalPBs,
                        deserialisedSummary.TotalFirstTimers);

                foreach (FastestTime fastestGirl in deserialisedSummary.FastestRunners.Girls.Appearances)
                {
                    TimeType registeredTime =
                        new TimeType(
                            fastestGirl.Minutes,
                            fastestGirl.Seconds);
                    DateType registeredDate =
                        new DateType(
                            fastestGirl.Date);
                    IAthleteTime time =
                        new AthleteTime(
                            fastestGirl.Key,
                            fastestGirl.Name,
                            registeredTime,
                            registeredDate);
                    summaryData.FastestGirls.Add(time);
                }

                foreach (FastestTime fastestBoy in deserialisedSummary.FastestRunners.Boys.Appearances)
                {
                    TimeType registeredTime =
                        new TimeType(
                            fastestBoy.Minutes,
                            fastestBoy.Seconds);
                    DateType registeredDate =
                        new DateType(
                            fastestBoy.Date);
                    IAthleteTime time =
                        new AthleteTime(
                            fastestBoy.Key,
                            fastestBoy.Name,
                            registeredTime,
                            registeredDate);
                    summaryData.FastestBoys.Add(time);
                }
            }
            catch (Exception ex)
            {
                this.logger.WriteLog("Error reading summary data: " + ex.ToString());

                summaryData = new Summary();
            }

            return summaryData;
        }
    }
}