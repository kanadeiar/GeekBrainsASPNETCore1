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

        public async Task<IViewComponentResult> InvokeAsync(string SectionId)
        {
            var sectionId = int.TryParse(SectionId, out var id) ? id : (int?)null;
            int? parentSectionId = null;

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
                    if (child.Id == sectionId)
                        parentSectionId = child.ParentId;

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

            ViewBag.SectionId = sectionId;
            ViewBag.ParentSectionId = parentSectionId;

            return View(patentsViews);
        }
    }
}
