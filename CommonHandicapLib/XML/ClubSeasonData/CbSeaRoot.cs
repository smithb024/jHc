namespace CommonHandicapLib.XML.ClubSeasonData
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    /// <summary>
    /// Serialiseable class. This provides the base class for the club data (season) file.
    /// </summary>
    [XmlRoot("CbSea")]
    public class CbSeaRoot : List<Club>
    {
    }
}
