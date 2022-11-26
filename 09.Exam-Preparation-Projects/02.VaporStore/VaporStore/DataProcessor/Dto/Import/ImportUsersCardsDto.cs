using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VaporStore.DataProcessor.Dto.Import
{
    public class ImportUsersCardsDto
    {
        [Required]
        [RegularExpression(Common.Validations.CARD_NUMBER_VERIFICATION_REGEX)]
        public string Number { get; set; }

        [Required]
        [RegularExpression(Common.Validations.CARD_CVCvERIFICATION_REGEX)]
        public string Cvc { get; set; }

        [Required]
        public string Type { get; set; }

    }
}
