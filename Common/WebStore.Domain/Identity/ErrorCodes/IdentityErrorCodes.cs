namespace WebStore.Domain.Identity.ErrorCodes
{
    /// <summary> Расшифровки ошибок системы Identity </summary>
    public class IdentityErrorCodes
    {
        /// <summary> По умолчанию </summary>
        public const string DefaultError                    = "DefaultError";
        /// <summary> Конкурентный доступ </summary>
        public const string ConcurrencyFailure              = "ConcurrencyFailure";
        /// <summary> Пароль </summary>
        public const string PasswordMismatch                = "PasswordMismatch";
        /// <summary> Токен </summary>
        public const string InvalidToken                    = "InvalidToken";
        /// <summary> Логин уже есть </summary>
        public const string LoginAlreadyAssociated          = "LoginAlreadyAssociated";
        /// <summary> Плохое имя </summary>
        public const string InvalidUserName                 = "InvalidUserName";
        /// <summary> Плохая почта </summary>
        public const string InvalidEmail                    = "InvalidEmail";
        /// <summary> Такой уже есть логин </summary>
        public const string DuplicateUserName               = "DuplicateUserName";
        /// <summary> Такая почта уже есть </summary>
        public const string DuplicateEmail                  = "DuplicateEmail";
        /// <summary> Неверная роль </summary>
        public const string InvalidRoleName                 = "InvalidRoleName";
        /// <summary> Такая роль уже есть </summary>
        public const string DuplicateRoleName               = "DuplicateRoleName";
        /// <summary> Уже есть пароль </summary>
        public const string UserAlreadyHasPassword          = "UserAlreadyHasPassword";
        /// <summary> Блокировка отсутствует </summary>
        public const string UserLockoutNotEnabled           = "UserLockoutNotEnabled";
        /// <summary> Уже назначена роль </summary>
        public const string UserAlreadyInRole               = "UserAlreadyInRole";
        /// <summary> Без роли </summary>
        public const string UserNotInRole                   = "UserNotInRole";
        /// <summary> Слишком короткий пароль </summary>
        public const string PasswordTooShort                = "PasswordTooShort";
        /// <summary> Пароль требует спецсимволов </summary>
        public const string PasswordRequiresNonAlphanumeric = "PasswordRequiresNonAlphanumeric";
        /// <summary> Пароль требует цифр </summary>
        public const string PasswordRequiresDigit           = "PasswordRequiresDigit";
        /// <summary> Пароль требует букв маленьких </summary>
        public const string PasswordRequiresLower           = "PasswordRequiresLower";
        /// <summary> Пароль требует букв больших </summary>
        public const string PasswordRequiresUpper           = "PasswordRequiresUpper";
        /// <summary> Все ошибки </summary>
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
        /// <summary> Получение расшифровки ошибки </summary>
        /// <param name="code">Код ошибки</param>
        /// <returns>Расшифровка</returns>
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
