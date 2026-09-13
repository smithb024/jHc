namespace CommonHandicapLib.XML.ClubSeasonData
{
    using System.Xml.Serialization;

    /// <summary>
    /// XML serialisable class which contains all the mob trophy scores of a specific athlete.
    /// </summary>
    public class TeamTrophyPointsRoot
    {
        /// <summary>
        /// All event competed in by the club.
        /// </summary>
        private TeamTrophyEventsRoot events;

        /// <summary>
        /// Gets or sets all the events.
        /// </summary>
        [XmlElement("event")]
        public TeamTrophyEventsRoot Events
        {
            get => this.events;
            set => this.events = value;
        }
    }
}
