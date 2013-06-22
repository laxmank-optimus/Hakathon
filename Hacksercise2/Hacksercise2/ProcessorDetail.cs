#region Info
/*Optimus Hackathon
 Owner : Stallions
 Date : 22/06/2013
 */
#endregion

using System;

namespace Hacksercise2
{
    public class ProcessorDetail
    {
        // types, identifiers and control properties
        public int ID { get; set; }
        public string ProcessorType { get; set; }
        public bool Process { get; set; }
        public bool Processing { get; set; }
        public bool Stop { get; set; }
        public bool Complete { get; set; }
        public bool Test { get; set; }

        // properties for storing counts
        public int Total { get; set; }
        public int Processed { get; set; }

        // Status properties
        public int RefreshStatusCount { get; set; }
        public string StatusText { get; set; }


        //Timing Information
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public ProcessorDetail()
        {
            // constructor
        }

        public ProcessorDetail(int id, int total, string processor_type, int refresh_status_count)
        {
            Process = true;
            Processing = false;
            Stop = false;
            Complete = false;

            RefreshStatusCount = refresh_status_count;
            ID = id;
            Total = total;
            ProcessorType = processor_type;
        }


        public void Begin()
        {
            StartTime = DateTime.Now;
            Process = false;
            Processing = true;
        }

        public void UpdateStatusText(string status_text)
        {
            if (StatusText == null)
                StatusText = "";

            if (StatusText.Length > 0)
                StatusText += "<br />" + DateTime.Now.ToLongTimeString() + ": " + status_text;
            else
                StatusText = DateTime.Now.ToLongTimeString() + ": " + status_text;

            Console.WriteLine(status_text);
        }


        public void UpdateCounts(int processed, int total)
        {
            Processed = processed;
            Total = total;
        }

        public void End()
        {
            Stop = true;
            Processing = false;
            Complete = true;
            EndTime = DateTime.Now;

            TimeSpan.FromTicks(EndTime.Ticks - StartTime.Ticks).TotalSeconds.ToString();

            UpdateStatusText("Processing completed in "
                + TimeSpan.FromTicks(EndTime.Ticks - StartTime.Ticks).TotalSeconds.ToString()
                + " seconds");
        }
    }
}