namespace Baitaptest.BackgroundTasks
{
    public class CashConsumingTask : IConsumingTask<string, string>
    {
        private readonly ITableProduct _tableProduct;
        private readonly ILogger<Product> _logger;
        private readonly IKafkaProducerManager _producerManager;
        public CashConsumingTask(ITableProduct tableProduct, ILogger<Product> logger, IKafkaProducerManager producerManager)
        {
            _tableProduct = tableProduct;
            _logger = logger;
            _producerManager = producerManager;
        }

        public void Execute(ConsumeResult<string, string> result)
        {

            var producer = _producerManager.GetProducer<string, string>("0");
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
                var message = new Message<string, string>
                {
                    Value = JsonSerializer.Serialize(p),
                    Headers = new Headers
                    {
                       {   
                         "eventName", Encoding.UTF8.GetBytes("InsertProduct")
                       },
                    }
                };
                producer.Produce(message);
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
                var message = new Message<string, string>
                {
                    Value = JsonSerializer.Serialize(p),
                    Headers = new Headers
                    {
                       {
                         "eventName", Encoding.UTF8.GetBytes("UpdateQuantity")
                       },
                    }
                };
                producer.Produce(message);
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
                var message = new Message<string, string>
                {
                    Value = JsonSerializer.Serialize(p),
                    Headers = new Headers
                    {
                       {
                         "eventName", Encoding.UTF8.GetBytes("UpdatePrice")
                       },
                    }
                };
                producer.Produce(message);
            }
        }
    }
}
