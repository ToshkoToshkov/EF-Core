using System;
using System.Collections.Generic;
using System.Text;

namespace SoftJail.Common
{
    public class Validations
    {
        //Prisoners
        public const int FULLNAME_MIN_LENGTH = 3;
        public const int FULLNAME_MAX_LENGTH = 20;

        public const int AGE_MIN_VALUE = 18;
        public const int AGE_MAX_VALUE = 65;

        public const decimal BAIL_MIN_VALUE = 0;
        public const decimal BAIL_MAX_VALUE = decimal.MaxValue;

        public const string REGEX_VALIDATION = @"^ The[A - Z][a - z] + $";

        //officer
        public const int OFFICER_FULLNAME_MIN_LENGTH = 3;
        public const int OFFICER_FULLNAME_MAX_LENGTH = 30;

     

        //Cell
        public const int CELL_NUMBER_MIN_VALUE = 1;
        public const int CELL_NUMBER_MAX_VALUE = 1000;

        //Department
        public const int DEPARTMENT_NAME_MIN_LENGTH = 3;
        public const int DEPARTMENT_NAME_MAX_LENGTH = 25;

    }
}
