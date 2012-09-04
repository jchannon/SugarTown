using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Nancy;
using Nancy.Authentication.Forms;
using Nancy.Bootstrapper;
using Nancy.Extensions;
using Nancy.ModelBinding;
using Nancy.Responses;
using Nancy.Responses.Negotiation;
using Nancy.Routing;
using Raven.Client;
using SugarTown.Models;
using SugarTown.Models.Raven;
using SugarTown.Modules;

namespace SugarTown
{
    public static class SugarTown
    {

        internal const string UrlPrefix = "/SugarTown";

        internal const string ResourcePrefix = UrlPrefix + "/Content/";
        internal const string ViewPrefix = UrlPrefix + "/Posts/";

        private const string PipelineKey = "__SugarTown";

        public static void Enable(IPipelines pipelines)
        {
            if (pipelines == null)
            {
                throw new ArgumentNullException("pipelines");
            }

            pipelines.BeforeRequest.AddItemToStartOfPipeline(
                new PipelineItem<Func<NancyContext, Response>>(
                    PipelineKey,
                    ctx =>
                    {
                        if (!ctx.Request.Path.StartsWith(UrlPrefix, StringComparison.OrdinalIgnoreCase))
                        {
                            return null;
                        }


                        if (ctx.Request.Path.StartsWith(ResourcePrefix, StringComparison.OrdinalIgnoreCase))// || (ctx.Request.Path.StartsWith(ViewPrefix, StringComparison.OrdinalIgnoreCase) && ctx.Request.Headers.Accept.Any(x => x.Item1 == "text/html")))
                        {
                            string resourceNamespace = string.Empty;
                            string path = string.Empty;

                            if (ctx.Request.Path.Contains(ResourcePrefix))
                            {
                                resourceNamespace = "SugarTown.Content";
                                path = Path.GetDirectoryName(ctx.Request.Url.Path.Replace(ResourcePrefix, string.Empty)) ?? string.Empty;
                            }
                            else
                            {
                                resourceNamespace = "SugarTown.Views";
                                path = Path.GetDirectoryName(ctx.Request.Url.Path.Replace(ViewPrefix, string.Empty)) ?? string.Empty;
                            }


                            if (!string.IsNullOrEmpty(path))
                            {
                                resourceNamespace += string.Format(".{0}", path.Replace('\\', '.'));
                            }

#if NONEMBEDDEDMODE
                            var fileName = Path.GetFileName(ctx.Request.Url.Path);
                            var directory = Path.GetDirectoryName(ctx.Request.Url.Path).Substring(11);

                            return new GenericFileResponse(directory + "\\" + fileName);
#else
                            return new EmbeddedFileResponse(
                                typeof(SugarTown).Assembly,
                                resourceNamespace,
                                Path.GetFileName(ctx.Request.Url.Path));
#endif
                        }


                        var formsAuthConfiguration =
                                new FormsAuthenticationConfiguration()
                                {
                                    RedirectUrl = SugarTown.UrlPrefix + "/login",
                                    UserMapper = new UserMapper(),
                                };

                        FormsAuthentication.Enable(pipelines, formsAuthConfiguration);

                        return null;

                    }));

        }
    }
}