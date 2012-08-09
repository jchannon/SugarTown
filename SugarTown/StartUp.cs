using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.ModelBinding;
using Nancy.ViewEngines;
using Raven.Client;
using SugarTown.Models.Raven;
using TinyIoC;

namespace SugarTown
{
    public class StartUp : IStartup
    {
        private readonly SugarTownConfiguration diagnosticsConfiguration;
        private readonly IRootPathProvider rootPathProvider;
        private readonly IEnumerable<ISerializer> serializers;
        private readonly NancyInternalConfiguration configuration;
        private readonly IModelBinderLocator modelBinderLocator;

        public StartUp(SugarTownConfiguration diagnosticsConfiguration, IRootPathProvider rootPathProvider, IEnumerable<ISerializer> serializers, NancyInternalConfiguration configuration, IModelBinderLocator modelBinderLocator)
        {
            this.diagnosticsConfiguration = diagnosticsConfiguration;
            this.rootPathProvider = rootPathProvider;
            this.serializers = serializers;
            this.configuration = configuration;
            this.modelBinderLocator = modelBinderLocator;
        }


        public IEnumerable<CollectionTypeRegistration> CollectionTypeRegistrations
        {
            get { return null; }
        }

        public void Initialize(IPipelines pipelines)
        {
            SugarTown.Enable(diagnosticsConfiguration, pipelines, rootPathProvider, serializers, configuration, modelBinderLocator);
        }

        public IEnumerable<InstanceRegistration> InstanceRegistrations
        {
            get
            {
                //var impl = new SugarTownViewLocator(viewLocator);
                //var test = new InstanceRegistration(typeof(IViewLocator), impl);

                //return new[] { test };

                return null;
            }
        }

        public IEnumerable<TypeRegistration> TypeRegistrations
        {

            get { return null; }
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