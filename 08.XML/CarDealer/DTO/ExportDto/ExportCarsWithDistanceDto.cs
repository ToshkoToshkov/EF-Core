using System;
using System.Xml.Serialization;

namespace CarDealer.DTO.ExportDto
{
    [XmlType("car")]
    public class ExportCarsWithDistanceDto

    {
        [XmlElement("make")]
        public String Make { get; set; }

        [XmlElement("model")]
        public string Model { get; set; }

        [XmlElement("travelled-distance")]
        public string TraveledDistance { get; set; }
    }
}
