using Baitaptest.BackgroundTasks;
using Baitaptest.Extensions;
using Baitaptest.IServices;
using Baitaptest.Memory;
using Baitaptest.Models;
using Baitaptest.Services;
using Baitaptest.Settings;
using Microsoft.EntityFrameworkCore;

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
    messageBus.RunConsumerAsync("0");
});

app.Run();