namespace WebApplication1.Models
{
    public class Bill
    {
        public string Id { get; set; }
        public DateTime CreateDate { get; set; }
        public decimal Money { get; set; }
        public string Username { get; set; }
        public int Status { get; set; }
        public virtual List<BillDetails> Details { get; set; }
        public virtual Account Account { get; set; }
    }
}
