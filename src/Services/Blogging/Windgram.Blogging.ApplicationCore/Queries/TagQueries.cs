using Dapper;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windgram.Blogging.ApplicationCore.Constants;
using Windgram.Blogging.ApplicationCore.Domain.Entities;
using Windgram.Blogging.ApplicationCore.ViewModels;
using Windgram.Caching;
using Windgram.Shared.Application.Models;

namespace Windgram.Blogging.ApplicationCore.Queries
{
    public class TagQueries : ITagQueries
    {
        private readonly string _connectionStrings;
        private readonly ICacheManager _cacheManager;
        public TagQueries(string connectionStrings, ICacheManager cacheManager)
        {
            _connectionStrings = connectionStrings;
            _cacheManager = cacheManager;
        }
        public async Task<TagViewModel> GetByAlias(string alias)
        {
            return await _cacheManager.GetOrCreateAsync(Tag.GetByAliasCacheKey(alias), () => LoadByAlias(alias));
        }
        private async Task<TagViewModel> LoadByAlias(string alias)
        {
            using (var connection = new MySqlConnection(_connectionStrings))
            {
                await connection.OpenAsync();
                var sql = $@"SELECT Id,Alias,Name,Description,CreatedBy,CreatedDateTime FROM 
                            {BloggingTableDefaults.Schema}_{BloggingTableDefaults.Tag} WHERE Alias = @alias;";
                var result = await connection.QueryFirstOrDefaultAsync<TagViewModel>(sql, new { alias });
                return result;
            }
        }
        public async Task<Tag> GetById(int id)
        {
            using (var connection = new MySqlConnection(_connectionStrings))
            {
                await connection.OpenAsync();
                var sql = $@"SELECT * FROM {BloggingTableDefaults.Schema}_{BloggingTableDefaults.Tag} WHERE Id = @id;";
                var result = await connection.QueryFirstOrDefaultAsync<Tag>(sql, new { id });
                return result;
            }
        }
        public async Task<Tag> GetByName(string name)
        {
            var normalizedName = name.ToUpperNormalized();
            return await _cacheManager.GetOrCreateAsync(Tag.GetByNameCacheKey(name), () => LoadByName(normalizedName));
        }
        private async Task<Tag> LoadByName(string normalizedName)
        {
            using (var connection = new MySqlConnection(_connectionStrings))
            {
                await connection.OpenAsync();
                var sql = $@"SELECT * FROM {BloggingTableDefaults.Schema}_{BloggingTableDefaults.Tag} WHERE NormalizedName = @normalizedName;";
                var result = await connection.QueryFirstOrDefaultAsync<Tag>(sql, new { normalizedName });
                return result;
            }
        }

        public async Task<IEnumerable<SelectItemModel>> GetSelectList(string keywords)
        {
            var tags = await GetPublishedTags();
            if (!keywords.IsNullOrEmpty())
            {
                tags = tags.Where(x => x.Name.Contains(keywords));
            }
            return tags.Select(x => new SelectItemModel
            {
                Text = x.Name,
                Value = x.Id
            });
        }
        public async Task<IEnumerable<Tag>> GetPublishedTags()
        {
            return await _cacheManager.GetOrCreateAsync(Tag.GetPublishedCacheKey, () => LoadPublishedTags());
        }
        public async Task<IEnumerable<Tag>> LoadPublishedTags()
        {
            using (var connection = new MySqlConnection(_connectionStrings))
            {
                await connection.OpenAsync();
                var sql = $@"SELECT * FROM {BloggingTableDefaults.Schema}_{BloggingTableDefaults.Tag} WHERE IsPublished = @isPublished;";
                var result = await connection.QueryAsync<Tag>(sql, new { isPublished = true });
                return result;
            }
        }

    }
}
