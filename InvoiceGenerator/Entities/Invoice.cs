using InvoiceGenerator.Models;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceGenerator.Entities
{
    public class Invoice
    {
        public string InvoiceNumber { get; set; }
        [BindProperty]
        public DateTime IssueDate { get; set; }
        public DateTime SoldDate { get; set; }
        public Person Seller { get; set; }
        public Person Buyer { get; set; }
        public int VatRate { get; set; }
        public string Currency { get; set; }
        public IEnumerable<InvoiceItem> InvoiceItems { get; set; }
    }
}
