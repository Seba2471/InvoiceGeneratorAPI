using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace InvoiceGenerator.Entities
{
    public class Invoice
    {
        [JsonIgnore]
        public Guid Id { get; set; }
        [JsonIgnore]
        public IdentityUser User { get; set; }
        public string InvoiceNumber { get; set; }
        [BindProperty]
        public DateTime IssueDate { get; set; }
        public DateTime SoldDate { get; set; }
        public Contractor Seller { get; set; }
        [JsonIgnore]
        public Guid SellerId { get; set; }
        public Contractor Buyer { get; set; }
        [JsonIgnore]
        public Guid BuyerId { get; set; }
        public int VatRate { get; set; }
        public string Currency { get; set; }
        public IEnumerable<InvoiceItem> InvoiceItems { get; set; }
    }
}
