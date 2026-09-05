namespace HandicapModel.Admin.IO.XML
{
    using CommonHandicapLib.Interfaces;
    using CommonHandicapLib.Messages;
    using CommonHandicapLib.XML.ClubData;
    using HandicapModel.ClubsModel;
    using HandicapModel.Interfaces.Admin.IO.XML;
    using NynaeveLib.XML;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using CommonMessenger = NynaeveLib.Messenger.Messenger;

    internal class ClubDataReader : IClubDataReader
    {
        /// <summary>
        /// The instance of the logger.
        /// </summary>
        private readonly IJHcLogger logger;

        /// <summary>
        /// Initialises a new instance of the <see cref="ClubData"/> class.
        /// </summary>
        /// <param name="logger"></param>
        public ClubDataReader(IJHcLogger logger)
        {
            this.logger = logger;
        }

        /// ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>SaveClubData</name>
        /// <date>22/01/15</date>
        /// <summary>
        /// Contructs the xml and writes it to a data file
        /// </summary>
        /// <param name="fileName">file name</param>
        /// <param name="clubList">list of clubs</param>
        /// ---------- ---------- ---------- ---------- ---------- ----------
        public bool SaveClubData(
            string fileName,
            Clubs clubList)
        {
            bool success = true;
            ClubDetailsRoot saveCollection = new ClubDetailsRoot();
            List<string> clubs = new List<string>();

            try
            {
                foreach (string club in clubList.ClubDetails)
                {
                    clubs.Add(club);
                }

                saveCollection.Clubs = clubs;

                XmlFileIo.WriteXml<ClubDetailsRoot>(
                    saveCollection,
                    fileName);
            }
            catch (Exception ex)
            {
                this.logger.WriteLog("Error saving club data " + ex.ToString());
                success = false;
            }

            return success;
        }

        /// ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>LoadClubData</name>
        /// <date>31/01/15</date>
        /// <summary>
        /// Loads the club list from the data file and returns it.
        /// </summary>
        /// <param name="fileName">file name</param>
        /// ---------- ---------- ---------- ---------- ---------- ----------
        public Clubs LoadClubData(string fileName)
        {
            Clubs clubList = new Clubs();

            if (!File.Exists(fileName))
            {
                string error = 
                    string.Format(
                        "Club data file missing, one created - {0}",
                        fileName);

                CommonMessenger.Default.Send(
                    new HandicapErrorMessage(
                        error));
                this.logger.WriteLog(error);

                this.SaveClubData(
                    fileName, 
                    new Clubs());
            }

            try
            {
                ClubDetailsRoot deserialisationClubDetails =
                    XmlFileIo.ReadXml<ClubDetailsRoot>(
                        fileName);

                foreach (string club in deserialisationClubDetails.Clubs)
                {
                    clubList.AddNewClub(club);
                }
            }
            catch (Exception ex)
            {
                this.logger.WriteLog("Error reading club data " + ex.ToString());
            }

            return clubList;
        }
    }
}