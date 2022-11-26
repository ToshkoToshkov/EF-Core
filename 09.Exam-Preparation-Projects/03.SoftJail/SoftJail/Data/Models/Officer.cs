using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SoftJail.Data.Models
{
    public class Officer
    {
        public Officer()
        {
            this.OfficerPrisoners = new HashSet<OfficerPrisoner>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(Common.Validations.OFFICER_FULLNAME_MAX_LENGTH)]
        public string FullName { get; set; }

        //[Range(typeof(Decimal), "0", "79228162514264337593543950335")]
        public decimal Salary { get; set; }

        public Enums.Position Position { get; set; }

        public Enums.Weapon Weapon { get; set; }

        [ForeignKey(nameof(Department))]
        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }

        public virtual ICollection<OfficerPrisoner> OfficerPrisoners { get; set; }

    }
}
