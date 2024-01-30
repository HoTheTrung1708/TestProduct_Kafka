using Baitaptest.BackgroundTasks;
using Baitaptest.Extensions;
using Baitaptest.Services;
using Baitaptest.Settings;
using Manonero.MessageBus.Kafka.Extensions;

var builder = WebApplication.CreateBuilder(args);

var appSetting = AppSetting.MapValue(builder.Configuration);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ProductDbContext>(options =>
{
    options.UseOracle(builder.Configuration.GetConnectionString("db"));
});
builder.Services.AddMemoryCache();
builder.Services.AddTransient<ITableProduct, ProductService>();
builder.Services.AddSingleton<TableProductMemory>();
builder.Services.AddKafkaConsumers(consumerBuidel =>
{
    consumerBuidel.AddConsumer<CashConsumingTask>(appSetting.GetConsumerSetting("0"));
    //consumerBuidel.AddConsumer<ConsumingTask>(appSetting.GetConsumerSetting("1"));
});
builder.Services.AddKafkaProducers(producerBuidel =>
{
    producerBuidel.AddProducer(appSetting.GetProducerSetting("0"));
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();


app.UseAuthorization();

app.MapControllers();

app.LoadDataToMemory<TableProductMemory, ProductDbContext>((productInMe, dbContext) =>
{
    new ProductMemorySeedAsync().SeedAsync(productInMe, dbContext).Wait();
});

app.UseKafkaMessageBus(messageBus =>
{
    messageBus.RunConsumer("0");
    //messageBus.RunConsumer("1");
});

app.Run();
