namespace CommonHandicapLib.XML.ResultsTable
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    /// <summary>
    /// Serialiseable class. This provides the base class for the results table.
    /// </summary>
    [XmlRoot("ResTbl")]
    public class ResultsTableRoot : List<Row>
    {
    }
}