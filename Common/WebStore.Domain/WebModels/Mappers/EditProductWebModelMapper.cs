using System.Collections.Generic;
using System.Linq;
using WebStore.Domain.WebModels.Product;

namespace WebStore.Domain.WebModels.Mappers
{
    /// <summary> Маппер в веб модель редактирования товаров </summary>
    public static class EditProductWebModelMapper
    {
        /// <summary> В вебмодель </summary>
        public static EditProductWebModel ToEditWeb(this Entities.Product product)
        {
            return product is null
                ? null
                : new EditProductWebModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    SectionId = product.SectionId,
                    BrandId = product.BrandId,
                    ImageUrl = product.ImageUrl,
                    Price = product.Price,
                    Tags = product.Tags.Select(p => new TagWebModel{ Id = p.Id, Text = p.Text }).ToList(),
                };
        }
        /// <summary> В вебмодель </summary>
        public static IEnumerable<EditProductWebModel> ToEditWeb(this IEnumerable<Entities.Product> products) => products.Select(ToEditWeb);
    }
}
