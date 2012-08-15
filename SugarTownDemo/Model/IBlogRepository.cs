using System.Collections.Generic;
using SugarTown.Models;

namespace SugarTownDemo.Model
{
    public interface IBlogRepository
    {
        List<Post> GetBlogPosts(string URL);
    }
}