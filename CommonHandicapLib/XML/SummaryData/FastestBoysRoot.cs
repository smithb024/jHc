namespace CommonHandicapLib.XML.SummaryData
{
    using System.Xml.Serialization;

    /// <summary>
    /// The fastest boy's times.
    /// </summary>
    public class FastestBoysRoot
    {
        /// <summary>
        /// All the fastest times.
        /// </summary>
        private FastestTimes times;

        /// <summary>
        /// All fastest times.
        /// </summary>
        [XmlElement("Time")]
        public FastestTimes Appearances
        {
            get => this.times;
            set => this.times = value;
        }
    }
}
