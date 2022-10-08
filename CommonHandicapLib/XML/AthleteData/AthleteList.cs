namespace CommonHandicapLib.XML.AthleteData
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Simple object which represents a collection of athletes.
    /// </summary>
    public class AthleteList
    {
        private AthleteCollection colours;

        public AthleteList()
        {
            this.AllAthletes = new AthleteCollection();
        }

        [XmlAttribute("Model")]
        public string Model
        {
            get
            {
                return "my string";
            }
            set
            {

            }
        }

        [XmlElement("Athlete")]
        public AthleteCollection AllAthletes
        {
            get
            {
                return this.colours;
            }
            set
            {
                this.colours = value;
            }
        }
    }
}
