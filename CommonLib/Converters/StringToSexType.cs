namespace CommonLib.Converters
{
    using CommonLib.Enumerations;

    public static class StringToSexType
    {
        /// <summary>
        ///   Converts the incoming string to an integer.
        /// </summary>
        /// <param name="inputString">input string</param>
        /// <returns>integer number</returns>
        public static SexType ConvertStringToSexType(string inputString)
        {
            if (inputString == SexType.Male.ToString())
            {
                return SexType.Male;
            }
            else if (inputString == SexType.Female.ToString())
            {
                return SexType.Female;
            }
            else
            {
                return SexType.NotSpecified;
            }
        }
    }
}