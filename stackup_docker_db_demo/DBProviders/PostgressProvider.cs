using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;
using stackup_docker_db_demo.Model;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace stackup_docker_db_demo.DBProviders
{
	public class PostgressProvider
	{
		IConfiguration _configuration;
		string _connectionString;
		public PostgressProvider(IConfiguration config)
		{
			_configuration = config;
			_connectionString = _configuration.GetValue<string>("DatabaseSettings:PostgressConnectionString");
		}

		public async Task<bool> CreateBlogPost(BlogPost post)
		{
			using var _connection = new NpgsqlConnection(_connectionString);
			string query = ""; ;
			var fullPath = $"stackup_docker_db_demo.SqlQueries.CreateBlogPost.sql";
			var assembly = Assembly.GetExecutingAssembly();
			using (Stream stream = assembly.GetManifestResourceStream(fullPath))
			using (StreamReader reader = new StreamReader(stream))
			{
				query = reader.ReadToEnd();
			}

			var inserted = await _connection.ExecuteAsync(query,
				new
				{
					Id = post.Id,
					BlogName = post.BlogName,
					BlogDescription = post.BlogDescription,
					BlogTitle = post.BlogTitle,
					Tags = post.Tags
				});

			return inserted == 0 ? false : true;
		}

		public async Task<List<BlogPost>> GetBlogPost()
		{
			using var _connection = new NpgsqlConnection(_connectionString);
			string query = "select * from blogpost";

			var blog = await _connection.QueryAsync<BlogPost>(query);

			return blog == null ? null : blog.ToList();

		}


	}
}
