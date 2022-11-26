using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VaporStore.Data.Models
{
    public class User
    {
        public User()
        {
            this.Cards = new HashSet<Card>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(Common.Validations.USER_USERNAME_MAX_LENGTH)]
        public string Username { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string Email { get; set; }

        [Range(Common.Validations.USER_AGE_MIN_VALUE, Common.Validations.USER_AGE_MAX_VALUE)]
        public int Age { get; set; }

        public virtual ICollection<Card> Cards { get; set; }

    }
}
