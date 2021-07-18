using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebStore.Domain.Entities.Base;
using WebStore.Domain.Identity;

namespace WebStore.Domain.Entities.Orders
{
    /// <summary> Заказ пользователя </summary>
    public class Order : NamedEntity
    {
        /// <summary> Пользователь </summary>
        [Required]
        public User User { get; set; }
        /// <summary> Телефон </summary>
        [Required, MaxLength(50)]
        public string Phone { get; set; }
        /// <summary> Адрес доставки </summary>
        [Required, MaxLength(500)]
        public string Address { get; set; }
        /// <summary> Время и дата заказа </summary>
        public DateTime DateTime { get; set; } = DateTime.Now;
        /// <summary> Товары заказа </summary>
        public ICollection<OrderItem> Items { get; set; }
    }
}
