using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SugarTown.Modules
{
    public class LoginModule : SugarTownModule
    {
        public LoginModule() : base("/login")
        {
            Get["/"] = parameters => View["login"];
        }
    }
}