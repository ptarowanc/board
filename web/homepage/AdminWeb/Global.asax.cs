using log4net;
using SimpleInjector;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace AdminWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();

            // Register your types, for instance:
            // container.Register<IUserRepository, SqlUserRepository>(Lifestyle.Scoped);
            DBLIB.SimpleInjectorHelper.initialize(ref container);

            // This is an extension method from the integration package.
            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());

            container.Verify();

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var log4netxml = new System.IO.FileInfo(Server.MapPath("log4net.xml"));
            log4net.Config.XmlConfigurator.Configure(log4netxml);

            ILog log = log4net.LogManager.GetLogger(typeof(MvcApplication));
            log.Info("관리자 웹이 로드되고 있습니다");
            log.Info("상품 이미지 경로 = " + Properties.Settings.Default.PRODUCT_IMG_PATH);
        }
    }
}
