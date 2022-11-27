using InvoiceGenerator.Entities;
using InvoiceGenerator.Responses;

namespace InvoiceGenerator.Persistence
{
    public interface IInvoiceRepository : IAsyncRepository<Invoice>
    {
        public Task<Pagination<Invoice>> GetUserInvoiceById(int pageNumber, int pageSize, string userId);

        public Task<Invoice> GetByIdWithIncludes(string Id);
        public Task<Boolean> IsOwner(string id, string userId);
    }
}
