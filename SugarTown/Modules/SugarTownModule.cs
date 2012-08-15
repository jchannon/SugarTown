using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy;

namespace SugarTown.Modules
{
    public abstract class SugarTownModule : NancyModule
    {
        protected SugarTownModule()
            : base(SugarTown.UrlPrefix)
        {
        }

        protected SugarTownModule(string basePath)
            : base(SugarTown.UrlPrefix + basePath)
        {
           
        }

       

    }
}