using Microsoft.AspNetCore.Mvc;
using SimpleMvcSitemap;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Interfaces.Services;

namespace WebStore.Controllers.API
{
    public class SiteMapController : ControllerBase
    {
        public async Task<IActionResult> Index([FromServices] IProductData productData)
        {
            var nodes = new List<SitemapNode>()
            {
                new (Url.Action("Index", "Home")),
                new (Url.Action("ProductDetails", "Home")),
                new (Url.Action("Checkout", "Home")),
                new (Url.Action("Blog", "Home")),
                new (Url.Action("BlogSingle", "Home")),
                new (Url.Action("ContactUs", "Home")),
                new (Url.Action("Second", "Home")),
                new (Url.Action("Index", "Catalog")),
                new (Url.Action("Details", "Catalog")),
                new (Url.Action("Index", "WebAPI")),
            };
            var sectionsNodes = (await productData.GetSections()).Select(s =>
                new SitemapNode(Url.Action("Index", "Catalog", new {SectionId = s.Id})));
            nodes.AddRange(sectionsNodes);
            foreach (var brand in await productData.GetBrands())
            {
                var brandNode = new SitemapNode(Url.Action("Index", "Catalog", new {BrandId = brand.Id}));
                nodes.Add(brandNode);
            }
            foreach (var product in await productData.GetProducts())
            {
                var brandNode = new SitemapNode(Url.Action("Details", "Catalog", new {product.Id}));
                nodes.Add(brandNode);
            }
            return new SitemapProvider().CreateSitemap(new SitemapModel(nodes));
        }
    }
}
