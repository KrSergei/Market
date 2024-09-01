using AutoMapper;
using Market.Abstractions;
using Market.Models;
using Market.Models.DTO;

namespace Market.Repo
{
    public class ProductRepository : IProductRepository
    {
        readonly private IMapper? _mapper;

        public ProductRepository(IMapper? mapper)
        {
            _mapper = mapper;
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
                    }
                    return entityProduct.Id;
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public IEnumerable<CategoryDto> GetCategory()
        {
            using (var context = new ProductContext())
            {
                var categoriesList = context.Categoryes.Select(x => _mapper.Map<CategoryDto>(x)).ToList();
                return categoriesList;
            }
        }

        public IEnumerable<ProductDto> GetProduct()
        {
            using (var context = new ProductContext())
            {
                var productsList = context.Products.Select(x => _mapper.Map<ProductDto>(x)).ToList();
                return productsList;
            }
        }
    }
}
