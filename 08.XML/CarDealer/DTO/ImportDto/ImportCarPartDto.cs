using System.Xml.Serialization;

namespace CarDealer.DTO.ImportDto
{
    [XmlType("partId")]

    public class ImportCarPartDto
    {
        [XmlAttribute("partId")]
        public int Id { get; set; }
    }
}
