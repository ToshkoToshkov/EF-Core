using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace SoftJail.DataProcessor.ImportDto
{
    [XmlType("Officer")]
    public class ImportOfficerDto
    {
        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Money")]
        public string Money { get; set; }

        [XmlElement("Position")]
        public string Position { get; set; }

        [XmlElement("Weapon")]
        public string Weapon { get; set; }

        [XmlElement("DepartmentId")]
        public string DepartmentId { get; set; }

        [XmlArray("Prisoners")]
        public ImportOfficerPrisonerDto[] Prisoners { get; set; }

    }
}
