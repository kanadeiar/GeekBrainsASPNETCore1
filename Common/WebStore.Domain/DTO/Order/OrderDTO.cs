using System;
using System.Collections.Generic;

namespace WebStore.Domain.DTO.Order
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public DateTime DateTime { get; set; }
        public IEnumerable<OrderItemDTO> Items { get; set; }
    }
}
