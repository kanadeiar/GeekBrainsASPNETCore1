namespace WebStore.Domain.Entities
{
    /// <summary> Элемент корзины </summary>
    public class CartItem
    {
        /// <summary> Товар этого элемента </summary>
        public int ProductId { get; set; }
        /// <summary> Количество </summary>
        public int Quantity { get; set; } = 1;
    }
}