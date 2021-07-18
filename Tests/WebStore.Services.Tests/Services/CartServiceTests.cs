using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebStore.Domain.Entities;
using WebStore.Domain.WebModels;
using WebStore.Domain.WebModels.Cart;

namespace WebStore.Services.Tests.Services
{
    [TestClass]
    public class CartServiceTests
    {
        private Cart _cart;

        [TestInitialize]
        public void Initialize()
        {
            _cart = new Cart
            {
                Items = Enumerable.Range(1, 5).Select(i => new CartItem()
                {
                    ProductId = i,
                    Quantity = i,
                }).ToList()
            };
        }






    }
}
