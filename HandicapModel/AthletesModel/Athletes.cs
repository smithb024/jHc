namespace HandicapModel.AthletesModel
{
    using System.Collections.Generic;
    using CommonHandicapLib.Types;
    using CommonLib.Types;
    using CommonLib.Enumerations;
    using HandicapModel.Common;
    using HandicapModel.Admin.Manage;

    /// <summary>
    /// Manages the athletes in the model.
    /// </summary>
    public class Athletes
    {
        /// <summary>
        /// The series configuration manager
        /// </summary>
        private readonly ISeriesConfigMngr seriesConfigManager;

        /// <summary>
        /// The next available key.
        /// </summary>
        private int nextKey = 0;

        /// <summary>
        /// Initialises a new instance of the <see cref="Athletes"/> class.
        /// </summary>
        /// <param name="seriesConfigMngr">series configuration manager</param>
        public Athletes(ISeriesConfigMngr seriesConfigMngr)
        {
            this.seriesConfigManager = seriesConfigMngr;
            this.AthleteDetails = new List<AthleteDetails>();
        }

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>NextKey</name>
        /// <date>18/01/15</date>
        /// <summary>
        /// Gets the next key.
        /// </summary>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        public int NextKey
        {
            get
            {
                ++nextKey;
                return nextKey;
            }

            private set
            {
                nextKey = value;
            }
        }

        /// <summary>
        /// Gets the athlete details.
        /// </summary>
        public List<AthleteDetails> AthleteDetails { get; }

        /// <summary>
        ///   Adds a new athlete to the list
        /// </summary>
        /// <param name="details">new athlete's details</param>
        public void SetNewAthlete(AthleteDetails details)
        {
            this.AthleteDetails.Add(details);
            this.SetNextKey();
        }

        /// <summary>
        ///   Deletes an athlete from the list.
        /// </summary>
        /// <param name="athleteKey">athlete's unique identifier</param>
        public void DeleteAthlete(int athleteKey)
        {
            for (int index = 0; index < this.AthleteDetails.Count; ++index)
            {
                if (this.AthleteDetails[index].Key == athleteKey)
                {
                    this.AthleteDetails.RemoveAt(index);
                    break;
                }
            }

            this.SetNextKey();
        }

        /// <summary>
        /// Changes an athlete's club.
        /// </summary>
        /// <param name="athleteKey">athlete's unique identifier</param>
        /// <param name="newClub">new club</param>
        /// <param name="runningNumbers">running number collection</param>
        /// <param name="signedConsent">
        /// indicates whether a signed consent form has been received.
        /// </param>
        /// <param name="active">indicates if the athlete is active or not.</param>
        /// <param name="predeclaredHandicap">user declared handicap</param>
        public void ChangeAthlete(
            int athleteKey,
            string newClub,
            List<string> runningNumbers,
            bool signedConsent,
            bool active,
            TimeType predeclaredHandicap)
        {
            for (int index = 0; index < this.AthleteDetails.Count; ++index)
            {
                if (this.AthleteDetails[index].Key == athleteKey)
                {
                    this.AthleteDetails[index].Club = newClub;
                    this.AthleteDetails[index].SignedConsent = signedConsent;
                    this.AthleteDetails[index].Active = active;
                    this.AthleteDetails[index].RunningNumbers = runningNumbers;
                    this.AthleteDetails[index].PredeclaredHandicap = predeclaredHandicap;
                    break;
                }
            }
        }

        /// <summary>
        /// Sets up the Key which is used as a unique identifier for each athlete. It finds the 
        /// highest unique number.
        /// </summary>
        public void SetNextKey()
        {
            NextKey = 0;
            foreach (AthleteDetails athlete in this.AthleteDetails)
            {
                if (athlete.Key > NextKey)
                {
                    NextKey = athlete.Key;
                }
            }
        }

        /// <summary>
        /// Use the key to get the name of the athlete.
        /// </summary>
        /// <param name="key">unique key</param>
        /// <returns>athlete name</returns>
        public string GetAthleteName(int key)
        {
            return AthleteDetails.Find(athlete => athlete.Key == key)?.Name ?? "Unknown Athlete";
        }

        /// <summary>
        /// Use the key to get the sex of the athlete.
        /// </summary>
        /// <param name="key">unique key</param>
        /// <returns>athlete's club</returns>
        public string GetAthleteClub(int key)
        {
            return AthleteDetails.Find(athlete => athlete.Key == key)?.Club ?? string.Empty;
        }

        /// <summary>
        /// Use the key to get the age of the athlete.
        /// </summary>
        /// <param name="key">unique key</param>
        /// <returns>athlete age</returns>
        public int? GetAthleteAge(int key)
        {
            return AthleteDetails.Find(athlete => athlete.Key == key)?.BirthDate.Age ?? null;
        }

        /// <summary>
        /// Use the key to get the sex of the athlete.
        /// </summary>
        /// <param name="key">unique key</param>
        /// <returns>athlete's sex</returns>
        public SexType GetAthleteSex(int key)
        {
            return AthleteDetails.Find(athlete => athlete.Key == key)?.Sex ?? SexType.NotSpecified;
        }

        /// <summary>
        /// Uses the key to identify the athlete then checks to see how many events have been 
        /// completed. If zero then the athlete is a first timer, return true.
        /// </summary>
        /// <param name="key">unique key</param>
        /// <returns>true if a first timer.</returns>
        public bool IsFirstTimer(int key)
        {
            AthleteDetails athlete = AthleteDetails.Find(a => a.Key == key);

            if (athlete == null)
            {
                return false;
            }

            return athlete.Appearances == 0;
        }

        /// <summary>
        /// Get the handicap (rounded)
        /// </summary>
        /// <param name="key">unique key</param>
        /// <returns>handicap value</returns>
        public TimeType GetRoundedHandicap(int key)
        {
            return (AthleteDetails.Find(athlete => athlete.Key == key)?.RoundedHandicap ?? new TimeType(0, 0));
        }

        /// <summary>
        /// Get the athlete's PB
        /// </summary>
        /// <param name="key">athlete key</param>
        /// <returns>PB value</returns>
        public TimeType GetPB(int key)
        {
            return (AthleteDetails.Find(athlete => athlete.Key == key)?.PB ?? new TimeType(0, 0));
        }

        /// <summary>
        /// Add a new time to the indicated (key) athlete.
        /// </summary>
        /// <param name="key">unique key</param>
        /// <param name="newTime">new time to add</param>
        public void AddNewTime(int key, Appearances newTime)
        {
            AthleteDetails athlete = AthleteDetails.Find(a => a.Key == key);

            if (athlete == null)
            {
                return;
            }

            athlete.AddRaceTime(newTime);
        }

        /// <summary>
        /// Counts all the athletes registered to the named club.
        /// </summary>
        /// <param name="clubName">club to check</param>
        /// <returns>number of athletes registed to the named club</returns>
        public int GetNumberRegisteredToClub(string clubName)
        {
            return AthleteDetails.FindAll(athlete => athlete.Club == clubName).Count;
        }

        /// <summary>
        /// Gets the primary running number assigned to an athlete.
        /// </summary>
        /// <param name="key">unique key</param>
        /// <returns>the primary running number</returns>
        public string GetAthleteRunningNumber(int key)
        {
            foreach (AthleteDetails details in this.AthleteDetails)
            {
                if (details.Key == key)
                {
                    return details.PrimaryNumber;
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Gets all the running numbers assigned to an athlete.
        /// </summary>
        /// <param name="key">unique key</param>
        /// <returns>all running numbers</returns>
        public List<string> GetAthleteRunningNumbers(int key)
        {
            foreach (AthleteDetails details in this.AthleteDetails)
            {
                if (details.Key == key)
                {
                    return details.RunningNumbers;
                }
            }

            return new List<string>();
        }

        /// <summary>
        /// Gets the next available race number based on the highest existing one.
        /// </summary>
        /// <remarks>
        /// To be made obsolete, race numbers managed at global level.
        /// </remarks>
        public int NextAvailableRaceNumber
        {
            get
            {
                int nextRaceNumber = 1;
                SeriesConfigType config = 
                    this.seriesConfigManager.ReadSeriesConfiguration();
                string numberPrefix = config?.NumberPrefix;

                foreach (AthleteDetails athlete in this.AthleteDetails)
                {
                    foreach (string number in athlete.RunningNumbers)
                    {
                        if (number.Length > numberPrefix.Length &&
                          string.Equals(number.Substring(0, numberPrefix.Length), numberPrefix))
                        {
                            int raceNumber;
                            if (int.TryParse(number.Substring(numberPrefix.Length), out raceNumber))
                            {
                                if (raceNumber >= nextRaceNumber)
                                {
                                    nextRaceNumber = raceNumber + 1;
                                }
                            }
                        }
                    }
                }

                return nextRaceNumber;
            }
        }

        /// <summary>
        /// Finds the athlete with the given running number and returns their unique id.
        /// </summary>
        /// <param name="runningNumber">running number to check against</param>
        /// <returns>athlete's unique id</returns>
        public int? GetAthleteKey(string runningNumber)
        {
            foreach (AthleteDetails athlete in this.AthleteDetails)
            {
                if (athlete.RunningNumbers.Contains(runningNumber))
                {
                    return athlete.Key;
                }
            }

            return null;
        }
    }
}