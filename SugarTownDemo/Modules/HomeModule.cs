using Nancy;

namespace SugarTownDemo.Modules
{
    public class HomeModule : NancyModule
    {
         public HomeModule()
         {
             Get["/"] = parameters => View["Index"];
         }
    }
}