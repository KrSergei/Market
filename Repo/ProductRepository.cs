using AutoMapper;
using Market.Abstractions;
using Market.Models;
using Market.Models.DTO;
using Microsoft.Extensions.Caching.Memory;

namespace Market.Repo
{
    public class ProductRepository : IProductRepository
    {
        readonly private IMapper? _mapper;
        private IMemoryCache _memoryCache;

        public ProductRepository(IMapper? mapper, IMemoryCache memoryCache)
        {
            _mapper = mapper;
            _memoryCache = memoryCache;
        }
        public int AddCategory(CategoryDto category)
        {
            try
            {
                using (var context = new ProductContext())
                {
                    var entityCategory = context.Categoryes.FirstOrDefault(x => x.Name.ToLower().Equals(category.Name.ToLower()));
                    if (entityCategory == null)
                    {                        
                        entityCategory = _mapper?.Map<Category>(category);
                        context.Categoryes.Add(entityCategory);
                        context.SaveChanges();                    
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
                using (var context = new ProductContext())
                {
                    var entityProduct = context.Products.FirstOrDefault(x => x.Name.ToLower().Equals(product.Name.ToLower()));
                    if (entityProduct == null)
                    {
                        entityProduct = _mapper?.Map<Product>(product);
                        context.Products.Add(entityProduct);
                        context.SaveChanges();
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
            using (var context = new ProductContext())
            {
                var categoriesList = context.Categoryes.Select(x => _mapper.Map<CategoryDto>(x)).ToList();
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
            using (var context = new ProductContext())
            {
                var productsList = context.Products.Select(x => _mapper.Map<ProductDto>(x)).ToList();
                _memoryCache.Set("products", productsList, TimeSpan.FromMinutes(30));
                return productsList;
            }
        }
    }
}
