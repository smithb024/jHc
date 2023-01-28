namespace CommonHandicapLib.XML.AthleteData
{
    using System.Xml.Serialization;

    /// <summary>
    /// XML serialisable class which contains all the numbers associated with a specific athlete.
    /// </summary>
    public class AthleteDataRunningNumbers
    {
        /// <summary>
        /// All the numbers associated with the athlete.
        /// </summary>
        private AthleteDataNumbers numbers;

        /// <summary>
        /// Gets or sets all the numbers associated with the athlete.
        /// </summary>
        [XmlElement("number")]
        public AthleteDataNumbers Numbers
        {
            get
            {
                return this.numbers;
            }
            set
            {
                this.numbers = value;
            }
        }
    }
}
