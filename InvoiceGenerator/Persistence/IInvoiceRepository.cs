using InvoiceGenerator.Entities;
using InvoiceGenerator.Responses;

namespace InvoiceGenerator.Persistence
{
    public interface IInvoiceRepository : IAsyncRepository<Invoice>
    {
        public Task<PaginationResponse<Invoice>> GetUserInvoiceById(int pageNumber, int pageSize, string userId);
    }
}
