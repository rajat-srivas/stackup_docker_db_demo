using Microsoft.Extensions.Caching.Distributed;
using MongoDB.Bson.IO;
using System.Threading.Tasks;
using System;
using stackup_docker_db_demo.Model;
using Newtonsoft.Json;
using JsonConvert = Newtonsoft.Json.JsonConvert;

namespace stackup_docker_db_demo.DBProviders
{
	public class RedisProvider
	{
		private readonly IDistributedCache _redisCache;
		public RedisProvider(IDistributedCache cache)
		{
			_redisCache = cache;
		}

		public async Task<BlogPost> GetKeyFromRedis()
		{
			string postJson = await _redisCache.GetStringAsync("default");
			if (string.IsNullOrEmpty(postJson)) return null;
			var post = Newtonsoft.Json.JsonConvert.DeserializeObject<BlogPost>(postJson);
			return post;
		}

		public async Task<BlogPost> UpdateKeyValue(BlogPost post)
		{
			string postJson = JsonConvert.SerializeObject(post);
			await _redisCache.SetStringAsync("default", postJson);
			return await GetKeyFromRedis();
		}

	}
}
