#region Info
/*Optimus Hackathon
 Owner : Stallions
 Date : 22/06/2013
 */
#endregion

using System;
using System.Threading;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Hacksercise2
{
    public static class ProcessManager
    {
        public static List<ProcessorDetail> ProcDetails { get; set; }

        public static int StartAsyncProcess(ProcessorDetail input_detail)
        {
            if (ProcDetails == null)
                ProcDetails = new List<ProcessorDetail>();

            bool error = false;

            foreach (ProcessorDetail detail in ProcDetails)
            {
                if (detail.ID == input_detail.ID)
                {
                    // task is already being processed
                    if (detail.Processing)
                    {
                        error = true;
                    }
                }
            }

            if (!error)
            {
                // removing any existing instances of this detail in the list
                ProcDetails.RemoveAll(z => z.ID == input_detail.ID);

                // Add the detail to the list
                ProcDetails.Add(input_detail);

                // Start processor thread
                Thread processorThread = new Thread(new ThreadStart(StartProcessorThread));
                processorThread.IsBackground = false;
                processorThread.Start();

                return 0;
            }
            else
                return 1;
        }

        public class AsyncProcessorThread
        {
            // Main processor thread
            public int ID { get; set; }

            public AsyncProcessorThread()
            {
                try
                {
                    ID = GetNextIDToProcess();

                    ProcDetails.SingleOrDefault(z => z.ID == ID).Begin();

                    ProcessorDetail fd = ProcDetails.SingleOrDefault(z => z.ID == ID);

                    // Add a case statement for each of the processor classes you create
                    switch (fd.ProcessorType)
                    {
                        case "MapApiProcess":
                            MapApiProcess processorExample = new MapApiProcess(fd);
                            break;
                        default:
                            ProcDetails.SingleOrDefault(z => z.ID == ID).StatusText = "No ProcessorType found. Exiting..";
                            ProcDetails.SingleOrDefault(z => z.ID == ID).End();
                            break;
                    }
                }
                catch (Exception ex) { }
            }
        }

        private static void StartProcessorThread()
        {
            AsyncProcessorThread processor = new AsyncProcessorThread();
        }


        private static void UpdateCounts(int file_id, int processed, int total)
        {
            ProcDetails.SingleOrDefault(z => z.ID == file_id).UpdateCounts(processed, total);
        }

        public static List<ProcessorDetail> GetAllProcessDetails()
        {
            List<ProcessorDetail> pds = new List<ProcessorDetail>();
            foreach (ProcessorDetail pd in ProcDetails)
            {
                pds.Add(pd);
            }
            return pds;
        }

        public static void RemoveCompletedProcesses()
        {
            // remove any complete processor details in the list
            ProcDetails.RemoveAll(z => z.Complete == true);
        }
        

        private static int GetNextIDToProcess()
        {
            return ProcDetails.SingleOrDefault(z => z.Process == true).ID;
        }

        public static ProcessorDetail GetProcessorDetail(int processId)
        {
            try
            {
                return ProcDetails.SingleOrDefault(z => z.ID == processId);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static int GetProcessCount()
        {
            try
            {
                return ProcDetails.Count;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public static bool Continue(int id)
        {
            return !ProcDetails.SingleOrDefault(z => z.ID == id).Stop;
        }

        public static void StopProcessor(int id)
        {
            ProcDetails.SingleOrDefault(z => z.ID == id).Stop = true;
            ProcDetails.SingleOrDefault(z => z.ID == id).Processing = false;
        }

        public static void FinalizeProcess(int id)
        {
            ProcDetails.SingleOrDefault(z => z.ID == id).End();
        }

    }
}