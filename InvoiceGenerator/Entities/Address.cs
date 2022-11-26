namespace InvoiceGenerator.Entities
{
    public class Address
    {
        public Guid Id { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Country { get; set; } = "Polska";
        public Contractor Contractor { get; set; }
        public Guid ContractorId { get; set; }
    }
}
