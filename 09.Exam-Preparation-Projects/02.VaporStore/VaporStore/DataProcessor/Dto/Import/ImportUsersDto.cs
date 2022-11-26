using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VaporStore.DataProcessor.Dto.Import
{
    public class ImportUsersDto
    {
       
        [Required]
        [MinLength(Common.Validations.USER_USERNAME_MIN_LENGTH)]
        [MaxLength(Common.Validations.USER_USERNAME_MAX_LENGTH)]
        public string Username { get; set; }

        [Required]
        [RegularExpression(Common.Validations.USER_FULLNAME_REGEX)]
        public string FullName { get; set; }

        [Required]
        public string Email { get; set; }

        [Range(Common.Validations.USER_AGE_MIN_VALUE, Common.Validations.USER_AGE_MAX_VALUE)]
        public int Age { get; set; }

        [MinLength(1)]
        public ImportUsersCardsDto[] Cards { get; set; }
    }
}
