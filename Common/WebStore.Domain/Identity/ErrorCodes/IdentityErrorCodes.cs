namespace WebStore.Domain.Identity.ErrorCodes
{
    public class IdentityErrorCodes
    {
        public const string DefaultError                    = "DefaultError";
        public const string ConcurrencyFailure              = "ConcurrencyFailure";
        public const string PasswordMismatch                = "PasswordMismatch";
        public const string InvalidToken                    = "InvalidToken";
        public const string LoginAlreadyAssociated          = "LoginAlreadyAssociated";
        public const string InvalidUserName                 = "InvalidUserName";
        public const string InvalidEmail                    = "InvalidEmail";
        public const string DuplicateUserName               = "DuplicateUserName";
        public const string DuplicateEmail                  = "DuplicateEmail";
        public const string InvalidRoleName                 = "InvalidRoleName";
        public const string DuplicateRoleName               = "DuplicateRoleName";
        public const string UserAlreadyHasPassword          = "UserAlreadyHasPassword";
        public const string UserLockoutNotEnabled           = "UserLockoutNotEnabled";
        public const string UserAlreadyInRole               = "UserAlreadyInRole";
        public const string UserNotInRole                   = "UserNotInRole";
        public const string PasswordTooShort                = "PasswordTooShort";
        public const string PasswordRequiresNonAlphanumeric = "PasswordRequiresNonAlphanumeric";
        public const string PasswordRequiresDigit           = "PasswordRequiresDigit";
        public const string PasswordRequiresLower           = "PasswordRequiresLower";
        public const string PasswordRequiresUpper           = "PasswordRequiresUpper";
        public static string[] All = { 
            DefaultError,
            ConcurrencyFailure,
            PasswordMismatch,
            InvalidToken,
            LoginAlreadyAssociated,
            InvalidUserName,
            InvalidEmail,
            DuplicateUserName,
            DuplicateEmail,
            InvalidRoleName,
            DuplicateRoleName,
            UserAlreadyHasPassword,
            UserLockoutNotEnabled,
            UserAlreadyInRole,
            UserNotInRole,
            PasswordTooShort,
            PasswordRequiresNonAlphanumeric,
            PasswordRequiresDigit,
            PasswordRequiresLower,
            PasswordRequiresUpper 
        };
        public static string GetDescription(string code)
        {
            string description;
            switch (code)
            {
                case DefaultError:
                    description = "Произошла неизвестная ошибка";
                    break;
                case ConcurrencyFailure:
                    description = "Ошибка параллельного доступа, объект был изменен";
                    break;
                case PasswordMismatch:
                    description = "Неверный пароль";
                    break;
                case InvalidToken:
                    description = "Неверный токен";
                    break;
                case LoginAlreadyAssociated:
                    description = "Пользователь с таким логином уже существует";
                    break;
                case InvalidUserName:
                    description = "Имя пользователя недействительно, может содержать только буквы и цифры";
                    break;
                case InvalidEmail:
                    description = "Недействительный адрес электронной почты";
                    break;
                case DuplicateUserName:
                    description = "Имя пользователя уже занято";
                    break;
                case DuplicateEmail:
                    description = "Адрес электронной почты уже занят";
                    break;
                case InvalidRoleName:
                    description = "Имя роли недействительно";
                    break;
                case DuplicateRoleName:
                    description = "Имя роли уже занято";
                    break;
                case UserAlreadyHasPassword:
                    description = "У пользователя уже установлен пароль";
                    break;
                case UserLockoutNotEnabled:
                    description = "Для этого пользователя отключена блокировка";
                    break;
                case UserAlreadyInRole:
                    description = "Пользователю уже назначена эта роль";
                    break;
                case UserNotInRole:
                    description = "Пользователю не назначена необходимая роль";
                    break;
                case PasswordTooShort:
                    description = "Слишком короткий пароль";
                    break;
                case PasswordRequiresNonAlphanumeric:
                    description = "Пароль должен содержать хотябы один не буквенно-цифровой символ";
                    break;
                case PasswordRequiresDigit:
                    description = "Пароль должен содержать хотя бы одну цифру (0-9)";
                    break;
                case PasswordRequiresLower:
                    description = "Пароль должен содержать хотя бы один символ в нижнем регистре (a-z)";
                    break;
                case PasswordRequiresUpper:
                    description = "Пароль должен содержать хотя бы один символ в верхнем регистре (A-Z)";
                    break;
                default:
                    description = "Произошла неизвестная ошибка";
                    break;
            }

            return description;
        }
    }
}
