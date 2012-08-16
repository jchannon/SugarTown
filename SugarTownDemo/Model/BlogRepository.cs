using System.Collections.Generic;
using RestSharp;
using SugarTown.Models;

namespace SugarTownDemo.Model
{
    public class BlogRepository : IBlogRepository
    {
        public List<Post> GetBlogUrlFriendlyPosts(string URL)
        {
            var client = new RestClient(URL);
            // client.Authenticator = new HttpBasicAuthenticator(username, password);

            var request = new RestRequest("SugarTown/Posts", Method.GET);
            //request.AddParameter("name", "value"); // adds to POST or URL querystring based on Method
            //request.AddUrlSegment("id", 123); // replaces matching token in request.Resource

            // easily add HTTP Headers
            request.AddHeader("Accept", "application/json");

            // add files to upload (works with compatible verbs)
            //request.AddFile(path);

            // execute the request
            //var response = client.Execute(request);
            //var content = response.Content; // raw content as string

            // or automatically deserialize result
            // return content type is sniffed but can be explicitly set via RestClient.AddHandler();
            var response = client.Execute<List<Post>>(request);
            //var name = response2.Data.Name;
            if (!string.IsNullOrWhiteSpace(response.ErrorMessage) || response.Data == null)
                return new List<Post>();

            var model = response.Data;

            return model;
        }

        public Post GetPost(string title, string url)
        {
            var client = new RestClient(url);
            // client.Authenticator = new HttpBasicAuthenticator(username, password);

            var request = new RestRequest("SugarTown/Posts/" + title, Method.GET);
            //request.AddParameter("name", "value"); // adds to POST or URL querystring based on Method
            //request.AddUrlSegment("id", 123); // replaces matching token in request.Resource

            // easily add HTTP Headers
            request.AddHeader("Accept", "application/json");

            // add files to upload (works with compatible verbs)
            //request.AddFile(path);

            // execute the request
            //var response = client.Execute(request);
            //var content = response.Content; // raw content as string

            // or automatically deserialize result
            // return content type is sniffed but can be explicitly set via RestClient.AddHandler();
            var model = client.Execute<Post>(request).Data;
            //var name = response2.Data.Name;
            return model;
        }
    }
}