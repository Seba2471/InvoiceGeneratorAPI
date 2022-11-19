namespace InvoiceGenerator.Models
{
    public class InvoiceItem
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Cost { get; set; }
    }
}
