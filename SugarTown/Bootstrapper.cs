using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Conventions;
using Raven.Client;
using SugarTown.Models.Raven;
using TinyIoC;

namespace SugarTown
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {

       

            base.ApplicationStartup(container, pipelines);
        }

        protected override void ConfigureConventions(NancyConventions conventions)
        {
            base.ConfigureConventions(conventions);

            //conventions.StaticContentsConventions.Add(
            //    StaticContentConventionBuilder.AddFile("/Raven.Studio.xap", "/Raven.Studio.xap")
            //);
        }

        protected override void ConfigureRequestContainer(TinyIoCContainer container, NancyContext context)
        {

            //var docSession = new DocumentSessionProvider();

            //container.Register(docSession).AsSingleton();
            Func<TinyIoCContainer, NamedParameterOverloads, IDocumentSession> factory = (ioccontainer, namedparams) => new DocumentSessionProvider().GetSession();
            container.Register(factory);     

            base.ConfigureRequestContainer(container, context);


        }
    }
}