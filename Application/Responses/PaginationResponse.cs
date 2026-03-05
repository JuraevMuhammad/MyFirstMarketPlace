using System.Net;

namespace Application.Responses;

public class PaginationResponse<T> : Response<T>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int TotalRecords { get; set; }

    public PaginationResponse(HttpStatusCode statusCode, string massage) : base(statusCode, massage)
    { }

    public PaginationResponse(int pageNumber, int pageSize, int totalRecords, T data) : base(data)
    {
        PageNumber = pageNumber > 0 ? pageNumber : 1;
        PageSize = pageSize > 0 ? pageSize : 10;
        TotalPages = (int)Math.Ceiling((double)totalRecords / PageSize);
        TotalRecords = totalRecords;
    }
}