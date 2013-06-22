#region Info
/*Optimus Hackathon
 Owner : Stallions
 Date : 22/06/2013
 */

#endregion

using System;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading;


namespace Hacksercise2
{
    public class MapApiProcess
    {
        // This class will simply count to ten in ten seconds
        private ProcessorDetail ProcessorDetail;

        public MapApiProcess(ProcessorDetail processor_detail)
        {
            ProcessorDetail = processor_detail;
            Process();
        }

        public void Process()
        {
            ProcessManager.GetProcessorDetail(ProcessorDetail.ID).UpdateStatusText("Processing has started");

            try
            {
                int rowcount = 0;
                int processed = 0;
                int total = 0;

                total = 10000;

                ProcessManager.GetProcessorDetail(ProcessorDetail.ID).UpdateStatusText("Commencing processing loop");
                while (true)
                {
                    rowcount++;
                    processed++;
                    if (UpdateStatus(processed, total))
                    {
                        int activeProcesses = ProcessManager.GetProcessCount();
                        ProcessManager.GetProcessorDetail(ProcessorDetail.ID).UpdateCounts(processed, total);

                        Console.WriteLine(string.Format("Processed {0} of {1}", processed, total));
                        Console.WriteLine(string.Format("Currently running {0} Process", activeProcesses));

                        if (!ProcessManager.Continue(ProcessorDetail.ID))
                            break;

                        int tic = int.Parse(DateTime.Now.Ticks.ToString().Substring(0, 9));
                        int unique_proc_id = new Random(tic).Next();

                        // create a new AsyncProcessorDetail
                        ProcessorDetail detail = new ProcessorDetail(unique_proc_id, 0, "MapApiProcess", 1);

                        // call AsyncProcessManager and pass it the AsyncProcessorDetail to be processed
                        ProcessManager.StartAsyncProcess(detail);
                    }
                    if (processed == total)
                        break;

                    // sleep for a second
                    Thread.Sleep(2000);
                }

                if (!ProcessManager.Continue(ProcessorDetail.ID))
                    ProcessManager.GetProcessorDetail(ProcessorDetail.ID).UpdateStatusText("Processing cancelled");

                ProcessManager.GetProcessorDetail(ProcessorDetail.ID).UpdateStatusText("Processing is complete");
            }
            catch (Exception ex)
            {
                string error = ex.Message + ex.StackTrace;
            }

            ProcessManager.FinalizeProcess(ProcessorDetail.ID);
            Console.ReadLine();
        }


        private bool UpdateStatus(int processed, int total)
        {
            if (processed > 0)
                if (((processed % ProcessorDetail.RefreshStatusCount) == 0) || (processed == total))
                    return true;
                else return false;
            else
                return false;
        }

    }
}