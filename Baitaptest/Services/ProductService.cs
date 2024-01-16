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

        public Product InsertProduct(Product p)
        {
            _memory.Memory.Add(p.Id.ToString(), p);
            return p;
        }

        public Product Add(Product product)
        {
            throw new NotImplementedException();
        }

        public List<Product> GetProducts()
        {
            throw new NotImplementedException();
        }

       
    }
}
