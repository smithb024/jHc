namespace HandicapModel.Admin.IO.XML
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;

    using CommonHandicapLib.Interfaces;
    using CommonHandicapLib.Messages;
    using CommonHandicapLib.Types;
    using CommonHandicapLib.XML.AthleteData;
    using CommonLib.Converters;
    using CommonLib.Enumerations;
    using CommonLib.Types;
    using GalaSoft.MvvmLight.Messaging;
    using HandicapModel.Admin.IO.TXT;
    using HandicapModel.Admin.Manage;
    using HandicapModel.AthletesModel;
    using HandicapModel.Common;
    using HandicapModel.Interfaces.Admin.IO.XML;
    using NynaeveLib.XML;

    /// <summary>
    /// IO Class for the Athlete data XML file.
    /// </summary>
    internal class AthleteDataReader : IAthleteDataReader
    {
        /// <summary>
        /// Root label in the file.
        /// </summary>
        private const string c_rootLabel = "AthleteDetails";

        /// <summary>
        /// Athlete list element label
        /// </summary>
        private const string c_athleteListLabel = "AthleteList";

        /// <summary>
        /// Athlete element label
        /// </summary>
        private const string c_athleteLabel = "Athlete";

        /// <summary>
        /// Key element label
        /// </summary>
        private const string c_keyLabel = "Key";

        /// <summary>
        /// Athlete name element label.
        /// </summary>
        private const string c_nameLabel = "Name";

        /// <summary>
        /// Athlete forename element label.
        /// </summary>
        private const string ForenameLabel = "Forename";

        /// <summary>
        /// Athlete family name element label.
        /// </summary>
        private const string FamilyNameLabel = "FamilyName";

        /// <summary>
        /// Athlete club element label
        /// </summary>
        private const string c_clubLabel = "Club";

        /// <summary>
        /// Signed consent element label.
        /// </summary>
        private const string SignedConsentAttribute = "SC";

        /// <summary>
        /// Predeclare handicap element label.
        /// </summary>
        private const string predeclaredHandicapLabel = "HC";

        /// <summary>
        /// Athlete sex element label
        /// </summary>
        private const string c_sexLabel = "Sex";

        /// <summary>
        /// Is currently active element label.
        /// </summary>
        private const string activeLabel = "Atv";

        /// <summary>
        /// Birth year attribute label.
        /// </summary>
        private const string birthYearAttribute = "bY";

        /// <summary>
        /// Birth month attribute label.
        /// </summary>
        private const string birthMonthAttribute = "bM";

        /// <summary>
        /// Birth day attribute label.
        /// </summary>
        private const string birthDayAttribute = "bD";

        /// <summary>
        /// Appearances element label
        /// </summary>
        private const string c_appearancesLabel = "apn";

        /// <summary>
        /// Time element label
        /// </summary>
        private const string c_timeLabel = "time";

        /// <summary>
        /// Race date element label
        /// </summary>
        private const string c_raceTimeLabel = "rtm";

        /// <summary>
        /// Race date element label
        /// </summary>
        private const string c_raceDateLabel = "rdt";

        /// <summary>
        /// Race numbers element lable
        /// </summary>
        private const string c_runningNumbersElement = "runningNumbers";

        /// <summary>
        /// Race number element label
        /// </summary>
        private const string c_numberElement = "number";

        /// <summary>
        /// Race number attribute label.
        /// </summary>
        private const string c_numberAttribute = "no";

        /// <summary>
        /// Normalisation config manager.
        /// </summary>
        private readonly INormalisationConfigMngr normalisationConfigManager;

        /// <summary>
        /// Series config manager
        /// </summary>
        private readonly ISeriesConfigMngr seriesConfigManager;

        /// <summary>
        /// The instance of the logger.
        /// </summary>
        private readonly IJHcLogger logger;

        /// <summary>
        /// Initialises a new instance of the <see cref="AthleteDataReader"/> class.
        /// </summary>
        /// <param name="normalisationConfigManager">Normalisation config manager</param>
        /// <param name="seriesConfigManager">series config manager</param>
        /// <param name="logger">application logger</param>
        public AthleteDataReader(
            INormalisationConfigMngr normalisationConfigManager,
            ISeriesConfigMngr seriesConfigManager,
            IJHcLogger logger)
        {
            this.logger = logger;
            this.normalisationConfigManager = normalisationConfigManager;
            this.seriesConfigManager = seriesConfigManager;
        }

        /// <summary>
        /// Contructs the xml and writes it to a data file
        /// </summary>
        public bool SaveAthleteData(
            string fileName,
            Athletes athleteDetailsList)
        {
            bool success = true;

            try
            {
                AthleteDetailsRoot athleteDetailsRoot = new AthleteDetailsRoot();
                XmlFileIo.WriteXml(
                    athleteDetailsRoot,
                    $"{RootIO.LoadRootFile()}{Path.DirectorySeparatorChar}{IOPaths.configurationPath}{Path.DirectorySeparatorChar}testAthletesDetailsFile.xml");
            }
            catch (XmlException ex)
            {
                this.logger.WriteLog(
                    $"Error writing the Athletes Data file: {ex.XmlMessage}");
            }

            try
            {
                XDocument writer = new XDocument(
                 new XDeclaration("1.0", "uft-8", "yes"),
                 new XComment("Athlete's data XML"));
                XElement rootElement = new XElement(c_rootLabel);
                XElement athleteListElement = new XElement(c_athleteListLabel);

                foreach (AthleteDetails athletesDetails in athleteDetailsList.AthleteDetails)
                {
                    XElement athleteElement = new XElement(c_athleteLabel,
                                                           new XAttribute(c_keyLabel, athletesDetails.Key),
                                                           new XAttribute(c_nameLabel, athletesDetails.Name),
                                                           new XAttribute(c_clubLabel, athletesDetails.Club),
                                                           new XAttribute(predeclaredHandicapLabel, athletesDetails.PredeclaredHandicap.ToString()),
                                                           new XAttribute(c_sexLabel, athletesDetails.Sex),
                                                           new XAttribute(birthYearAttribute, athletesDetails.BirthDate.BirthYear),
                                                           new XAttribute(birthMonthAttribute, athletesDetails.BirthDate.BirthMonth),
                                                           new XAttribute(birthDayAttribute, athletesDetails.BirthDate.BirthDay),
                                                           new XAttribute(SignedConsentAttribute, athletesDetails.SignedConsent),
                                                           new XAttribute(activeLabel, athletesDetails.Active));

                    XElement runningNumberElement = new XElement(c_runningNumbersElement);
                    XElement timesListElement = new XElement(c_appearancesLabel);

                    foreach (string number in athletesDetails.RunningNumbers)
                    {
                        XElement numberElement = new XElement(c_numberElement,
                                                              new XAttribute(c_numberAttribute, number));
                        runningNumberElement.Add(numberElement);
                    }

                    foreach (Appearances time in athletesDetails.Times)
                    {
                        XElement timeElement = new XElement(c_timeLabel,
                                                            new XAttribute(c_raceTimeLabel, time.Time.ToString()),
                                                            new XAttribute(c_raceDateLabel, time.Date.ToString()));
                        timesListElement.Add(timeElement);
                    }

                    athleteElement.Add(runningNumberElement);
                    athleteElement.Add(timesListElement);
                    athleteListElement.Add(athleteElement);
                }

                rootElement.Add(athleteListElement);
                writer.Add(rootElement);
                writer.Save(fileName);
            }
            catch (Exception ex)
            {
                this.logger.WriteLog("Error writing Athlete data " + ex.ToString());
            }


            return success;
        }

        /// <summary>
        /// Reads the athlete details xml from file and decodes it.
        /// </summary>
        /// <param name="fileName">name of xml file</param>
        /// <returns>decoded athlete's details</returns>
        public Athletes ReadAthleteData(string fileName)
        {
            AthleteDetailsRoot deserialisedAthleteDetails;
            Athletes deserialisedAthletes =
                new Athletes(
                    this.seriesConfigManager);

            if (!File.Exists(fileName))
            {
                string error = string.Format("Athlete Data file missing, one created - {0}", fileName);
                Messenger.Default.Send(
                    new HandicapErrorMessage(
                        error));
                this.logger.WriteLog(error);
                SaveAthleteData(fileName, new Athletes(this.seriesConfigManager));
            }

            try
            {
                deserialisedAthleteDetails =
                    XmlFileIo.ReadXml<AthleteDetailsRoot>(
                        fileName);
            }
            catch (XmlException ex)
            {
                deserialisedAthleteDetails = new AthleteDetailsRoot();
                this.logger.WriteLog(
                    $"Error reading the Athletes Data file: {ex.XmlMessage}");
            }

            // Convert the deserialised objects into a model object.
            foreach (Athlete deserialisedAthlete in deserialisedAthleteDetails[0].AllAthletes)
            {
                List<Appearances> athleteAppearances = new List<Appearances>();
                TimeType predeclaredTime =
                        new TimeType(
                            deserialisedAthlete.PredeclaredHandicap);

                AthleteDetails athleteDetails =
                    new AthleteDetails(
                        deserialisedAthlete.Key,
                        deserialisedAthlete.Name,
                        deserialisedAthlete.Club,
                        predeclaredTime,
                        deserialisedAthlete.Sex,
                        deserialisedAthlete.SignedConsent,
                        deserialisedAthlete.Active,
                        athleteAppearances,
                        string.Empty,
                        string.Empty,
                        string.Empty,
                        this.normalisationConfigManager);

                foreach (AthleteDataNumber runningNumbers in deserialisedAthlete.RunningNumbers.Numbers)
                {
                    athleteDetails.AddNewNumber(runningNumbers.Number);
                }

                foreach (AthleteDataTime raceTimeList in deserialisedAthlete.Appearances.Appearances)
                {
                    RaceTimeType time = 
                        new RaceTimeType(
                            raceTimeList.Time);
                    DateType date = 
                        new DateType(
                            raceTimeList.Date);
                    Appearances appearance =
                        new Appearances(
                            time,
                            date);

                    athleteDetails.AddRaceTime(appearance);
                }

                deserialisedAthletes.SetNewAthlete(athleteDetails);
            }

            return deserialisedAthletes;
        }

        /// <summary>
        /// Convert from a string to a boolean. A null string returns false.
        /// The input value has been read from an xml file.
        /// </summary>
        /// <param name="xmlValue">value to convert</param>
        /// <returns>new boolean</returns>
        private bool ConvertBool(string xmlValue)
        {
            if (xmlValue == null)
            {
                return false;
            }

            string test = true.ToString();

            if (string.Compare(xmlValue, true.ToString(), true) == 0)
            {
                return true;
            }

            return false;
        }
    }
}