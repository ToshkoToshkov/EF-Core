using System;
using System.Xml.Serialization;

namespace CarDealer.DTO.ImportDto
{
    [XmlType("Customer")]
    public class ImportCustumerDto
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("birthDate")]
        public DateTime BirthDate { get; set; }

        [XmlElement("isYoungDriver")]
        public string IsYoungDriver { get; set; }
    }
}
