using System.Collections.Generic;
using System.Threading.Tasks;
using Windgram.Blogging.ApplicationCore.Domain.Entities;
using Windgram.Blogging.ApplicationCore.ViewModels;
using Windgram.Shared.Application.Models;

namespace Windgram.Blogging.ApplicationCore.Queries
{
    public interface ITagQueries
    {
        Task<TagViewModel> GetByAlias(string alias);
        Task<Tag> GetByName(string name);
        Task<Tag> GetById(int id);
        Task<IEnumerable<Tag>> GetPublishedTags();
        Task<IEnumerable<SelectItemModel>> GetSelectList(string keywords);
        Task<IEnumerable<Tag>> LoadPublishedTags();
    }
}