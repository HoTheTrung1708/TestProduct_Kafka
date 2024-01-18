using Baitaptest.DTOs;
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
        private readonly ITableProduct    _tableProduct;
        private readonly ILogger<Product> _logger;
        public CashConsumingTask(ITableProduct tableProduct, ILogger<Product> logger)
        {
            _tableProduct = tableProduct;
            _logger       = logger;
        }

        public void Execute(ConsumeResult<string, string> result)
        {
             Headers headers = result.Message.Headers;
            var productEvent = "";
            // Convert header keys and values to strings
            foreach (var header in headers)
            {
                productEvent = Encoding.UTF8.GetString(header.GetValueBytes());
            }
            if (productEvent == "InsertProduct")
            {
                var p = JsonSerializer.Deserialize<Product>(result.Message.Value);
                _tableProduct.InsertProduct(p);
            }

            else if (productEvent == "UpdateQuantity")
            {
                var p = JsonSerializer.Deserialize<UpdateQuantity>(result.Message.Value);
                var productInMem = _tableProduct.GetProduct(p.ProductId);
                if (productInMem != null)
                {
                    if (p.Increase == true)
                    {
                        productInMem.Quantity = productInMem.Quantity + p.Quantity;
                    }
                    else
                    {
                        if (productInMem.Quantity < p.Quantity)
                        {
                            _logger.LogError("deo am duoc");
                        }
                        else
                        {
                            productInMem.Quantity = productInMem.Quantity - p.Quantity;
                        }
                    }
                    _tableProduct.UpdateQuantity(productInMem);
                }
            }

            else if (productEvent == "UpdatePrice")
            {
                var p = JsonSerializer.Deserialize<UpdateQuantity>(result.Message.Value);
                var productInMem = _tableProduct.GetProduct(p.ProductId);

                if (productInMem != null)
                {
                    if (p.Price < 0)
                    {
                        _logger.LogError("Price khong the la so âm!!!");
                    }
                    else
                    {
                        productInMem.Price = p.Price;
                        _tableProduct.UpdatePrice(productInMem);
                    }
                }
            }
        }
    }
}
