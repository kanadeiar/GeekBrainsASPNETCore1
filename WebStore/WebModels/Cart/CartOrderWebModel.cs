namespace WebStore.WebModels.Cart
{
    /// <summary> Веб модель совместная </summary>
    public class CartOrderWebModel
    {
        public CartWebModel Cart { get; set; }
        public CreateOrderViewModel CreateOrder { get; set; }
    }
}
