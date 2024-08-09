namespace XMLtoDB.src.Models
{
    public class Order
    {
        public int No { get; set; }
        public string Reg_Date { get; set; }
        public Double Sum { get; set; }
        public List<Product> Products { get; set; }
        public User User { get; set; }
    }
}
