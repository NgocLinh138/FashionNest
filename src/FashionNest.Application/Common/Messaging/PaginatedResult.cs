namespace FashionNest.Application.Common.Messaging
{
    public class PaginatedResult<T>
    {
        public PaginatedResult(List<T> items, int totalItemCount, int pageIndex, int pageSize)
        {
            Items = items;
            TotalItemCounts = totalItemCount;
            TotalPages = (int)Math.Ceiling(totalItemCount / (double)pageSize);
            ItemsFrom = pageSize * (pageIndex - 1) + 1;
            ItemsTo = ItemsFrom + pageSize - 1;
        }

        public List<T> Items { get; set; }
        public int TotalPages { get; set; }
        public int TotalItemCounts { get; set; }
        public int ItemsFrom { get; set; }
        public int ItemsTo { get; set; }
    }
}
