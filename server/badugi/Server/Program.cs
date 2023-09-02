using log4net.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server
{
    static class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (Environment.UserInteractive)
                {
                    BadugiService service = new BadugiService(args);
                    service.TestStartup(args);
                    //service.TestStop();
                    AutoResetEvent dummy = new AutoResetEvent(false);
                    dummy.WaitOne();
                }
                else
                {
                    ServiceBase[] ServicesToRun;
                    ServicesToRun = new ServiceBase[]
                    {
                    new BadugiService(args)
                    };
                    ServiceBase.Run(ServicesToRun);
                }
            }
            catch (Exception e)
            {
                Log.Setup(args[0]);
                Log._log.FatalFormat("Main:{0}", e.ToString());
            }
        }
    }
}
