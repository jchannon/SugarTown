using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.ModelBinding;
using Nancy.Responses.Negotiation;
using Nancy.ViewEngines;
using Raven.Client;
using SugarTown.Models.Raven;
using TinyIoC;

namespace SugarTown
{
    public class StartUp : IApplicationStartup
    {
        //private readonly SugarTownConfiguration diagnosticsConfiguration;
        //private readonly IRootPathProvider rootPathProvider;
        //private readonly IEnumerable<ISerializer> serializers;
        //private readonly NancyInternalConfiguration configuration;
        //private readonly IModelBinderLocator modelBinderLocator;
        //private readonly IEnumerable<IResponseProcessor> _responseProcessors;

        //public StartUp(SugarTownConfiguration diagnosticsConfiguration, IRootPathProvider rootPathProvider, IEnumerable<ISerializer> serializers, NancyInternalConfiguration configuration, IModelBinderLocator modelBinderLocator, IEnumerable<IResponseProcessor> responseProcessors)
        //{
        //    this.diagnosticsConfiguration = diagnosticsConfiguration;
        //    this.rootPathProvider = rootPathProvider;
        //    this.serializers = serializers;
        //    this.configuration = configuration;
        //    this.modelBinderLocator = modelBinderLocator;
        //    _responseProcessors = responseProcessors;
        //}

        public void Initialize(IPipelines pipelines)
        {
            SugarTown.Enable(pipelines);
            //SugarTown.Enable(diagnosticsConfiguration, pipelines, rootPathProvider, serializers, configuration, modelBinderLocator, _responseProcessors);
        }

    }

    public class ViewStartUp : IApplicationRegistrations
    {
        private readonly SugarTownViewResolver viewResolver;

        public ViewStartUp(IViewResolver resolver)
        {
            this.viewResolver = new SugarTownViewResolver(resolver);
        }

        public IEnumerable<TypeRegistration> TypeRegistrations
        {
            get { return null; }
        }

        public IEnumerable<CollectionTypeRegistration> CollectionTypeRegistrations
        {
            get { return null; }
        }

        public IEnumerable<InstanceRegistration> InstanceRegistrations
        {
            get { yield return new InstanceRegistration(typeof(IViewResolver), this.viewResolver); }
        }

       
    }
}