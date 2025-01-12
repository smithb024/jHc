namespace CommonHandicapLib.Helpers
{
    using System.Linq;

    /// <summary>
    /// Helper class for names.
    /// </summary>
    public static class NameHelper
    {
        /// <summary>
        /// Returns the forename from the given name
        /// </summary>
        /// <param name="name">athlete name</param>
        /// <returns>athlete forename</returns>
        public static string GetForename(string name)
        {
            if (name.Contains(' '))
            {
                return name.Substring(0, name.LastIndexOf(' '));
            }
            else
            {
                return name;
            }
        }

        /// <summary>
        /// Returns the surname from the given name
        /// </summary>
        /// <param name="name">athlete name</param>
        /// <returns>athlete surname</returns>
        public static string GetSurname(string name)
        {
            if (name.Contains(' '))
            {
                return name.Substring(name.LastIndexOf(' ') + 1);
            }
            else
            {
                return name;
            }
        }
    }
}
