using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Raven.Abstractions.Data;
using SugarTown.Models;


namespace DataImport
{
    class Program
    {
        static void Main(string[] args)
        {
            var documentStore = new Raven.Client.Embedded.EmbeddableDocumentStore { ConnectionStringName = "RavenDB" };
            documentStore.Initialize();

            var session = documentStore.OpenSession();

            documentStore.DatabaseCommands.DeleteByIndex("Raven/DocumentsByEntityName",
                                             new IndexQuery
                                             {
                                                 Query = session.Advanced.LuceneQuery<object>()
                                                 .WhereEquals("Tag", "Posts")
                                                 .ToString()
                                             },
                                             false);
          
            StreamReader re = File.OpenText("Data.txt");

            JsonTextReader reader = new JsonTextReader(re);
            JArray root = JArray.Load(reader);

            foreach (JObject o in root)
            {
                Post post = new Post();
                post.Id = o["Id"].ToObject<string>();
                post.Title = o["Title"].ToObject<string>();
                post.DateCreated = o["DateCreated"].ToObject<DateTime>();
                post.Body = "<p>"+o["Body"].ToObject<string>()+"</p>";

                if (o["Tags"] != null)
                {
                    List<string> tags = new List<string>();
                    var strippedData = o["Tags"].ToObject<string>().Split(',');
                    foreach (string s in strippedData)
                    {
                        tags.Add(s.Trim());
                    }

                    post.Tags = tags;
                }
                else
                {
                    post.Tags = null;
                }

                post.Published = o["Published"].ToObject<bool>();

                session.Store(post);

            }

            session.SaveChanges();
            Console.WriteLine("Data Imported");
            Console.ReadKey();
        }
    }


}
