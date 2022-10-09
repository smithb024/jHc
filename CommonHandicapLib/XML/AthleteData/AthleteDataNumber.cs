namespace CommonHandicapLib.XML.AthleteData
{
    using System.Xml.Serialization;

    /// <summary>
    /// A XML row in the athlete data file. This presents a registered number for a specific athlete.
    /// </summary>
    public class AthleteDataNumber
    {
        /// <summary>
        /// The athlete race number.
        /// </summary>
        private string number;

        /// <summary>
        /// Gets or sets the registered number of the athlete.
        /// </summary>
        [XmlAttribute("no")]
        public string Number
        {
            get
            {
                return this.number;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    this.number = string.Empty;
                }
                else
                {
                    this.number = value;
                }
            }
        }
    }
}
