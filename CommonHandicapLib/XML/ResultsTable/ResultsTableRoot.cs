namespace CommonHandicapLib.XML.ResultsTable
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [XmlRoot("ResTbl")]
    public class ResultsTableRoot : List<Row>
    {
    }
}