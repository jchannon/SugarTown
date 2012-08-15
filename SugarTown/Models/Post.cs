using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SugarTown.Models
{
    public class Post
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public DateTime DateCreated { get; set; }
        public string Body { get; set; }
        public List<string> Tags { get; set; }
    }
}