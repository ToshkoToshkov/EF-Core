using System;
using System.Collections.Generic;
using System.Text;

namespace VaporStore.Common
{
    public class Validations
    {
        //Users
        public const int USER_USERNAME_MIN_LENGTH = 3;
        public const int USER_USERNAME_MAX_LENGTH = 20;

        public const int USER_AGE_MIN_VALUE = 3;
        public const int USER_AGE_MAX_VALUE = 103;

        public const string USER_FULLNAME_REGEX = @"^[A-Z][a-z]+\s[A-Z][a-z]+$";

        //Games
        public const int GAME_PRICE_MIN_VALUE = 0;
        public const int GAME_TAGS_MIN_VALUE = 1;

        //Cards
        public const string CARD_NUMBER_VERIFICATION_REGEX 
            = @"^\d{4}\s\d{4}\s\d{4}\s\d{4}$";

        public const string CARD_CVCvERIFICATION_REGEX = @"^\d{3}$";


    }
}
