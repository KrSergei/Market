namespace Market.Models
{
    public class Storage : BaseModel
    {
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public virtual List<ProductStorage> ProductStorages { get; set; } = new();
    }
}
