using Microsoft.EntityFrameworkCore;

namespace InvoiceGenerator.Entities
{
    public class InvoiceItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        [Precision(2)]
        public decimal Cost { get; set; }
        public Invoice Invoice { get; set; }
        public Guid InvoiceId { get; set; }
    }
}
