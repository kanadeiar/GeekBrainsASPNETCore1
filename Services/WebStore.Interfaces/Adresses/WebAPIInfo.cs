namespace WebStore.Interfaces.Adresses
{
    /// <summary> Ссылки на адреса апи контроллера </summary>
    public static class WebAPIInfo
    {
        /// <summary> Адрес апи тестовых значений </summary>
        public const string ApiValue = "Api/Value";
        /// <summary> Адрес апи сотрудников </summary>
        public const string ApiWorker = "Api/Worker";
        /// <summary> Адрес апи товаров </summary>
        public const string ApiProduct = "Api/Product";
        /// <summary> Адрес апи заказов </summary>
        public const string ApiOrder = "Api/Order";

        public static class Identity
        {
            /// <summary> Адрес апи пользователей </summary>
            public const string ApiUser = "Api/User";
            /// <summary> Адрес апи ролей пользователей </summary>
            public const string ApiRole = "Api/Role";
        }
    }
}
