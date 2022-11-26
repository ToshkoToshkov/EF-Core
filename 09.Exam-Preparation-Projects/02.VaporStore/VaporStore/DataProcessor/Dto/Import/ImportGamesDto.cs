using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using VaporStore.Common;
using VaporStore.Data.Models;

namespace VaporStore.DataProcessor.Dto.Import
{
    //"Price": 0,
    //"ReleaseDate": "2013-07-09",
    //"Developer": "Valid Dev",
    //"Genre": "Valid Genre",
    //"Tags": ["Valid Tag"]

    public class ImportGamesDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [Range(0, (double)decimal.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        public string ReleaseDate { get; set; }

        [Required]
        public string Developer { get; set; }

        [Required]
        public string Genre { get; set; }

        [MinLength(Validations.GAME_TAGS_MIN_VALUE)]
        public string[] Tags { get; set; }
    }
}
