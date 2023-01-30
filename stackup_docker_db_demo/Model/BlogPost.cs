using MongoDB.Driver;
using System.Collections.Generic;

namespace stackup_docker_db_demo.Model
{
	public class BlogPost
	{
		public string Id { get; set; }

		public string BlogName { get; set; }

		public string BlogTitle { get; set; }

		public string BlogDescription { get; set;}

		public string Tags { get; set; }

	}

	public class BlogPostResponse
	{
		public IEnumerable<BlogPost> MongoBlogPost { get; set; }
		public BlogPost RedisBlogPost { get; set; }

		public List<BlogPost> PostgressBlogPost { get; set;}
	}
}
