using Baitaptest.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Baitaptest.IServices
{
    public interface ITableProduct
    {
        Product InsertProduct(Product p);

        public Product GetProduct(int id);

        public Product UpdateQuantity(Product product);

        public Product UpdatePrice(Product product);
    }
}
