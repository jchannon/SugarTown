using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using Nancy.Bootstrapper;
using Nancy.ViewEngines;
using Raven.Client;
using SugarTown.Models.Raven;
using TinyIoC;

namespace SugarTown
{
    public class StartUp : IStartup
    {
        private readonly IViewLocator viewLocator;
        public StartUp(IViewLocator viewLocator)
        {
            this.viewLocator = viewLocator;
        }

        public IEnumerable<CollectionTypeRegistration> CollectionTypeRegistrations
        {
            get { return null; }
        }

        public void Initialize(IPipelines pipelines)
        {

        }

        public IEnumerable<InstanceRegistration> InstanceRegistrations
        {
            get
            {
                var impl = new SugarTownViewLocator(viewLocator);
                var test = new InstanceRegistration(typeof(IViewLocator), impl);

                return new[] { test };
            }
        }

        public IEnumerable<TypeRegistration> TypeRegistrations
        {

            get
            {
                //Func<string,IDocumentSession> factory = p => new DocumentSessionProvider().GetSession();
                //container.Register(factory);

                var fact = new TypeRegistration(typeof(IDocumentSession), typeof(DocumentSessionProvider));
                return new[] { fact };
            }
        }

    }

    

    public class SugarTownViewLocator : IViewLocator
    {
        private readonly IViewLocator _viewLocator;

        public SugarTownViewLocator(IViewLocator viewLocator)
        {
            _viewLocator = viewLocator;
        }

        #region IViewLocator Members

        public ViewLocationResult LocateView(string viewName, Nancy.NancyContext context)
        {
            //Find code to look in SugarTown using Reflection I assume??????
            //If it can't find it use default locator
            return _viewLocator.LocateView(viewName, context);
        }

        #endregion
    }
}