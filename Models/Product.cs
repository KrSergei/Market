namespace Market.Models
{
    public class Product : BaseModel
    {
        public int Price { get; set; }
        public int Category_Id { get; set; }
        public string Descriptions { get; set; } = null!;
        public virtual Category Category { get; set; } = null!;
        public virtual List<Storage> Storages { get; set; } = null!;
    }
}