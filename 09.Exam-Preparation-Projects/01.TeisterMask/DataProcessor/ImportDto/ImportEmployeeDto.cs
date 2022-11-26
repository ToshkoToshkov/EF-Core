using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TeisterMask.DataProcessor.ImportDto
{
    public class ImportEmployeeDto
    {
        [Required]
        [MinLength(Common.Validations.USERNAME_MIN_LENGTH)]
        [MaxLength(Common.Validations.VALIDATE_USERNAME_LENGTH)]
        [RegularExpression(Common.Validations.USERNAME_REGEX)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression(Common.Validations.EMPLOYEE_PHONE_REGEX)]
        public string Phone { get; set; }

        public int[] Tasks { get; set; }

    }
}
