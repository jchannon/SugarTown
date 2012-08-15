using System;
using Nancy.ViewEngines;

namespace SugarTown
{
    public class SugarTownViewResolver : IViewResolver
    {
        private  SugarTownViewRenderer _renderer;
        private readonly IViewResolver _defaultResolver;

        public SugarTownViewResolver(IViewResolver defaultResolver)
        {
            _defaultResolver = defaultResolver;
        }

        /// <summary>
        /// Locates a view based on the provided information.
        /// </summary>
        /// <param name="viewName">The name of the view to locate.</param>
        /// <param name="model">The model that will be used with the view.</param>
        /// <param name="viewLocationContext">A <see cref="ViewLocationContext"/> instance, containing information about the context for which the view is being located.</param>
        /// <returns>A <see cref="ViewLocationResult"/> instance if the view could be found, otherwise <see langword="null"/>.</returns>
        public ViewLocationResult GetViewLocation(string viewName, dynamic model, ViewLocationContext viewLocationContext)
        {
            if (!viewLocationContext.Context.Request.Path.StartsWith(SugarTown.UrlPrefix, StringComparison.OrdinalIgnoreCase))
            {
                return _defaultResolver.GetViewLocation(viewName, model, viewLocationContext);
            }

            _renderer = new SugarTownViewRenderer(viewLocationContext.Context);

            var fullName = string.Concat(viewName, ".cshtml");

            var stream = _renderer.GetBodyStream(fullName);

            return _renderer.GetViewLocationResult(fullName, stream);
        }
    }
}