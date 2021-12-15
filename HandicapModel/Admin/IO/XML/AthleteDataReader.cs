namespace HandicapModel.Admin.IO.XML
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;

    using CommonHandicapLib;
    using CommonHandicapLib.Interfaces;
    using CommonHandicapLib.Messages;
    using CommonHandicapLib.Types;
    using CommonLib.Converters;
    using CommonLib.Types;
    using GalaSoft.MvvmLight.Messaging;
    using HandicapModel.AthletesModel;
    using HandicapModel.Common;
    using HandicapModel.Interfaces.Admin.IO.XML;

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
        /// The instance of the logger.
        /// </summary>
        private readonly IJHcLogger logger;

        /// <summary>
        /// Initialises a new instance of the <see cref="AthleteDataReader"/> class.
        /// </summary>
        /// <param name="logger">application logger</param>
        public AthleteDataReader(IJHcLogger logger)
        {
            this.logger = logger;
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
            Athletes athleteData = new Athletes();

            if (!File.Exists(fileName))
            {
                string error = string.Format("Athlete Data file missing, one created - {0}", fileName);
                Messenger.Default.Send(
                    new HandicapErrorMessage(
                        error));
                this.logger.WriteLog(error);
                SaveAthleteData(fileName, new Athletes());
            }

            try
            {
                XDocument reader = XDocument.Load(fileName);
                XElement rootElement = reader.Root;
                XElement athleteListElement = rootElement.Element(c_athleteListLabel);

                var athleteList = from Athlete in athleteListElement.Elements(c_athleteLabel)
                                  select new
                                  {
                                      key = (string)Athlete.Attribute(c_keyLabel),
                                      name = (string)Athlete.Attribute(c_nameLabel),
                                      club = (string)Athlete.Attribute(c_clubLabel),
                                      predeclaredHandicap = (string)Athlete.Attribute(predeclaredHandicapLabel),
                                      sex = (string)Athlete.Attribute(c_sexLabel),
                                      signedConsent = (string)Athlete.Attribute(SignedConsentAttribute),
                                      active = (string)Athlete.Attribute(activeLabel),
                                      birthYear = (string)Athlete.Attribute(birthYearAttribute),
                                      birthMonth = (string)Athlete.Attribute(birthMonthAttribute),
                                      birthDay = (string)Athlete.Attribute(birthDayAttribute),
                                      runningNumbers = from RunningNumbers in Athlete.Elements(c_runningNumbersElement)
                                                       select new
                                                       {
                                                           numbers = from Numbers in RunningNumbers.Elements(c_numberElement)
                                                                     select new
                                                                     {
                                                                         number = (string)Numbers.Attribute(c_numberAttribute)
                                                                     }
                                                       },
                                      timeList = from TimeList in Athlete.Elements(c_appearancesLabel)
                                                 select new
                                                 {
                                                     time = from Time in TimeList.Elements(c_timeLabel)
                                                            select new
                                                            {
                                                                time = (string)Time.Attribute(c_raceTimeLabel),
                                                                date = (string)Time.Attribute(c_raceDateLabel)
                                                            }
                                                 }
                                  };

                foreach (var athlete in athleteList)
                {
                    bool signedConsent = this.ConvertBool(athlete.signedConsent);
                    bool isActive = this.ConvertBool(athlete.active);

                    AthleteDetails athleteDetails =
                      new AthleteDetails(
                        (int)StringToIntConverter.ConvertStringToInt(
                          athlete.key),
                          athlete.name,
                          athlete.club,
                          new TimeType(athlete.predeclaredHandicap),
                          StringToSexType.ConvertStringToSexType(athlete.sex),
                          signedConsent,
                          isActive,
                          new List<Appearances>(),
                          athlete.birthYear,
                          athlete.birthMonth,
                          athlete.birthDay);

                    foreach (var runningNumbers in athlete.runningNumbers)
                    {
                        foreach (var number in runningNumbers.numbers)
                        {
                            athleteDetails.AddNewNumber(number.number);
                        }
                    }

                    foreach (var raceTimeList in athlete.timeList)
                    {
                        foreach (var raceTime in raceTimeList.time)
                        {
                            athleteDetails.AddRaceTime(new Appearances(new RaceTimeType(raceTime.time),
                                                                       new DateType(raceTime.date)));
                        }
                    }

                    athleteData.SetNewAthlete(athleteDetails);
                }
            }
            catch (Exception ex)
            {
                this.logger.WriteLog("Error reading athlete data: " + ex.ToString());

                athleteData = new Athletes();
            }

            return athleteData;
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