using Market.Models.DTO;

namespace Market.Abstractions
{
    public interface IProductRepository
    {
        public int AddCategory(CategoryDto category);
        public IEnumerable<CategoryDto> GetCategory();
        public int AddProduct(ProductDto product);
        public IEnumerable<ProductDto> GetProduct();
    }
}
