namespace Baitaptest.Memory
{
    public class ProductMemorySeedAsync
    {
        public async Task SeedAsync(TableProductMemory memory, ProductDbContext dbContext)
        {
            var products = await dbContext.Products.ToListAsync();
            if (products.Count > 0)
            {
                foreach (var product in products)
                {
                    memory.Memory.Add(product.Id.ToString(), product);
                }
            }
        }
    }
}