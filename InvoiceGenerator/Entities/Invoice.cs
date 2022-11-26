using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceGenerator.Entities
{
    public class Invoice
    {
        public Guid Id { get; set; }
        public IdentityUser User { get; set; }
        public string InvoiceNumber { get; set; }
        [BindProperty]
        public DateTime IssueDate { get; set; }
        public DateTime SoldDate { get; set; }
        public Contractor Seller { get; set; }
        public Guid SellerId { get; set; }
        public Contractor Buyer { get; set; }
        public Guid BuyerId { get; set; }
        public int VatRate { get; set; }
        public string Currency { get; set; }
        public IEnumerable<InvoiceItem> InvoiceItems { get; set; }
    }
}
