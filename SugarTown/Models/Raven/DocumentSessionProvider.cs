using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Embedded;
using Raven.Database.Server;

namespace SugarTown.Models.Raven
{
    public class DocumentSessionProvider
    {
        private static EmbeddableDocumentStore _documentStore;

        private static EmbeddableDocumentStore DocumentStore
        {
            get { return (_documentStore ?? (_documentStore = CreateDocumentStore())); }
        }

        private static EmbeddableDocumentStore CreateDocumentStore()
        {
            //NonAdminHttp.EnsureCanListenToWhenInNonAdminContext(8080);
            EmbeddableDocumentStore store = new EmbeddableDocumentStore
            {
                ConnectionStringName = "RavenDB",
                UseEmbeddedHttpServer = true,
                //DataDirectory = "App_Data"
            };

            store.Initialize();

            return store;
        }

        public IDocumentSession GetSession()
        {
            var session = DocumentStore.OpenSession();
            return session;
        }
    }
}