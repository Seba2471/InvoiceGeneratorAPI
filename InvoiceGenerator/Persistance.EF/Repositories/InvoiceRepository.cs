using InvoiceGenerator.Entities;
using InvoiceGenerator.Persistance.EF;
using InvoiceGenerator.Persistence;
using InvoiceGenerator.Repositories;

namespace InvoiceGenerator.PersistanceEF.Repositories
{
    public class InvoiceRepository : BaseRepository<Invoice>, IInvoiceRepository
    {
        public InvoiceRepository(InvoiceGeneratorContext dbContext) : base(dbContext)
        {
        }
    }
}
