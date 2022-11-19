namespace InvoiceGenerator.Models
{
    public class Test
    {
        public string InvoiceNumber { get; set; }
        public IEnumerable<InvoiceItem> InvoiceItems { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime SoldDate { get; set; }
        public Person Seller { get; set; }
    }
}
