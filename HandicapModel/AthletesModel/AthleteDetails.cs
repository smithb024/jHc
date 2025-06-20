﻿namespace HandicapModel.AthletesModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CommonHandicapLib.Helpers;
    using CommonHandicapLib.Types;
    using CommonLib.Enumerations;
    using CommonLib.Types;
    using HandicapModel.Admin.Manage;
    using HandicapModel.Common;

    /// <summary>
    /// Created to hold the athlete details in memory.
    /// </summary>
    public class AthleteDetails
    {
        /// <summary>
        /// The configuration information used to work out the handicaps
        /// </summary>
        private readonly NormalisationConfigType normalisationConfig;

        /// <summary>
        ///   Creates a new instance of the AthleteDetails class
        /// </summary>
        /// <param name="key">unique key</param>
        /// <param name="normalisationConfigManager">normalisation config manager</param>
        public AthleteDetails(
            int key,
            INormalisationConfigMngr normalisationConfigManager)
          : this(
              key,
              string.Empty,
              string.Empty,
              string.Empty,
              new TimeType(59, 59),
              SexType.NotSpecified,
              "1970",
              "1",
              "1",
              false,
              true,
              normalisationConfigManager)
        {
        }

        /// <summary>
        ///   Creates a new instance of the AthleteDetails class
        /// </summary>
        /// <param name="key">unique key</param>
        /// <param name="forename">forename of the athlete</param>
        /// <param name="familyName">family name of the athlete</param>
        /// <param name="club">athlete's club</param>
        /// <param name="roundedHandicap">rounded handicap</param>
        /// <param name="sex">athlete's sex</param>
        /// <param name="birthYear">birth year, no longer recorded</param>
        /// <param name="birthMonth">birth month, no longer recorded</param>
        /// <param name="birthDay">birth day, no longer recorded</param>
        /// <param name="signedConsent">
        /// indicates whether the parental consent form has been signed
        /// </param>
        /// <param name="active">active</param>
        /// <param name="normalisationConfigManager">normalisation config manager</param>
        public AthleteDetails(
            int key,
            string forename,
            string familyName,
            string club,
            TimeType roundedHandicap,
            SexType sex,
            string birthYear,
            string birthMonth,
            string birthDay,
            bool signedConsent,
            bool active,
            INormalisationConfigMngr normalisationConfigManager)
        {
            this.Key = key;
            this.Forename = forename;
            this.FamilyName = familyName;
            this.Club = club;
            this.PredeclaredHandicap = roundedHandicap;
            this.Sex = sex;
            this.SignedConsent = signedConsent;
            this.Active = active;

            this.RunningNumbers = new List<string>();
            this.Times = new List<Appearances>();

            this.BirthDate =
              new DateOfBirth(
                birthYear,
                birthMonth,
                birthDay);

            this.normalisationConfig = normalisationConfigManager.ReadNormalisationConfiguration();
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="AthleteDetails"/> class.
        /// </summary>
        /// <param name="key">athlete unique key</param>
        /// <param name="forename">forename of the athlete</param>
        /// <param name="familyName">family name of the athlete</param>
        /// <param name="club">regitered club</param>
        /// <param name="roundedHandicap">current rounded handicap</param>
        /// <param name="sex">sex of the athlete</param>
        /// <param name="signedConsent">
        /// indicates whether the parental consent form has been signed
        /// </param>
        /// <param name="active">indicates whehter the athlete is active</param>
        /// <param name="times">collection of all times</param>
        /// <param name="birthYear">birth year, no longer recorded</param>
        /// <param name="birthMonth">birth month, no longer recorded</param>
        /// <param name="birthDay">birth day, no longer recorded</param>
        /// <param name="normalisationConfigManager">normalisation config manager</param>
        public AthleteDetails(
            int key,
            string forename,
            string familyName,
            string club,
            TimeType roundedHandicap,
            SexType sex,
            bool signedConsent,
            bool active,
            List<Appearances> times,
            string birthYear,
            string birthMonth,
            string birthDay,
            INormalisationConfigMngr normalisationConfigManager)
        {
            this.Key = key;
            this.Forename = forename;
            this.FamilyName = familyName;
            this.Club = club;
            this.PredeclaredHandicap = roundedHandicap;
            this.Sex = sex;
            this.Times = times;
            this.SignedConsent = signedConsent;
            this.Active = active;

            this.RunningNumbers = new List<string>();
            this.Times = new List<Appearances>();

            this.BirthDate =
                new DateOfBirth(
                    birthYear,
                    birthMonth,
                    birthDay);

            this.normalisationConfig = normalisationConfigManager.ReadNormalisationConfiguration();
        }

        /// <summary>
        /// Event which is used to inform interested parties that there has been a change to this model.
        /// </summary>
        public event EventHandler ModelUpdateEvent;

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        public int Key { get; set; }

        /// <summary>
        /// Gets or sets the athlete name
        /// </summary>
        public string Name => $"{this.Forename} {this.FamilyName}";

        /// <summary>
        /// Gets the Forename of the athlete.
        /// </summary>
        public string Forename { get; set; }

        /// <summary>
        /// Gets the Family Name of the athlete.
        /// </summary>
        public string FamilyName { get; set; }

        /// <summary>
        /// Gets or sets all the running numbers used this season this season.
        /// </summary>
        public List<string> RunningNumbers { get; set; }

        /// <summary>
        /// Gets the first number in the list of running numbers.
        /// </summary>
        /// <remarks>
        /// Used for sorting the list by running number. It provides the first registered number to sort by.
        /// </remarks>
        public string PrimaryNumber
        {
            get
            {
                if (this.RunningNumbers.Count > 0)
                {
                    for (int index = 0; index < this.RunningNumbers.Count; ++index)
                    {
                        if (string.Compare(this.RunningNumbers[index].Substring(0, 1), "A") == 0)
                        {
                            return this.RunningNumbers[index];
                        }
                    }

                    return this.RunningNumbers[0];
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Gets or sets the athlete's club
        /// </summary>
        public string Club { get; set; }

        /// <summary>
        /// Gets the athlete's pb.
        /// </summary>
        public TimeType PB
        {
            get
            {
                TimeType pb = new TimeType();

                foreach (Appearances appearance in this.Times)
                {
                    if (appearance.Time < pb)
                    {
                        pb = appearance.Time;
                    }
                }

                return pb;
            }
        }

        /// <summary>
        /// Gets or sets the handicap which has been set by the user.
        /// </summary>
        public TimeType PredeclaredHandicap { get; set; }

        /// <summary>
        /// Gets the athlete's last appearance.
        /// </summary>
        public DateType LastAppearance
        {
            get
            {
                if (this.Appearances == 0)
                {
                    return new DateType();
                }
                else
                {
                    return this.Times.Last().Date;
                }
            }
        }

        /// <summary>
        /// Gets the athlete's number of appearances.
        /// </summary>
        public int Appearances => this.Times.Count();

        /// <summary>
        /// Gets or sets the athlete's sex.
        /// </summary>
        public SexType Sex { get; set; }

        /// <summary>
        /// Gets or sets a value which indicates if the athlete is active.
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets a value which indicates if the parental consent has been signed.
        /// </summary>
        public bool SignedConsent { get; set; }

        /// <summary>
        /// Gets or sets the athletes date of birth.
        /// </summary>
        public DateOfBirth BirthDate { get; set; }

        /// <summary>
        /// Gets or sets the number of events run.
        /// </summary>
        public List<Appearances> Times { get; set; }

        /// <summary>
        /// Adds a new running number to the list.
        /// </summary>
        /// <param name="runningNumber">running number</param>
        public void AddNewNumber(string runningNumber)
        {
            this.RunningNumbers.Add(runningNumber);
        }

        /// <summary>
        ///   Adds new time to the list.
        /// </summary>
        /// <param name="newTime">new time to add</param>
        public void AddRaceTime(Appearances newTime)
        {
            this.Times.Add(newTime);
            this.ModelUpdateEvent?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Remove all appearances corresponding to the argument date. Do nothing if there are no 
        /// appearances for the date.
        /// </summary>
        /// <param name="date"><date of the appearance/param>
        public void RemoveAppearances(DateType date)
        {
            if (this.Times.Exists(dateCheck => dateCheck.Date == date))
            {
                this.Times.Remove(Times.Find(dateCheck => dateCheck.Date == date));
                this.ModelUpdateEvent?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Searches all race numbers associated with this <see cref="AthleteDetails"/>. If the 
        /// <paramref name="inputNumber"/> matches any, true is returned.
        /// </summary>
        /// <param name="inputNumber">number to search against</param>
        /// <returns></returns>
        public bool MatchesAthlete(string inputNumber)
        {
            foreach (string number in this.RunningNumbers)
            {
                if (string.Equals(number, inputNumber))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Indicates whether this <see cref="AthleteDetails"/> is currently a first timer.
        /// </summary>
        /// <remarks>
        /// If there are no appearances, then they are a first timer.
        /// If any of the times are not DNF then they can't be a first timer.
        /// </remarks>
        /// <returns>
        /// Is a first timer.
        /// </returns>
        public bool IsFirstTimer()
        {
            if (this.Appearances == 0)
            {
                return true;
            }

            foreach (Appearances time in this.Times)
            {
                if (time.IsTimeValid())
                {
                    return false;
                }
            }

            return true;
        }
    }
}