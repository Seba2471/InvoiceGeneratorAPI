namespace InvoiceGenerator.Models
{
    public class InvoiceDto
    {
        public Guid Id { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime IssueDate { get; set; }
        public string SellerFullName { get; set; }
        public string BuyerFullName { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
    }
}
