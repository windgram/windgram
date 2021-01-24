using System.Threading.Tasks;
using Windgram.Blogging.ApplicationCore.ViewModels;

namespace Windgram.Blogging.ApplicationCore.Queries
{
    public interface IPostQueries
    {
        Task<PostViewModel> GetById(int id);
    }
}