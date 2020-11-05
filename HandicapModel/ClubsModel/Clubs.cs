namespace HandicapModel.ClubsModel
{
    using System.Collections.Generic;

    /// <summary>
    /// Manages the clubs in the model.
    /// </summary>
    public class Clubs
    {
        /// <summary>
        ///   Creates a new instance of the ClubListType class
        /// </summary>
        public Clubs()
        {
            this.ClubDetails = new List<string>();
        }

        /// <summary>
        /// Gets a list of all the clubs.
        /// </summary>
        public List<string> ClubDetails { get; }

        /// <summary>
        ///   Adds a new club to the list
        /// </summary>
        /// <param name="club">new club</param>
        public void AddNewClub(string club)
        {
            this.ClubDetails.Add(club);
        }

        /// <summary>
        /// Remove the club at the selected index
        /// </summary>
        /// <param name="selectedIndex">selected index</param>
        public void RemoveClub(string name)
        {
            this.ClubDetails.RemoveAt(
                this.ClubDetails.FindIndex(
                    clubName => clubName == name));
        }
    }
}