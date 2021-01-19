using System.Collections.Generic;

namespace Windgram.Shared.Application.Pager
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