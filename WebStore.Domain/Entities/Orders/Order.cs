using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebStore.Domain.Entities.Base;
using WebStore.Domain.Identity;

namespace WebStore.Domain.Entities.Orders
{
    public class Order : NamedEntity
    {
        [Required]
        public User User { get; set; }

        [Required, MaxLength(50)]
        public string Phone { get; set; }

        [Required, MaxLength(500)]
        public string Address { get; set; }

        public DateTime DateTime { get; set; } = DateTime.Now;

        public ICollection<OrderItems> Items { get; set; } = new List<OrderItems>();
    }
}
