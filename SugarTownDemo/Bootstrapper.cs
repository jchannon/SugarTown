using System.Collections.Generic;
using System.Configuration;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Conventions;
using SugarTown;
using TinyIoC;

namespace SugarTownDemo
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            //IT DOESNT NOT COME THROUGH HERE FIRST WHERE I WANT IT TO
            //container.Register<ISugarTownSettings, SugarTownSettings>();
            base.ApplicationStartup(container, pipelines);
        }

        protected override void ConfigureRequestContainer(TinyIoCContainer container, NancyContext context)
        {
            //I SHOULD NOT HAVE TO DO THIS HERE BUT SOMETHING WEIRD IS GOING ON IN THAT WHEN THE APP STARTS UP IT COMES HERE FIRST
            container.Register<ISugarTownSettings, SugarTownSettings>();

            //I WOULD LIKE THE CONTEXT ITEM TO STILL BE HERE BUT AT THE MOMENT I HAVE TO ADD IT ON EVERY REQUEST
            if (!context.Items.ContainsKey("SugarTownSettings"))
            {
                ISugarTownSettings settings = container.Resolve<ISugarTownSettings>();
                settings.Settings = new Dictionary<string, string>();
                settings.Settings.Add("RouteName", ConfigurationManager.AppSettings["RouteName"]);

                context.Items.Add("SugarTownSettings", settings);
            }

            base.ConfigureRequestContainer(container, context);
        }

    }


    public class SugarTownSettings : ISugarTownSettings
    {
        public  Dictionary<string, string> Settings
        {
            get;
            set;
        }
    }
}