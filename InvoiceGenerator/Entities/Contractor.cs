using System.Text.Json.Serialization;

namespace InvoiceGenerator.Entities
{
    public class Contractor
    {
        [JsonIgnore]
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public Address Address { get; set; }
        public string NIP { get; set; }
        [JsonIgnore]
        public IEnumerable<Invoice> InvoicesBuyer { get; set; }
        [JsonIgnore]
        public IEnumerable<Invoice> InvoicesSeller { get; set; }
        [JsonIgnore]
        public Guid InvoiceId { get; set; }

    }
}
