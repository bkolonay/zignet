using System.Web.Http;
using System.Web.Http.Cors;
using Unity;
using Unity.Lifetime;
using ZigNet.Api.DependencyInjection;
using ZigNet.Api.Mapping;
using ZigNet.Business;
using ZigNet.Database;
using ZigNet.Database.EntityFramework;

namespace ZigNet.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);

            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // container registeration taken from here: https://docs.microsoft.com/en-us/aspnet/web-api/overview/advanced/dependency-injection
            var unityContainer = new UnityContainer();
            unityContainer.RegisterType<IZigNetEntitiesWrapper, ZigNetEntitiesWrapper>(new HierarchicalLifetimeManager());
            unityContainer.RegisterType<IZigNetEntitiesWriter, ZigNetEntitiesWriter>(new HierarchicalLifetimeManager());
            unityContainer.RegisterType<IZigNetEntitiesReadOnly, ZigNetEntitiesReadOnly>(new HierarchicalLifetimeManager());
            unityContainer.RegisterType<IZigNetDatabase, ZigNetEntityFrameworkDatabase>(new HierarchicalLifetimeManager());
            unityContainer.RegisterType<IZigNetBusiness, ZigNetBusiness>(new HierarchicalLifetimeManager());
            unityContainer.RegisterType<IZigNetApiMapper, ZigNetApiMapper>(new HierarchicalLifetimeManager());
            config.DependencyResolver = new UnityDependencyResolver(unityContainer);
        }
    }
}
