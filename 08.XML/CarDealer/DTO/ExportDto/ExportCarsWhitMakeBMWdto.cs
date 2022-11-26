using System.Xml.Serialization;

namespace CarDealer.DTO.ExportDto
{
    [XmlType("car")]
    public class ExportCarsWhitMakeBMWdto
    {
        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlAttribute("model")]
        public string Model { get; set; }

        [XmlAttribute("travelled-distance")]
        public string TraveledDistance { get; set; }

    }
}
