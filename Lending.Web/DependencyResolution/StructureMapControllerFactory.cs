using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Lending.Web.Controllers;
using StructureMap;

namespace Lending.Web.DependencyResolution
{
    public class StructureMapControllerFactory : DefaultControllerFactory
    {
        protected override IController GetControllerInstance(
            RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null)
                return null;

            var controller = new Container().GetInstance(controllerType);

            return controller as Controller;
        }
    }
}