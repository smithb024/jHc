namespace CommonHandicapLib.XML.SummaryData
{
    using System.Xml.Serialization;

    /// <summary>
    /// All the fastest runners.
    /// </summary>
    public class FastestRunnersRoot
    {
        /// <summary>
        /// All the fastest boys times.
        /// </summary>
        private FastestBoysRoot boys;

        /// <summary>
        /// All the fastest girls times.
        /// </summary>
        private FastestGirlsRoot girls;

        /// <summary>
        /// All fastest boys times.
        /// </summary>
        [XmlElement("FastestBoys")]
        public FastestBoysRoot Boys
        {
            get => this.boys;
            set => this.boys = value;
        }

        /// <summary>
        /// All fastest girls times.
        /// </summary>
        [XmlElement("FastestGirls")]
        public FastestGirlsRoot Girls
        {
            get => this.girls;
            set => this.girls = value;
        }
    }
}
