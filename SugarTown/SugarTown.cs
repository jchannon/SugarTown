using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Nancy;
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

        //public static void Enable(SugarTownConfiguration diagnosticsConfiguration, IPipelines pipelines, IRootPathProvider rootPathProvider, IEnumerable<ISerializer> serializers, NancyInternalConfiguration configuration, IModelBinderLocator modelBinderLocator, IEnumerable<IResponseProcessor> responseProcessors)
        public static void Enable(IPipelines pipelines)
        {
            if (pipelines == null)
            {
                throw new ArgumentNullException("pipelines");
            }

            //var keyGenerator = new DefaultModuleKeyGenerator();

            //var diagnosticsModuleCatalog = new SugarTownDiagnosticsModuleCatalog(keyGenerator, rootPathProvider, configuration, diagnosticsConfiguration);

            //var diagnosticsRouteCache = new RouteCache(diagnosticsModuleCatalog, keyGenerator, new DefaultNancyContextFactory());

            //var diagnosticsRouteResolver = new DefaultRouteResolver(
            //    diagnosticsModuleCatalog,
            //    new DefaultRoutePatternMatcher(),
            //    new SugarTownDiagnosticsModuleBuilder(rootPathProvider, serializers, modelBinderLocator),
            //    diagnosticsRouteCache, responseProcessors);

            //if (configuration == null)
            //{
            //    throw new ArgumentNullException("configuration");
            //}

            //if (!configuration.IsValid)
            //{
            //    throw new ArgumentException("Configuration is invalid", "configuration");
            //}

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

                            return new EmbeddedFileResponse(
                                typeof(SugarTown).Assembly,
                                resourceNamespace,
                                Path.GetFileName(ctx.Request.Url.Path));
                        }



                        //var renderer = new SugarTownDiagnosticsViewRenderer(ctx);

                        //IEnumerable<Post> model = new[]
                        //                {
                        //                    new Post()
                        //                        {
                        //                            Body = "<h1>Hi</hi>",
                        //                            DateCreated = DateTime.Now,
                        //                            Id = "/posts/3",
                        //                            Tags = new[] {"123,456"},
                        //                            Title = "Testing"
                        //                        },
                        //                         new Post()
                        //                        {
                        //                            Body = "<h1>Hi 2</hi>",
                        //                            DateCreated = DateTime.Now,
                        //                            Id = "/posts/32",
                        //                            Tags = new[] {"789"},
                        //                            Title = "Testing 2"
                        //                        }
                        //                };

                        //var negotiate =  renderer["Raw", model];

                        return null;
                        //return ShowMeTheSugar(ctx, diagnosticsRouteResolver, diagnosticsConfiguration);

                    }));

        }

        //private static Response ShowMeTheSugar(NancyContext ctx, IRouteResolver routeResolver, SugarTownConfiguration diagnosticsConfiguration)
        //{
        //    var resolveResult = routeResolver.Resolve(ctx);

        //    ctx.Parameters = resolveResult.Item2;
        //    var resolveResultPreReq = resolveResult.Item3;
        //    var resolveResultPostReq = resolveResult.Item4;

        //    ExecuteRoutePreReq(ctx, resolveResultPreReq);

        //    if (ctx.Response == null)
        //    {
        //        ctx.Response = resolveResult.Item1.Invoke(resolveResult.Item2);
        //    }

        //    if (ctx.Request.Method.ToUpperInvariant() == "HEAD")
        //    {
        //        ctx.Response = new HeadResponse(ctx.Response);
        //    }

        //    if (resolveResultPostReq != null)
        //    {
        //        resolveResultPostReq.Invoke(ctx);
        //    }



        //    // If we duplicate the context this makes more sense :)
        //    return ctx.Response;
        //}

        //private static void ExecuteRoutePreReq(NancyContext context, Func<NancyContext, Response> resolveResultPreReq)
        //{
        //    if (resolveResultPreReq == null)
        //    {
        //        return;
        //    }

        //    var resolveResultPreReqResponse = resolveResultPreReq.Invoke(context);

        //    if (resolveResultPreReqResponse != null)
        //    {
        //        context.Response = resolveResultPreReqResponse;
        //    }
        //}
    }
}

//namespace SugarTown
//{
    //    using System.Collections.Generic;
    //    using System.Linq;
    //    using Nancy.ModelBinding;
    //    using Nancy.Bootstrapper;
    //    using Nancy.Responses;
    //    using TinyIoC;

    //    internal class SugarTownDiagnosticsModuleCatalog : INancyModuleCatalog
    //    {
    //        private readonly TinyIoCContainer container;

    //        public SugarTownDiagnosticsModuleCatalog(IModuleKeyGenerator keyGenerator, IRootPathProvider rootPathProvider, NancyInternalConfiguration configuration, SugarTownConfiguration diagnosticsConfiguration)
    //        {
    //            this.container = ConfigureContainer(keyGenerator, rootPathProvider, configuration, diagnosticsConfiguration);
    //        }

    //        /// <summary>
    //        /// Get all NancyModule implementation instances - should be per-request lifetime
    //        /// </summary>
    //        /// <param name="context">The current context</param>
    //        /// <returns>An <see cref="IEnumerable{T}"/> instance containing <see cref="NancyModule"/> instances.</returns>
    //        public IEnumerable<NancyModule> GetAllModules(NancyContext context)
    //        {
    //            return this.container.ResolveAll<NancyModule>(false);
    //        }

    //        /// <summary>
    //        /// Retrieves a specific <see cref="NancyModule"/> implementation based on its key - should be per-request lifetime
    //        /// </summary>
    //        /// <param name="moduleKey">Module key</param>
    //        /// <param name="context">The current context</param>
    //        /// <returns>The <see cref="NancyModule"/> instance that was retrived by the <paramref name="moduleKey"/> parameter.</returns>
    //        public NancyModule GetModuleByKey(string moduleKey, NancyContext context)
    //        {
    //            return this.container.Resolve<NancyModule>(moduleKey);
    //        }

    //        private static TinyIoCContainer ConfigureContainer(IModuleKeyGenerator moduleKeyGenerator, IRootPathProvider rootPathProvider, NancyInternalConfiguration configuration, SugarTownConfiguration diagnosticsConfiguration)
    //        {
    //            var diagContainer = new TinyIoCContainer();

    //            diagContainer.Register<IModuleKeyGenerator>(moduleKeyGenerator);
    //            diagContainer.Register<IRootPathProvider>(rootPathProvider);
    //            diagContainer.Register<NancyInternalConfiguration>(configuration);
    //            diagContainer.Register<IModelBinderLocator, DefaultModelBinderLocator>();
    //            diagContainer.Register<IBinder, DefaultBinder>();
    //            diagContainer.Register<IFieldNameConverter, DefaultFieldNameConverter>();
    //            diagContainer.Register<BindingDefaults, BindingDefaults>();
    //            diagContainer.Register<ISerializer, DefaultJsonSerializer>();
    //            diagContainer.Register<SugarTownConfiguration>(diagnosticsConfiguration);

    //            Func<TinyIoCContainer, NamedParameterOverloads, IDocumentSession> factory = (ioccontainer, namedparams) => new DocumentSessionProvider().GetSession();
    //            diagContainer.Register(factory);

    //            foreach (var moduleType in AppDomainAssemblyTypeScanner.TypesOf<SugarTownModule>().ToArray())
    //            {
    //                diagContainer.Register(typeof(NancyModule), moduleType, moduleKeyGenerator.GetKeyForModuleType(moduleType)).AsMultiInstance();
    //            }

    //            return diagContainer;
    //        }
    //    }

    //    internal class SugarTownDiagnosticsModuleBuilder : INancyModuleBuilder
    //    {
    //        private readonly IRootPathProvider rootPathProvider;

    //        private readonly IEnumerable<ISerializer> serializers;
    //        private readonly IModelBinderLocator modelBinderLocator;

    //        public SugarTownDiagnosticsModuleBuilder(IRootPathProvider rootPathProvider, IEnumerable<ISerializer> serializers, IModelBinderLocator modelBinderLocator)
    //        {
    //            this.rootPathProvider = rootPathProvider;
    //            this.serializers = serializers;
    //            this.modelBinderLocator = modelBinderLocator;
    //        }

    //        /// <summary>
    //        /// Builds a fully configured <see cref="NancyModule"/> instance, based upon the provided <paramref name="module"/>.
    //        /// </summary>
    //        /// <param name="module">The <see cref="NancyModule"/> that shoule be configured.</param>
    //        /// <param name="context">The current request context.</param>
    //        /// <returns>A fully configured <see cref="NancyModule"/> instance.</returns>
    //        public NancyModule BuildModule(NancyModule module, NancyContext context)
    //        {
    //            CreateNegotiationContext(module, context);

    //            // Currently we don't connect view location, binders etc.
    //            module.Context = context;
    //            module.Response = new DefaultResponseFormatter(rootPathProvider, context, serializers);
    //            module.ModelBinderLocator = this.modelBinderLocator;

    //            return module;
    //        }

    //        private static void CreateNegotiationContext(NancyModule module, NancyContext context)
    //        {
    //            // TODO - not sure if this should be here or not, but it'll do for now :)
    //            context.NegotiationContext = new NegotiationContext
    //            {
    //                ModuleName = module.GetModuleName(),
    //                ModulePath = module.ModulePath,
    //            };
    //        }
    //    }
//}