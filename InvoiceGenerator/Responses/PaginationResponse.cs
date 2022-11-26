namespace InvoiceGenerator.Responses
{
    public class PaginationResponse<T>
    {
        public ICollection<T> Items { get; set; }
        public int TotalPages { get; set; }
        public int ItemsFrom { get; set; }
        public int ItemsTo { get; set; }
        public int TotalItemsCount { get; set; }

        public PaginationResponse(ICollection<T> items, int totalCount, int pageSize, int pageNumber)
        {
            Items = items;
            TotalItemsCount = totalCount;
            ItemsFrom = pageSize * (pageNumber - 1) + 1;

            if (items.Count < pageSize)
            {
                ItemsTo = pageSize * (pageNumber - 1) + items.Count;
            }
            else
            {
                ItemsTo = ItemsFrom + pageSize - 1;
            }
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        }
    }
}
