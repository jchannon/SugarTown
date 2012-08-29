using System.Collections.Generic;
using AgnosticPaging;
using RestSharp;
using SugarTown.Models;

namespace SugarTownDemoMVC.Model
{
    public class BlogRepository : IBlogRepository
    {
        public Paged<Post> GetBlogUrlFriendlyPosts(string URL, int pageNumber)
        {
            var client = new RestClient(URL);

            var request = new RestRequest("SugarTown/Posts/page/" + pageNumber, Method.GET);

            request.AddHeader("Accept", "application/json");

            var response = client.Execute<Paged<Post>>(request);
            
            if (!string.IsNullOrWhiteSpace(response.ErrorMessage) || response.Data == null)
                return new Paged<Post>();

            var model = response.Data;

            return model;
        }

        public Post GetPost(string title, string url)
        {
            var client = new RestClient(url);

            var request = new RestRequest("SugarTown/Posts/" + title, Method.GET);

            request.AddHeader("Accept", "application/json");

            var model = client.Execute<Post>(request).Data;
            
            return model;
        }

        public Paged<Post> GetPostsWithTags(string URL, string tag, int pageNumber)
        {
            var client = new RestClient(URL);

            var request = new RestRequest("SugarTown/Posts/tag/" + tag + "/page/" + pageNumber, Method.GET);

            request.AddHeader("Accept", "application/json");

            var response = client.Execute<Paged<Post>>(request);

            if (!string.IsNullOrWhiteSpace(response.ErrorMessage) || response.Data == null)
                return new Paged<Post>();

            var model = response.Data;

            return model;
        }
    }
}