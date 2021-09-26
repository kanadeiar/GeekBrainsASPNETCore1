using System.Collections.Generic;
using System.Linq;
using WebStore.Domain.WebModels.Product;

namespace WebStore.Domain.WebModels.Mappers
{
    /// <summary> Маппер в вебмодель товаров </summary>
    public static class ProductWebModelMapper
    {
        /// <summary> В вебмодель </summary>
        public static ProductWebModel ToWeb(this Entities.Product product)
        {
            return product is null
                ? null
                : new ProductWebModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    Section = product.Section.Name,
                    Brand = product.Brand.Name,
                    ImageUrl = product.ImageUrl,
                    ImageUrls = product.ImageUrls.Select(p => p.Url).ToList(),
                    Price = product.Price,
                    Tags = product.Tags.Select(p => new TagWebModel{ Id = p.Id, Text = p.Text }).ToList(),
                };
        }
        /// <summary> В вебмодель </summary>
        public static IEnumerable<ProductWebModel> ToWeb(this IEnumerable<Entities.Product> products) => products.Select(ToWeb);
    }
}
