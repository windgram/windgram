using Dapper;
using MySqlConnector;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windgram.Blogging.ApplicationCore.Constants;
using Windgram.Blogging.ApplicationCore.Domain.Entities;
using Windgram.Blogging.ApplicationCore.Enums;
using Windgram.Blogging.ApplicationCore.ViewModels;
using Windgram.Caching;

namespace Windgram.Blogging.ApplicationCore.Queries
{
    public class PostQueries : IPostQueries
    {
        private readonly string _connectionStrings;
        private readonly ICacheManager _cacheManager;
        public PostQueries(string connectionStrings, ICacheManager cacheManager)
        {
            _connectionStrings = connectionStrings;
            _cacheManager = cacheManager;
        }
        public async Task<PostViewModel> GetById(int id)
        {
            return await _cacheManager.GetOrCreateAsync(Post.GetByIdCacheKey(id),
                () => LoadById(id));
        }

        private async Task<PostViewModel> LoadById(int id)
        {
            using (var connection = new MySqlConnection(_connectionStrings))
            {
                await connection.OpenAsync();

                var sql = $@"SELECT P.*,PC.MetaDescription,PC.MetaKeyword,PC.HtmlContent,T.Name AS TagName 
                        FROM {BloggingTableDefaults.Schema}_{BloggingTableDefaults.Post} AS P
                        JOIN {BloggingTableDefaults.Schema}_{BloggingTableDefaults.PostContent} AS PC ON P.Id = PC.PostId
                        JOIN {BloggingTableDefaults.Schema}_{BloggingTableDefaults.PostTag} AS PT ON P.Id = PT.PostId
                        JOIN {BloggingTableDefaults.Schema}_{BloggingTableDefaults.Tag} AS T ON PT.TagId = T.Id
                        WHERE P.Id = @Id;";
                var result = await connection.QueryAsync<dynamic>(sql, new { id });
                if (result.AsList().Count == 0)
                    return null;
                return MapSinglePost(result);
            }
        }
        private PostViewModel MapSinglePost(dynamic list)
        {
            var result = list[0];
            var model = new PostViewModel
            {
                Tags = new List<string>(),
                CoverFileId = result.CoverFileId,
                CreatedBy = result.CreatedBy,
                CreatedDateTime = result.CreatedDateTime,
                Description = result.Description,
                HtmlContent = result.HtmlContent,
                Id = result.Id,
                MetaDescription = result.MetaDescription,
                MetaKeyword = result.MetaKeyword,
                ParentPostId = result.ParentPostId,
                PostStatus = (PostStatusType)result.PostStatus,
                PostType = (PostType)result.PostType,
                PublishedDateTime = result.PublishedDateTime,
                Slug = result.Slug,
                Title = result.Title
            };
            foreach (dynamic item in list)
            {
                model.Tags.Add(item.TagName);
            }
            return model;
        }
    }
}
