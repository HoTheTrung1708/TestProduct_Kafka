using Baitaptest.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Baitaptest.IServices
{
    public interface ITableProduct
    {
        Product InsertProduct(Product p);
        public Product Add(Product product);
        public List<Product> GetProducts();
    }
}
