using Baitaptest.IServices;
using Baitaptest.Memory;
using Baitaptest.Models;
using System;

namespace Baitaptest.Services
{
    public class ProductService : ITableProduct
    {
        private readonly TableProductMemory _memory;
        private readonly ILogger<Product> _logger;
        public ProductService(TableProductMemory memory, ILogger<Product> logger)
        {
            _memory = memory;
            _logger = logger;
        }

        public Product GetProduct(int id)
        {
            var p = _memory.Memory.FirstOrDefault(x=>x.Value.Id == id).Value;
            return p;
        }

        public Product InsertProduct(Product p)
        {
            _memory.Memory.Add(p.Id.ToString(), p);
            return p;
        }

        public Product UpdatePrice(Product product)
        {
            _memory.Memory.TryGetValue(product.Id.ToString(), out product);

            return product;
        }

        public Product UpdateQuantity(Product product)
        {
            var pr = GetProduct(product.Id);
   
           _memory.Memory.TryGetValue(pr.Id.ToString(), out product);
            
            return pr;
        }
    }
}
