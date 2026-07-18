namespace CommonHandicapLib.XML.AthleteDataSeason
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    /// <summary>
    /// Serialiseable class. This provides the base class for the athlete data (season) file.
    /// </summary>
    [XmlRoot("AtlSea")]
    public class AllSeaRoot : List<EntrantList>
    {
    }
}