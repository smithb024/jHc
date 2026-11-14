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
        /// Gets or sets the total number of athletes in the event..
        /// </summary>
        [XmlAttribute("TotalRunner")]
        public int TotalRunners
        {
            get => this.totalAthletes;
            set => this.totalAthletes = value;
        }

        /// <summary>
        /// Gets or sets the total number of male athletes in the event.
        /// </summary>
        [XmlAttribute("MaleRunners")]
        public int MaleRunners
        {
            get => this.totalMaleAthletes;
            set => this.totalMaleAthletes = value;
        }

        /// <summary>
        /// Gets or sets the total number of female athletes in the event.
        /// </summary>
        [XmlAttribute("FemaleRunners")]
        public int FemaleRunners
        {
            get => this.totalFemaleAthletes;
            set => this.totalFemaleAthletes = value;
        }

        /// <summary>
        /// Gets or sets the total number of YBs run in the event.
        /// </summary>
        [XmlAttribute("YBs")]
        public int TotalYBs
        {
            get => this.totalYBs;
            set => this.totalYBs = value;
        }

        /// <summary>
        /// Gets or sets the total number of PBs run in the event.
        /// </summary>
        [XmlAttribute("PBs")]
        public int TotalPBs
        {
            get => this.totalPBs;
            set => this.totalPBs = value;
        }

        /// <summary>
        /// Gets or sets the total number of first timers to run in the event.
        /// </summary>
        [XmlAttribute("FirstTimers")]
        public int TotalFirstTimers
        {
            get => this.totalFirstTimers;
            set => this.totalFirstTimers = value;
        }
    }
}
