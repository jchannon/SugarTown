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
        public void Initialize(IPipelines pipelines)
        {
            SugarTown.Enable(pipelines);
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