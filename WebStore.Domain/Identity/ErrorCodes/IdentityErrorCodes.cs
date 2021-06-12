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
            var description = code switch
            {
                DefaultError => "Произошла неизвестная ошибка",
                ConcurrencyFailure => "Ошибка параллельного доступа, объект был изменен",
                PasswordMismatch => "Неверный пароль",
                InvalidToken => "Неверный токен",
                LoginAlreadyAssociated => "Пользователь с таким логином уже существует",
                InvalidUserName => "Имя пользователя недействительно, может содержать только буквы и цифры",
                InvalidEmail => "Недействительный адрес электронной почты",
                DuplicateUserName => "Имя пользователя уже занято",
                DuplicateEmail => "Адрес электронной почты уже занят",
                InvalidRoleName => "Имя роли недействительно",
                DuplicateRoleName => "Имя роли уже занято",
                UserAlreadyHasPassword => "У пользователя уже установлен пароль",
                UserLockoutNotEnabled => "Для этого пользователя отключена блокировка",
                UserAlreadyInRole => "Пользователю уже назначена эта роль",
                UserNotInRole => "Пользователю не назначена необходимая роль",
                PasswordTooShort => "Слишком короткий пароль",
                PasswordRequiresNonAlphanumeric => "Пароль должен содержать хотябы один не буквенно-цифровой символ",
                PasswordRequiresDigit => "Пароль должен содержать хотя бы одну цифру (0-9)",
                PasswordRequiresLower => "Пароль должен содержать хотя бы один символ в нижнем регистре (a-z)",
                PasswordRequiresUpper => "Пароль должен содержать хотя бы один символ в верхнем регистре (A-Z)", 
                _ => "Произошла неизвестная ошибка",
            };
            return description;
        }
    }
}
