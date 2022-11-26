using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace TeisterMask.DataProcessor.ImportDto
{
    [XmlType("Task")]
    public class ImportTaskSubDto
    {
        [XmlElement("Name")]
        [Required]
        [MinLength(Common.Validations.VALIDATE_NAME_TASK_MIN_LENGTH)]
        [MaxLength(Common.Validations.VALIDATE_NAME_TASK_LENGTH)]
        public string Name { get; set; }

        [XmlElement("OpenDate")]
        [Required]
        public string TaskOpenDate { get; set; }

        [XmlElement("DueDate")]
        [Required]
        public string TaskDueDate { get; set; }

        [XmlElement("ExecutionType")]
        [Range(Common.Validations.VALIDATE_EXEC_TYPE_MIN_VALUE, Common.Validations.VALIDATE_EXEC_TYPE_MAX_VALUE)]
        public int ExecutionType { get; set; }

        [XmlElement("LabelType")]
        [Range(Common.Validations.VALIDATE_LABEL_TYPE_MIN_VALUE, Common.Validations.VALIDATE_LABEL_TYPE_MAX_VALUE)]
        public int LabelType { get; set; }

    }
}
