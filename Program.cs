using Autofac;
using Autofac.Extensions.DependencyInjection;
using Market.Abstractions;
using Market.Models;
using Market.Repo;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(MappingProFile));
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

//Получение строки подключения к БД из json
var config = new ConfigurationBuilder();
config.AddJsonFile("appsettings.json");
var cfg = config.Build();

//Добавление через autofac
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterType<ProductRepository>().As<IProductRepository>();

    containerBuilder.Register(c => new ProductContext(cfg.GetConnectionString("db"))).InstancePerDependency();
});

builder.Services.AddMemoryCache(mc => mc.TrackStatistics = true);
//Добавление через синглтон
//builder.Services.AddSingleton<IProductRepository, ProductRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var staticFilesPath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles");
Directory.CreateDirectory(staticFilesPath);

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(staticFilesPath), RequestPath = "/static"
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
