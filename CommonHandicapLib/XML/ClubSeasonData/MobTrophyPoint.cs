namespace CommonHandicapLib.XML.ClubSeasonData
{
    using System.Xml.Serialization;

    /// <summary>
    /// A XML row in the club season data file. This presents the points scored by the club.
    /// </summary>
    public class MobTrophyPoint
    {
        /// <summary>
        /// The points scored.
        /// </summary>
        private int points;

        /// <summary>
        /// The unique id of the athlete scoring the points.
        /// </summary>
        private int key;

        /// <summary>
        /// Gets or sets the points scored.
        /// </summary>
        [XmlAttribute("pt")]
        public int Points
        {
            get => this.points;
            set => this.points = value;
        }

        /// <summary>
        /// Gets or sets the key of the points scoring athlete.
        /// </summary>
        [XmlAttribute("key")]
        public int Key
        {
            get => this.key;
            set => this.key= value;
        }
    }
}
