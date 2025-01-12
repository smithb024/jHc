namespace HandicapModel.Admin.IO.XML
{
    using System.Collections.Generic;
    using System.IO;
    using CommonHandicapLib.Helpers;
    using CommonHandicapLib.Interfaces;
    using CommonHandicapLib.Messages;
    using CommonHandicapLib.Types;
    using CommonHandicapLib.XML.AthleteData;
    using CommonLib.Types;
    using HandicapModel.Admin.Manage;
    using HandicapModel.AthletesModel;
    using HandicapModel.Common;
    using HandicapModel.Interfaces.Admin.IO.XML;
    using NynaeveLib.XML;
    using CommonMessenger = NynaeveLib.Messenger.Messenger;

    /// <summary>
    /// IO Class for the Athlete data XML file.
    /// </summary>
    public class AthleteDataReader : IAthleteDataReader
    {
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
                AthleteCollection saveCollection = new AthleteCollection();
                foreach (AthleteDetails athletesDetails in athleteDetailsList.AthleteDetails)
                {
                    AthleteDataNumbers saveAthleteDataNumbers =
                        new AthleteDataNumbers();
                    foreach (string runningNumber in athletesDetails.RunningNumbers)
                    {
                        AthleteDataNumber saveAthleteDataNumber =
                            new AthleteDataNumber()
                            {
                                Number = runningNumber
                            };
                        saveAthleteDataNumbers.Add(saveAthleteDataNumber);
                    }

                    AthleteDataRunningNumbers saveAthleteDataRunningNumbers =
                        new AthleteDataRunningNumbers()
                        {
                            Numbers = saveAthleteDataNumbers,
                        };

                    AthleteDataTimes saveAthleteDataTimes = new AthleteDataTimes();
                    foreach (Appearances appearance in athletesDetails.Times) 
                    {
                        AthleteDataTime saveAthleteDataTime =
                            new AthleteDataTime()
                            {
                                Date = appearance.DateString,
                                Time = appearance.TimeString
                            };
                        saveAthleteDataTimes.Add(saveAthleteDataTime);
                    }

                    AthleteDataAppearances saveAthleteDataAppearances =
                        new AthleteDataAppearances()
                        {
                            Appearances = saveAthleteDataTimes
                        };

                    Athlete saveAthlete = 
                        new Athlete()
                        {
                           Key = athletesDetails.Key,
                           Name = athletesDetails.Name,
                           Forename = athletesDetails.Forename,
                           FamilyName = athletesDetails.FamilyName,
                           Club = athletesDetails.Club,
                           Sex = athletesDetails.Sex,
                           SignedConsent = athletesDetails.SignedConsent,
                           Active = athletesDetails.Active,
                           PredeclaredHandicap = athletesDetails.PredeclaredHandicap.ToString(),
                           Appearances = saveAthleteDataAppearances,
                           RunningNumbers = saveAthleteDataRunningNumbers
                        };
                    saveCollection.Add(saveAthlete);
                }

                AthleteList saveList =
                    new AthleteList()
                    {
                        AllAthletes = saveCollection
                    };
                AthleteDetailsRoot athleteDetailsRoot =
                    new AthleteDetailsRoot()
                    {
                        saveList
                    };
                XmlFileIo.WriteXml(
                     athleteDetailsRoot,
                     fileName);

                this.logger.WriteLog(
                    $"Writen the Athletes Data file: {fileName}");

            }
            catch (XmlException ex)
            {
                this.logger.WriteLog(
                    $"Error writing the Athletes Data file: {ex.XmlMessage}");
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
                CommonMessenger.Default.Send(
                    new HandicapErrorMessage(
                        error));
                this.logger.WriteLog(error);
                this.SaveAthleteData(fileName, new Athletes(this.seriesConfigManager));
            }

            try
            {
                deserialisedAthleteDetails =
                    XmlFileIo.ReadXml<AthleteDetailsRoot>(
                        fileName);
            }
            catch (XmlException ex)
            {
                this.logger.WriteLog(
                    $"Error reading the Athletes Data file: {ex.XmlMessage}");
                return new Athletes(this.seriesConfigManager);
            }

            // Convert the deserialised objects into a model object.
            foreach (Athlete deserialisedAthlete in deserialisedAthleteDetails[0].AllAthletes)
            {
                List<Appearances> athleteAppearances = new List<Appearances>();
                TimeType predeclaredTime =
                        new TimeType(
                            deserialisedAthlete.PredeclaredHandicap);

                string deserialisedForename =
                    string.IsNullOrEmpty(deserialisedAthlete.Forename)
                    ? deserialisedAthlete.Forename
                    : NameHelper.GetForename(deserialisedAthlete.Name);
                string deserialisedFamilyName =
                    string.IsNullOrEmpty(deserialisedAthlete.FamilyName)
                    ? deserialisedAthlete.FamilyName
                    : NameHelper.GetSurname(deserialisedAthlete.Name);

                AthleteDetails athleteDetails =
                    new AthleteDetails(
                        deserialisedAthlete.Key,
                        deserialisedForename,
                        deserialisedFamilyName,
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
    }
}