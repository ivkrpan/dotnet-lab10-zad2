using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;

namespace dotnet_lab10_02
{
    public partial class Service1 : ServiceBase
    {

        EventLog log;

        bool aktivan;
        public Service1()
        {
            InitializeComponent();

            if (!EventLog.SourceExists("DOTNETLAB"))
            {
                EventSourceCreationData sourceData = new EventSourceCreationData("DOTNETLAB", "Application");
                EventLog.CreateEventSource(sourceData);
            }

            log = new EventLog();
            log.Log = "Application";
            log.Source = "DOTNETLAB";
        }

        protected override void OnStart(string[] args)
        {
            aktivan = true;

            log.WriteEntry("Pokrenut u: " + DateTime.Now.ToLongTimeString());

            WaitCallback delegat = new WaitCallback(nestoRadim);
            ThreadPool.QueueUserWorkItem(delegat);    
        }

        protected override void OnStop()
        {
            aktivan = false;
            log.WriteEntry("Zaustavljen u: " + DateTime.Now.ToLongTimeString());
        }


        void nestoRadim(object state)
        {
            while (aktivan)
            {
                log.WriteEntry("Nešto radim...");
                Thread.Sleep(10000); 
            }
        }
    }
}
