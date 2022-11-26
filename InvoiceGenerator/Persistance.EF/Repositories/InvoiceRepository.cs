using InvoiceGenerator.Entities;
using InvoiceGenerator.Persistance.EF;
using InvoiceGenerator.Persistence;
using InvoiceGenerator.Repositories;
using InvoiceGenerator.Responses;
using Microsoft.EntityFrameworkCore;

namespace InvoiceGenerator.PersistanceEF.Repositories
{
    public class InvoiceRepository : BaseRepository<Invoice>, IInvoiceRepository
    {
        public InvoiceRepository(InvoiceGeneratorContext dbContext) : base(dbContext)
        {
        }

        public async Task<PaginationResponse<Invoice>> GetUserInvoiceById(int pageNumber, int pageSize, string userId)
        {

            var baseQuery = _dbContext.Invoices.OrderBy(i => i.IssueDate);

            var invoices = await baseQuery
                .Include(x => x.User)
                .Include(x => x.InvoiceItems)
                .Include(x => x.Buyer)
                    .ThenInclude(x => x.Address)
                .Include(x => x.Seller)
                    .ThenInclude(x => x.Address)
                .Where(x => x.User.Id == userId)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

            var totalCount = baseQuery.Where(x => x.User.Id == userId).AsNoTracking().Count();

            return new PaginationResponse<Invoice>(invoices, totalCount, pageSize, pageNumber);
        }
    }
}
