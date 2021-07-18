namespace WebStore.Domain.WebModels.Cart
{
    /// <summary> Веб модель совместная корзины товаров и создания заказа </summary>
    public class CartOrderWebModel
    {
        /// <summary> Корзина товаров </summary>
        public CartWebModel Cart { get; set; }
        /// <summary> Заказ пользователя </summary>
        public CreateOrderWebModel Order { get; set; } = new CreateOrderWebModel();
    }
}
