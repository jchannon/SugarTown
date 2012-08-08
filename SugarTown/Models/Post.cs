using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace SugarTown.Models
{
    public class Post
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public DateTime DateCreated { get; set; }
        public string Body { get; set; }
        public IEnumerable<string> Tags { get; set; }
    }
}