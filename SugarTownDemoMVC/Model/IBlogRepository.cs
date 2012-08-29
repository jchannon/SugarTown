using AgnosticPaging;
using SugarTown.Models;

namespace SugarTownDemoMVC.Model
{
    public interface IBlogRepository
    {
        Paged<Post> GetBlogUrlFriendlyPosts(string URL, int pageNumber);
        Post GetPost(string title, string url);
        Paged<Post> GetPostsWithTags(string domain, string tag, int pageNumber);
    }
}