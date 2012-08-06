using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace SugarTown.Models
{
    public class Post
    {
        /// <summary>
        /// Id (in that exact case) is used by Raven.
        /// If on a Url of /dinners/edit/162 Nancy will bind the Id property to 162 so,
        /// we have to make sure that it starts with dinners/ so Raven can identify it properly
        /// </summary>
        private string id;
        public string Id
        {
            get { return id; }
            set
            {
                if (!value.StartsWith("posts"))
                    value = "posts/" + value;

                id = value;
            }
        }

        //[HiddenInput(DisplayValue = false)]
        [JsonIgnore]
        public int ID
        {
            get
            {
                return int.Parse(Id.Substring(Id.LastIndexOf("/", System.StringComparison.Ordinal) + 1));
            }
        }
        public string Title { get; set; }
        public DateTime DateCreated { get; set; }
        public string Body { get; set; }
        public IEnumerable<string> Tags { get; set; }
    }
}