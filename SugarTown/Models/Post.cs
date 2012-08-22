using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SugarTown.Models
{
    public class Post
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Slug
        {
            get { return Title.Replace(" ", "-"); }
        }
        public DateTime DateCreated { get; set; }
        public string Body { get; set; }
        public List<string> Tags { get; set; }
        public bool Published { get; set; }
    
    }
}