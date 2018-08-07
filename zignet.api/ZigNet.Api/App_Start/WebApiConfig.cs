using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using SimpleInjector.Lifestyles;
using System.Web.Http;
using System.Web.Http.Cors;
using ZigNet.Api.Mapping;
using ZigNet.Business;
using ZigNet.Database;
using ZigNet.Database.EntityFramework;
using ZigNet.Services;
using ZigNet.Services.EntityFramework;
using ZigNet.Services.EntityFramework.Mapping;

namespace ZigNet.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            container.Register<IZigNetEntitiesWrapper, ZigNetEntitiesWrapper>(Lifestyle.Scoped);

            container.Register<ISuiteResultMapper, SuiteResultMapper>(Lifestyle.Scoped);
            container.Register<ITemporaryTestResultsService, TemporaryTestResultsService>(Lifestyle.Scoped);
            container.Register<ISuiteResultService, SuiteResultService>(Lifestyle.Scoped);
            container.Register<ISuiteService, SuiteService>(Lifestyle.Scoped);
            container.Register<ILatestSuiteResultsService, LatestSuiteResultsService>(Lifestyle.Scoped);
            container.Register<ITestResultService, TestResultService>(Lifestyle.Scoped);

            container.Register<ISuiteBusinessProvider, SuiteBusinessProvider>(Lifestyle.Scoped);
            container.Register<ILatestSuiteResultsBusinessProvider, LatestSuiteResultsBusinessProvider>(Lifestyle.Scoped);
            container.Register<ITestResultBusinessProvider, TestResultBusinessProvider>(Lifestyle.Scoped);

            container.Register<IZigNetEntitiesWriter, ZigNetEntitiesWriter>(Lifestyle.Scoped);
            container.Register<IZigNetEntitiesReadOnly, ZigNetEntitiesReadOnly>(Lifestyle.Scoped);
            container.Register<IZigNetDatabase, ZigNetEntityFrameworkDatabase>(Lifestyle.Scoped);

            container.Register<IZigNetBusiness, ZigNetBusiness>(Lifestyle.Scoped);
            container.Register<IZigNetApiMapper, ZigNetApiMapper>(Lifestyle.Scoped);

            container.RegisterWebApiControllers(GlobalConfiguration.Configuration);
            container.Verify();
            GlobalConfiguration.Configuration.DependencyResolver =
                new SimpleInjectorWebApiDependencyResolver(container);

            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
