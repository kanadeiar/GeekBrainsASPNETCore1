﻿namespace WebStore.WebModels.UserProfile
{
    public class UserOrderWebModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public int Count { get; set; }
        public decimal PriceSum { get; set; }
    }
}