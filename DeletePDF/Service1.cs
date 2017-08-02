using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace DeletePDF
{
    public partial class Service1 : ServiceBase
    {
        SimpleLogger logger = new SimpleLogger(true);
        static Timer _timer;
        static string _ScheduledRunningTime = ConfigurationManager.AppSettings["ScheduledRunningTime"].ToString();
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                logger.Info("OnStart method at :" + DateTime.Now);
                _timer = new Timer();
                _timer.Interval = 1 * 60 * 1000;//Every one minute
                _timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
                _timer.Start();
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Start Exception :{0}; \n{1}; \n{2}", ex.ToString(), ex.Message, ex.InnerException.Message));
            }
        }

        protected override void OnStop()
        {

        }

        static void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            string _CurrentTime = String.Format("{0}:{1}", DateTime.Now.Hour, DateTime.Now.Minute);
            if (_CurrentTime == _ScheduledRunningTime)
            {
                (new FileManager()).FindAndDelete();
            }
        }
    }
}
