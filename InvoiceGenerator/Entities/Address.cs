using System.Text.Json.Serialization;

namespace InvoiceGenerator.Entities
{
    public class Address
    {
        [JsonIgnore]
        public Guid Id { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Country { get; set; } = "Polska";
        [JsonIgnore]
        public Contractor Contractor { get; set; }
        [JsonIgnore]
        public Guid ContractorId { get; set; }
    }
}
