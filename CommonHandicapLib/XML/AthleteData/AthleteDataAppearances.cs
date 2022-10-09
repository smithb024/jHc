namespace CommonHandicapLib.XML.AthleteData
{
    using System.Xml.Serialization;

    /// <summary>
    /// XML serialisable class which contains all the appearances of a specific athlete.
    /// </summary>
    public class AthleteDataAppearances
    {
        /// <summary>
        /// All appearances for the athlete.
        /// </summary>
        private AthleteDataTimes appearances;

        /// <summary>
        /// All appearances.
        /// </summary>
        [XmlElement("time")]
        public AthleteDataTimes Appearances
        {
            get
            {
                return this.appearances;
            }
            set
            {
                this.appearances = value;
            }
        }
    }
}