using Baitaptest.IServices;
using Baitaptest.Models;
using Confluent.Kafka;
using Manonero.MessageBus.Kafka.Abstractions;
using System.Text;
using System.Text.Json;

namespace Baitaptest.BackgroundTasks
{
    public class CashConsumingTask : IConsumingTask<string, string>
    {
        private readonly ITableProduct _tableProduct;
       

        public CashConsumingTask(ITableProduct itableProduct)
        {
            _tableProduct = itableProduct;
        }
        public Task ExecuteAsync(ConsumeResult<string, string> result)
        {
            var p = JsonSerializer.Deserialize<Product>(result.Message.Value);
            Headers headers = result.Message.Headers;
            var productEvent = "";
            // Convert header keys and values to strings
            foreach (var header in headers)
            {
                productEvent = Encoding.UTF8.GetString(header.GetValueBytes());
            }
            if (productEvent == "InsertProduct")
            {
                _tableProduct.InsertProduct(p);   
            }
            else
            if (productEvent == "UpdateQuantity")
            {

            }
            return null;
        }
    }
}
