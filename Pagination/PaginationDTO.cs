namespace Core.Utilities.Pagination;

public class PaginationDTO<TEntity>
{
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
    public int PageCount { get; set; }
    public List<TEntity> Data { get; set; }
}