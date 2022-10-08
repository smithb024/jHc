namespace CommonHandicapLib.XML.AthleteData
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    /// <summary>
    /// Serialiseable class. This provides the base class for the results table.
    /// </summary>
    [XmlRoot("AthleteDetails")]
    public class AthleteDetailsRoot : List<AthleteList>
    {
    }
}