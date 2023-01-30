using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using stackup_docker_db_demo.Model;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace stackup_docker_db_demo.DBProviders
{
	public class MongoProvider
	{
		public IMongoClient _mongoClient;
		public IMongoDatabase _mongoDatabase;
		public IMongoCollection<BlogPost> BlogPosts { get; }

		public MongoProvider(IConfiguration config, IMongoClient _client)
		{
			_mongoClient = _client;
			_mongoDatabase = _mongoClient.GetDatabase("stackupMongodb");
			BlogPosts = _mongoDatabase.GetCollection<BlogPost>("BlogPosts");
		}

		public async Task CreatePost(BlogPost post)
		{
			await BlogPosts.InsertOneAsync(post);
		}

		public async Task<IEnumerable<BlogPost>> GetPost()
		{
			return await BlogPosts.Find(x => true).ToListAsync();
		}
	}
}
