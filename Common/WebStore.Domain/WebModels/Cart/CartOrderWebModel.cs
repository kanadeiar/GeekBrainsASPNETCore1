namespace WebStore.Domain.WebModels.Cart
{
    /// <summary> Веб модель совместная корзины товаров и создания заказа </summary>
    public class CartOrderWebModel
    {
        public CartWebModel Cart { get; set; }
        public CreateOrderWebModel Order { get; set; } = new ();
    }
}
