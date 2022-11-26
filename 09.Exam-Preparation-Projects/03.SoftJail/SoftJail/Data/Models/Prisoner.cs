using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftJail.Data.Models
{
    public class Prisoner
    {
        public Prisoner()
        {
            this.Mails = new HashSet<Mail>();
            this.PrisonerOfficers = new HashSet<OfficerPrisoner>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(Common.Validations.FULLNAME_MAX_LENGTH, MinimumLength = 3)]
        public string FullName { get; set; }

        [Required]
        [RegularExpression(@"^The [A-Z][a-z]+$")]
        public string Nickname { get; set; }

        [Range(18,65)]
        public int Age { get; set; }

        public DateTime IncarcerationDate { get; set; }

        public DateTime? ReleaseDate { get; set; }

        [Range((Double)Common.Validations.BAIL_MIN_VALUE, (double)Common.Validations.BAIL_MAX_VALUE)]
        public decimal? Bail { get; set; }

        public int? CellId { get; set; }
        [ForeignKey(nameof(CellId))]
        public virtual Cell Cell { get; set; }


        public virtual ICollection<Mail> Mails { get; set; }

        public virtual ICollection<OfficerPrisoner> PrisonerOfficers { get; set; }

    }
}