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
                    MatgoService service = new MatgoService(args);
                    service.TestStartup(args);
                    AutoResetEvent dummy = new AutoResetEvent(false);
                    dummy.WaitOne();
                    service.TestStop();
                }
                else
                {
                    ServiceBase[] ServicesToRun;
                    ServicesToRun = new ServiceBase[]
                    {
                    new MatgoService(args)
                    };
                    ServiceBase.Run(ServicesToRun);
                }
            }
            catch (Exception e)
            {
                Log.Setup(args[0]);
                Log._log.FatalFormat("Main:{0}", e.ToString());
            }
            Application.Exit();
        }
    }
}
