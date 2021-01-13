using System.Collections.Generic;

namespace Windgram.Application.Shared.Pager
{
    public interface IPagedResult<T> where T : class
    {
        IEnumerable<T> Data { get; }
        int PageIndex { get; }
        int PageSize { get; }
        int TotalCount { get; }
        int TotalPages { get; }
    }
}