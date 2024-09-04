using AutoMapper;
using Market.Abstractions;
using Market.Models;
using Market.Models.DTO;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Market.Repo;

public class ProductRepository : IProductRepository
{
    readonly private IMapper? _mapper;
    private IMemoryCache _memoryCache;
    private ProductContext _context;

    public ProductRepository(IMapper? mapper, IMemoryCache memoryCache, ProductContext context)
    {
        _mapper = mapper;
        _memoryCache = memoryCache;
        _context = context;
    }
    public int AddCategory(CategoryDto category)
    {
        try
        {
            using (_context)
            {
                var entityCategory = _context.Categoryes.FirstOrDefault(x => x.Name.ToLower().Equals(category.Name.ToLower()));
                if (entityCategory == null)
                {                        
                    entityCategory = _mapper?.Map<Category>(category);
                    _context.Categoryes.Add(entityCategory);
                    _context.SaveChanges();                    
                    _memoryCache.Remove("category");
                }
                return entityCategory.Id;
            }
        }
        catch (Exception e)
        {
            throw;
        }
    }

    public int AddProduct(ProductDto product)
    {
        try
        {
            using (_context)
            {
                var entityProduct = _context.Products.FirstOrDefault(x => x.Name.ToLower().Equals(product.Name.ToLower()));
                if (entityProduct == null)
                {
                    entityProduct = _mapper?.Map<Product>(product);
                    _context.Products.Add(entityProduct);
                    _context.SaveChanges();
                    _memoryCache.Remove("products");
                }
                return entityProduct.Id;
            }
        }
        catch (Exception e)
        {
            throw;
        }
    }

    public IEnumerable<CategoryDto> GetCategoryes()
    {
        if(_memoryCache.TryGetValue("category", out List<CategoryDto>? categoryDto))
        {
            return categoryDto;
        }           
        using (_context)
        {
            var categoriesList = _context.Categoryes.Select(x => _mapper.Map<CategoryDto>(x)).ToList();
            _memoryCache.Set("category", categoriesList, TimeSpan.FromMinutes(30));
            return categoriesList;
        }
    }

    public IEnumerable<ProductDto> GetProducts()
    {
        if (_memoryCache.TryGetValue("products", out List<ProductDto>? productDto))
        {
            return productDto;
        }
        using (_context)
        {
            var productsList = _context.Products.Select(x => _mapper.Map<ProductDto>(x)).ToList();
            _memoryCache.Set("products", productsList, TimeSpan.FromMinutes(30));
            return productsList;
        }
    }

    public string GetCasheStatisticURL()
    {
        var content = "";
        content = JsonSerializer.Serialize(_memoryCache.GetCurrentStatistics());
        return content;
    }
}
