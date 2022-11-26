using SoftJail.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SoftJail.DataProcessor.ImportDto
{
    public class ImportPrisonerDto
    {
        [Required]
        [MinLength(Common.Validations.FULLNAME_MIN_LENGTH)]
        [MaxLength(Common.Validations.FULLNAME_MAX_LENGTH)]
        public string FullName { get; set; }

        [Required]
        [RegularExpression(Common.Validations.REGEX_VALIDATION)]
        public string Nickname { get; set; }

        [Range(Common.Validations.AGE_MIN_VALUE,Common.Validations.AGE_MAX_VALUE)]
        public int Age { get; set; }

        [Required]
        public string IncarcerationDate { get; set; }

        public string ReleaseDate { get; set; }

        [Range((Double)Common.Validations.BAIL_MIN_VALUE, (double)Common.Validations.BAIL_MAX_VALUE)]
        public decimal? Bail { get; set; }

        public int? CellId { get; set; }

        public ImportPrisonerMailsDto[] Mails { get; set; }
    }
}
