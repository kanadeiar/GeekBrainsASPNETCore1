using System.ComponentModel.DataAnnotations;

namespace WebStore.WebModels.Cart
{
    /// <summary> Веб модель создания заказа </summary>
    public class CreateOrderWebModel
    {
        [Display(Name = "Название")]
        [Required(ErrorMessage = "Нужно ввести название заказа")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Длинна названия заказа должна быть от 2 до 100 символов")]
        public string Name { get; set; }

        [Display(Name = "Телефон")]
        [Required(ErrorMessage = "Нужно ввести свой номер, иначе как с вами связаться?")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Длинна телефона должна быть от 2 до 50 символов")]
        public string Phone { get; set; }

        [Display(Name = "Адрес")]
        [Required(ErrorMessage = "Нужно обязательно ввести адрес, куда доставить товары")]
        [StringLength(500, ErrorMessage = "Длинна адреса доставки не может превышать 500 символов")]
        public string Address { get; set; }
    }
}
