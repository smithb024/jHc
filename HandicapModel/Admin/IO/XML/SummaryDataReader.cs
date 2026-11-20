namespace HandicapModel.Admin.IO.XML
{
    using CommonHandicapLib.Interfaces;
    using CommonHandicapLib.Messages;
    using CommonHandicapLib.XML.EventsData;
    using CommonHandicapLib.XML.SummaryData;
    using CommonLib.Converters;
    using CommonLib.Types;
    using HandicapModel.Common;
    using HandicapModel.Interfaces.Admin.IO.XML;
    using HandicapModel.Interfaces.Common;
    using NynaeveLib.XML;
    using System;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;
    using CommonMessenger = NynaeveLib.Messenger.Messenger;

    /// <summary>
    /// Summary data reader
    /// </summary>
    internal class SummaryDataReader : ISummaryDataReader
    {
        private const string c_rootLabel = "SummaryDetails";

        private const string c_RunnersLabel = "TotalRunner";
        private const string c_MaleRunnersLabel = "MaleRunners";
        private const string c_FemaleRunnersLabel = "FemaleRunners";
        private const string c_YBLabel = "YBs";
        private const string c_PBLabel = "PBs";
        private const string c_FirstTimersLabel = "FirstTimers";
        private const string c_FastestLabel = "FastestRunners";

        private const string fastestBoysLabel = "FastestBoys";
        private const string fastestGirlsLabel = "FastestGirls";
        private const string timeLabel = "Time";
        private const string keyLabel = "Key";
        private const string c_NameLabel = "Name";
        private const string c_MinutesLabel = "Minutes";
        private const string c_SecondsLabel = "Seconds";
        private const string dateLabel = "Date";

        /// <summary>
        /// The instance of the logger.
        /// </summary>
        private readonly IJHcLogger logger;

        /// <summary>
        /// Initialises a new instance of the <see cref="SummaryDataReader"/> class.
        /// </summary>
        /// <param name="logger"></param>
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
            ISummary summaryData = null;

            if (!File.Exists(fileName))
            {
                string error = $"Summary file missing, one created - {fileName}";
                CommonMessenger.Default.Send(
                    new HandicapErrorMessage(
                        error));
                this.logger.WriteLog(error);
                SaveSummaryData(fileName, new Summary());
            }

            try
            {
                XDocument reader = XDocument.Load(fileName);
                XElement rootElement = reader.Root;

                XElement TotalRunnersElement = rootElement.Element(c_RunnersLabel);
                XElement MaleRunnersElement = rootElement.Element(c_MaleRunnersLabel);
                XElement FemaleRunnersElement = rootElement.Element(c_FemaleRunnersLabel);
                XElement YBsElement = rootElement.Element(c_YBLabel);
                XElement PBsElement = rootElement.Element(c_PBLabel);
                XElement FirstTimersElement = rootElement.Element(c_FirstTimersLabel);
                XElement FastestElement = rootElement.Element(c_FastestLabel);

                XElement FastestBoysElement = FastestElement.Element(fastestBoysLabel);
                XElement FastestGirlsElement = FastestElement.Element(fastestGirlsLabel);


                summaryData = 
                    new Summary(
                        (int)StringToIntConverter.ConvertStringToInt(MaleRunnersElement.Value),
                        (int)StringToIntConverter.ConvertStringToInt(FemaleRunnersElement.Value),
                        (int)StringToIntConverter.ConvertStringToInt(YBsElement.Value),
                        (int)StringToIntConverter.ConvertStringToInt(PBsElement.Value),
                        (int)StringToIntConverter.ConvertStringToInt(FirstTimersElement.Value));

                var boysList = from Athlete in FastestBoysElement.Elements(timeLabel)
                               select new
                               {
                                   key = (string)Athlete.Attribute(keyLabel),
                                   name = (string)Athlete.Attribute(c_NameLabel),
                                   minutes = (int)Athlete.Attribute(c_MinutesLabel),
                                   seconds = (int)Athlete.Attribute(c_SecondsLabel),
                                   date = (string)Athlete.Attribute(dateLabel)
                               };

                foreach (var athlete in boysList)
                {
                    summaryData.SetFastestBoy((int)StringToIntConverter.ConvertStringToInt(athlete.key),
                                              athlete.name,
                                              new TimeType(athlete.minutes, athlete.seconds),
                                              new DateType(athlete.date));
                }

                var girlsList = from Athlete in FastestGirlsElement.Elements(timeLabel)
                                select new
                                {
                                    key = (string)Athlete.Attribute(keyLabel),
                                    name = (string)Athlete.Attribute(c_NameLabel),
                                    minutes = (int)Athlete.Attribute(c_MinutesLabel),
                                    seconds = (int)Athlete.Attribute(c_SecondsLabel),
                                    date = (string)Athlete.Attribute(dateLabel)
                                };

                foreach (var athlete in girlsList)
                {
                    summaryData.SetFastestGirl((int)StringToIntConverter.ConvertStringToInt(athlete.key),
                                               athlete.name,
                                               new TimeType(athlete.minutes, athlete.seconds),
                                               new DateType(athlete.date));
                }
            }
            catch (Exception ex)
            {
                this.logger.WriteLog("Error reading summary data: " + ex.ToString());

                summaryData = new Summary();
            }

            this.SaveSummaryData(fileName, summaryData);
            return summaryData;
        }
    }
}