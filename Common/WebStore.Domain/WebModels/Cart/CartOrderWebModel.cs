namespace WebStore.Domain.WebModels.Cart
{
    /// <summary> Веб модель совместная </summary>
    public class CartOrderWebModel
    {
        public CartWebModel Cart { get; set; }
        public CreateOrderWebModel Order { get; set; } = new ();
    }
}
