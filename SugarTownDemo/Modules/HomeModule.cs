using Nancy;

namespace SugarTownDemo.Modules
{
    public class HomeModule : NancyModule
    {
         public HomeModule() : base("/show")
         {
             Get["/"] = parameters => View["Index"];
         }
    }
}