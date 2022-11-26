namespace TeisterMask.Common
{
    public class Validations
    {
        //Employees
        public const int USERNAME_MIN_LENGTH = 3;
        public const int VALIDATE_USERNAME_LENGTH = 40;
        public const int VALIDATE_PHONE_LENGTH = 12;
        public const string USERNAME_REGEX = @"^[A-Za-z0-9]+$";
        public const string EMPLOYEE_PHONE_REGEX = @"^\d{3}\-\d{3}\-\d{4}$";

        //Projects
        public const int VALIDATE_NAME_LENGTH = 40;
        public const int VALIDATE_NAME_MIN_LENGTH = 2;

        //Tasks
        public const int VALIDATE_NAME_TASK_LENGTH = 40;
        public const int VALIDATE_NAME_TASK_MIN_LENGTH = 2;

        public const int VALIDATE_EXEC_TYPE_MIN_VALUE = 0;
        public const int VALIDATE_EXEC_TYPE_MAX_VALUE = 3;

        public const int VALIDATE_LABEL_TYPE_MIN_VALUE = 0;
        public const int VALIDATE_LABEL_TYPE_MAX_VALUE = 4;

    }
}
