namespace jHCVMUI.ViewModels.ViewModels.Helpers
{
    using System;
    using System.Linq;
    using System.Collections.ObjectModel;
    using Types;

    /// <summary>
    /// Filter factory, used to used to filter collections of athletes.
    /// </summary>
    public static class AthleteCollectionFilter
    {
        /// <summary>
        /// Return a collection of <see cref="AthleteType"/> objects where the first letter of the 
        /// surname matches the <paramref name="letter"/>. 
        /// If <paramref name="letter"/> is empty, everything is returned.
        /// </summary>
        /// <param name="originalCollection">collection to search</param>
        /// <param name="letter">search parameter</param>
        /// <returns>filtered collection</returns>
        public static ObservableCollection<AthleteType> FilterSurname(
          ObservableCollection<AthleteType> originalCollection,
          string letter)
        {
            return AthleteCollectionFilter.FilterCollection(
              originalCollection,
              letter,
              AthleteCollectionFilter.FirstLetterSurname);
        }

        /// <summary>
        /// Return a collection of <see cref="AthleteType"/> objects where the 
        /// <paramref name="searchString"/> is contained within each of the returned values.
        /// If <paramref name="searchString"/> is empty, everything is returned.
        /// </summary>
        /// <param name="originalCollection">collection to search</param>
        /// <param name="searchString">search parameter</param>
        /// <returns>filtered collection</returns>
        public static ObservableCollection<AthleteType> SearchName(
          ObservableCollection<AthleteType> originalCollection,
          string searchString)
        {
            return AthleteCollectionFilter.FilterCollection(
              originalCollection,
              searchString,
              AthleteCollectionFilter.ContainsString);
        }

        /// <summary>
        /// Determine whether the collection should be filtered. If it is, then run the 
        /// <paramref name="searchMethod"/> filter across it, otherwise return the collection whole.
        /// </summary>
        /// <param name="originalCollection">collection to search</param>
        /// <param name="searchString">search parameter</param>
        /// <param name="searchMethod">method used for the search</param>
        /// <returns>filtered collection</returns>
        private static ObservableCollection<AthleteType> FilterCollection(
          ObservableCollection<AthleteType> originalCollection,
          string searchString,
          Func<ObservableCollection<AthleteType>, string, ObservableCollection<AthleteType>> searchMethod)
        {
            if (originalCollection?.Count == 0)
            {
                return new ObservableCollection<AthleteType>();
            }

            if (string.IsNullOrWhiteSpace(searchString) || searchString.Length < 1)
            {
                return originalCollection;
            }

            return searchMethod.Invoke(
              originalCollection,
              searchString);
        }

        /// <summary>
        /// Filter the collection of <paramref name="originalCollection"/> and return only those
        /// athletes where the name contains the <paramref name="searchString"/>.
        /// </summary>
        /// <param name="originalCollection">collection to search</param>
        /// <param name="searchString">string to search for</param>
        /// <returns>filtered collection</returns>
        private static ObservableCollection<AthleteType> ContainsString(
        ObservableCollection<AthleteType> originalCollection,
        string searchString)
        {
            ObservableCollection<AthleteType> returnList = new ObservableCollection<AthleteType>();

            foreach (AthleteType athlete in originalCollection)
            {
                if (athlete.Forename.Contains(searchString) ||
                    athlete.FamilyName.Contains(searchString))
                {
                    returnList.Add(athlete);
                }
            }

            return returnList;
        }

        /// <summary>
        /// Filter the collection of <paramref name="originalCollection"/> and return only those
        /// athletes where the surname starts with the <paramref name="letter"/> letter.
        /// </summary>
        /// <param name="originalCollection">collection to search</param>
        /// <param name="letter">letter to search for</param>
        /// <returns>filtered collection</returns>
        private static ObservableCollection<AthleteType> FirstLetterSurname(
          ObservableCollection<AthleteType> originalCollection,
          string letter)
        {
            ObservableCollection<AthleteType> returnList = new ObservableCollection<AthleteType>();

            foreach (AthleteType athlete in originalCollection)
            {
                string surnameFirstLetter = athlete.FamilyName.Substring(0, 1);

                if (string.Compare(surnameFirstLetter, letter) == 0)
                {
                    returnList.Add(athlete);
                }
            }

            return returnList;
        }
    }
}