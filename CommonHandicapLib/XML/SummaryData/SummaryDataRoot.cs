namespace CommonHandicapLib.XML.SummaryData
{
    using System.Xml.Serialization;

    /// <summary>
    /// Serialiseable class. This provides the base class for the summary details files.
    /// </summary>
    [XmlRoot("SummaryDetails")]
    public class SummaryDataRoot
    {
        /// <summary>
        /// The total number of athletes in the event.
        /// </summary>
        private int totalAthletes;

        /// <summary>
        /// The total number of male athletes in the event.
        /// </summary>
        private int totalMaleAthletes;

        /// <summary>
        /// The total number of female athletes in the event.
        /// </summary>
        private int totalFemaleAthletes;

        /// <summary>
        /// The total number of PBs run in the event.
        /// </summary>
        private int totalPBs;

        /// <summary>
        /// The total number of YBs run in the event.
        /// </summary>
        private int totalYBs;

        /// <summary>
        /// The total number of first timers to run in the event.
        /// </summary>
        private int totalFirstTimers;

        /// <summary>
        /// The fastest runners.
        /// </summary>
        private FastestRunnersRoot fastestRunners;

        /// <summary>
        /// Gets or sets the total number of athletes in the event..
        /// </summary>
        [XmlElement("TotalRunner")]
        public int TotalRunners
        {
            get => this.totalAthletes;
            set => this.totalAthletes = value;
        }

        /// <summary>
        /// Gets or sets the total number of male athletes in the event.
        /// </summary>
        [XmlElement("MaleRunners")]
        public int MaleRunners
        {
            get => this.totalMaleAthletes;
            set => this.totalMaleAthletes = value;
        }

        /// <summary>
        /// Gets or sets the total number of female athletes in the event.
        /// </summary>
        [XmlElement("FemaleRunners")]
        public int FemaleRunners
        {
            get => this.totalFemaleAthletes;
            set => this.totalFemaleAthletes = value;
        }

        /// <summary>
        /// Gets or sets the total number of YBs run in the event.
        /// </summary>
        [XmlElement("YBs")]
        public int TotalYBs
        {
            get => this.totalYBs;
            set => this.totalYBs = value;
        }

        /// <summary>
        /// Gets or sets the total number of PBs run in the event.
        /// </summary>
        [XmlElement("PBs")]
        public int TotalPBs
        {
            get => this.totalPBs;
            set => this.totalPBs = value;
        }

        /// <summary>
        /// Gets or sets the total number of first timers to run in the event.
        /// </summary>
        [XmlElement("FirstTimers")]
        public int TotalFirstTimers
        {
            get => this.totalFirstTimers;
            set => this.totalFirstTimers = value;
        }

        /// <summary>
        /// Gets or sets the fastest runners in the summary.
        /// </summary>
        [XmlElement("FastestRunners")]
        public FastestRunnersRoot FastestRunners
        {
            get => this.fastestRunners;
            set => this.fastestRunners = value;
        }
    }
}
