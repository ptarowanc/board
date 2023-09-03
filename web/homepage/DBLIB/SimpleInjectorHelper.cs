using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DBLIB
{
    public static class SimpleInjectorHelper
    {
        public static void initialize(ref Container container)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(typeof(SimpleInjectorHelper));

            container.Register<GameEntities>(Lifestyle.Scoped);

            foreach (var type in Assembly.GetExecutingAssembly().GetTypes().Where(a => a.Namespace != null && a.Namespace.StartsWith("DBLIB.Service") && (a.Name.EndsWith("Service") || a.Name.EndsWith("Helper") )))
            {
                log.Info("registering service [" + type.Name + "] at [" + type.Namespace + "]");
                container.Register(type);
            }

        }
    }
}
