namespace WebStore.Domain.WebModels.UserProfile
{
    /// <summary> Веб модель данных заказа пользователя </summary>
    public class UserOrderWebModel
    {
        /// <summary> Идентификатор </summary>
        public int Id { get; set; }
        /// <summary> Название </summary>
        public string Name { get; set; }
        /// <summary> Телефон </summary>
        public string Phone { get; set; }
        /// <summary> Адрес </summary>
        public string Address { get; set; }
        /// <summary> Количество </summary>
        public int Count { get; set; }
        /// <summary> Сумма </summary>
        public decimal PriceSum { get; set; }
    }
}
