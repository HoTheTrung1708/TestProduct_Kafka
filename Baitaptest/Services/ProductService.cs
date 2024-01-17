using Baitaptest.IServices;
using Baitaptest.Memory;
using Baitaptest.Models;
using System;

namespace Baitaptest.Services
{
    public class ProductService : ITableProduct
    {
        private readonly TableProductMemory _memory;

        public ProductService(TableProductMemory memory)
        {
            _memory = memory;
        }

        public Product GetQuantity(int id)
        {
            var product = _memory.Memory.FirstOrDefault(x => x.Key == id.ToString()).Value;
            return product;
        }

        public Product InsertProduct(Product p)
        {
            _memory.Memory.Add(p.Id.ToString(), p);
            return p;
        }

        public Product UpdateQuantity(Product product)
        {
            var pr = GetQuantity(product.Id);
            _memory.Memory.TryGetValue(pr.Id.ToString(), out product);
            pr.Quantity = product.Quantity;
            return pr;
        }
    }
}
