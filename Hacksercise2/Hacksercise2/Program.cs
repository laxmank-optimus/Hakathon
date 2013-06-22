#region Info
/*Optimus Hackathon
 Owner : Stallions
 Date : 22/06/2013
 */
#endregion


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using log4net.Config;

namespace Hacksercise2
{
    class Program
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(Program));

        static Program()
        {
           log4net.Config.DOMConfigurator.Configure();
        }

        static void Main(string[] args)
        {
            Program prg = new Program();
            Console.WriteLine("Process Started");

            //logging the start event
            logger.Info("MainProcess Started at " + DateTime.Now);

            //trigger the main process
            prg.StartAsynchProcess();
            
        }


        private void StartAsynchProcess()
        {
            // generate a unique id when a record of the process running
            int tic = int.Parse(DateTime.Now.Ticks.ToString().Substring(0, 9));
            int unique_proc_id = new Random(tic).Next();

            // create a new AsyncProcessorDetail
            ProcessorDetail detail = new ProcessorDetail(unique_proc_id, 0, "MapApiProcess", 1);

            // call AsyncProcessManager and pass it the AsyncProcessorDetail to be processed
            ProcessManager.StartAsyncProcess(detail);

        }
    }
}
