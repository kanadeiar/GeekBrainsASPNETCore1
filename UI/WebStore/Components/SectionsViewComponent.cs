using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.WebModels;
using WebStore.Interfaces.Services;

namespace WebStore.Components
{
    public class SectionsViewComponent : ViewComponent
    {
        private readonly IProductData _productData;
        public SectionsViewComponent(IProductData productData)
        {
            _productData = productData;
        }

        public async Task<IViewComponentResult> InvokeAsync(string sectionId)
        {
            var all = await _productData.GetSections();
            
            var parents = all.Where(p => p.ParentId == null);
            var patentsViews = parents.Select(p => new SectionWebModel
            {
                Id = p.Id,
                Name = p.Name,
                Order = p.Order,
                CountProduct = p.Products.Count,
            }).ToList();

            foreach (var patentsView in patentsViews)
            {
                var children = all.Where(c => c.ParentId == patentsView.Id);
                foreach (var child in children)
                {
                    patentsView.Children.Add(new SectionWebModel
                    {
                        Id = child.Id,
                        Name = child.Name,
                        Order = child.Order,
                        Parent = patentsView,
                        CountProduct = child.Products.Count,
                    });
                }
                patentsView.Children.Sort((a, b) => Comparer<int>.Default.Compare(a.Order, b.Order));
            }
            patentsViews.Sort((a, b) => Comparer<int>.Default.Compare(a.Order, b.Order));

            return View(patentsViews);
        }
    }
}
