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
            nodes.AddRange((await productData.GetSections())
                .Select(s => new SitemapNode(Url.Action("Index", "Catalog", new { SectionId = s.Id } ))) );
            nodes.AddRange((await productData.GetBrands())
                .Select(b => new SitemapNode(Url.Action("Index", "Catalog", new { BrandId = b.Id } ))) );
            nodes.AddRange((await productData.GetProducts()).Products
                .Select(p => new SitemapNode(Url.Action("Details", "Catalog", new { p.Id } ))) );
            return new SitemapProvider().CreateSitemap(new SitemapModel(nodes));
        }
    }
}
