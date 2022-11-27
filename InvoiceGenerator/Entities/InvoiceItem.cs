using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace InvoiceGenerator.Entities
{
    public class InvoiceItem
    {
        [JsonIgnore]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        [Precision(2)]
        public decimal Cost { get; set; }
        [JsonIgnore]
        public Invoice Invoice { get; set; }
        [JsonIgnore]
        public Guid InvoiceId { get; set; }
    }
}
