namespace InvoiceGenerator.Entities
{
    public class Contractor
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public Address Address { get; set; }
        public string NIP { get; set; }
        public IEnumerable<Invoice> InvoicesBuyer { get; set; }
        public IEnumerable<Invoice> InvoicesSeller { get; set; }
        public Guid InvoiceId { get; set; }

    }
}
