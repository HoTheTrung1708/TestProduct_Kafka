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

        public Product GetQuantity(int id)
        {
            var p = _memory.Memory.FirstOrDefault(x=>x.Value.Id == id).Value;
            return p;
        }

        public Product InsertProduct(Product p)
        {
            _memory.Memory.Add(p.Id.ToString(), p);
            return p;
        }

        public Product UpdateQuantity(Product product)
        {
            var pr = GetQuantity(product.Id);
            if(pr != null)
            {
                _logger.LogError("Product khong ton tai");
            }
            else
            {
                _memory.Memory.TryGetValue(pr.Id.ToString(), out product);
            }
            return pr;
        }
    }
}
