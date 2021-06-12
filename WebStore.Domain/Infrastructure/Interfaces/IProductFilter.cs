using System.Runtime.InteropServices;
using WebStore.Domain.Infrastructure.Filters;

namespace WebStore.Domain.Infrastructure.Interfaces
{
    public interface IProductFilter
    {
        int? SectionId { get; set; }
        int? BrandId { get; set; }
        int[] Ids { get; set; }
    }
}