namespace Market.Models
{
    public class Storage : BaseModel
    {  
        public virtual List<Product> Products { get; set; } = new();
        public int Count { get; set; }       
    }
}
