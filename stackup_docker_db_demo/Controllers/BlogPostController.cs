using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using stackup_docker_db_demo.DBProviders;
using stackup_docker_db_demo.Model;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace stackup_docker_db_demo.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BlogPostController : ControllerBase
	{
		RedisProvider _redis;
		PostgressProvider _postgress;
		MongoProvider _mongo;
		public BlogPostController(RedisProvider redis, PostgressProvider postgress, MongoProvider mongo)
		{
			_redis = redis;
			_postgress = postgress;
			_mongo = mongo;
		}

		[HttpPost]
		public async Task<IActionResult> CreateDiscountCoupon(BlogPost post)
		{
			//dummy code to create post in all three containerized db
			post.Id = Guid.NewGuid().ToString();
			post.BlogTitle += " Redis";
			await _redis.UpdateKeyValue(post);
			await _postgress.CreateBlogPost(post);
			await _mongo.CreatePost(post);
			return Ok();
		}

		[HttpGet]
		public async Task<IActionResult> GetBlogPost()
		{
			var response = new BlogPostResponse();
			response.PostgressBlogPost = await _postgress.GetBlogPost();
			response.MongoBlogPost = await _mongo.GetPost();
			response.RedisBlogPost = await _redis.GetKeyFromRedis();
			return Ok(response);
		}
	}
}
