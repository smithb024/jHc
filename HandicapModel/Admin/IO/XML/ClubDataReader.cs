namespace HandicapModel.Admin.IO.XML
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using CommonHandicapLib;
    using CommonHandicapLib.Messages;
    using GalaSoft.MvvmLight.Messaging;
    using HandicapModel.ClubsModel;

    internal static class ClubDataReader
    {
        private const string c_rootLabel = "ClubDetails";
        private const string c_clubLabel = "club";

        /// ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>SaveClubData</name>
        /// <date>22/01/15</date>
        /// <summary>
        /// Contructs the xml and writes it to a data file
        /// </summary>
        /// <param name="fileName">file name</param>
        /// <param name="clubList">list of clubs</param>
        /// ---------- ---------- ---------- ---------- ---------- ----------
        public static bool SaveClubData(string fileName,
                                        Clubs clubList)
        {
            bool success = true;

            try
            {
                XDocument writer = new XDocument(
                 new XDeclaration("1.0", "uft-8", "yes"),
                 new XComment("Athlete's data XML"));

                XElement rootElement = new XElement(c_rootLabel);
                foreach (string club in clubList.ClubDetails)
                {
                    XElement clubElement = new XElement(c_clubLabel, club);
                    rootElement.Add(clubElement);
                }

                writer.Add(rootElement);
                writer.Save(fileName);
            }
            catch (Exception ex)
            {
                Logger logger = Logger.GetInstance();
                logger.WriteLog("Error saving club data " + ex.ToString());
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
        public static Clubs LoadClubData(string fileName)
        {
            Clubs clubList = new Clubs();

            if (!File.Exists(fileName))
            {
                string error = string.Format("Club data file missing, one created - {0}", fileName);
                Messenger.Default.Send(
                    new HandicapErrorMessage(
                        error));
                Logger.Instance.WriteLog(error);
                SaveClubData(fileName, new Clubs());
            }

            try
            {
                XDocument reader = XDocument.Load(fileName);
                XElement rootElement = reader.Root;

                var myClubList = from Club in rootElement.Elements(c_clubLabel)
                                 select new
                                 {
                                     club = (string)Club.Value
                                 };

                foreach (var club in myClubList)
                {
                    clubList.AddNewClub(club.club);
                }
            }
            catch (Exception ex)
            {
                Logger logger = Logger.GetInstance();
                logger.WriteLog("Error reading club data " + ex.ToString());
            }

            return clubList;
        }
    }
}