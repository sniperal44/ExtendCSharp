using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace ExtendCSharp.Classes
{
    public class ProcessWatcher
    {
        String processName;
        public EnumStatus Status { get; private set; }


        List<ManagementEventWatcher> Watchers;
        public ProcessWatcher(string processName)
        {
            this.processName = processName;
            Status  = EnumStatus.NotWatching;
            Watchers = new List<ManagementEventWatcher>();
            CreateWatchers();

        }

        private void CreateWatchers()
        {
            Watchers.Add(CreateWatchForProcessStart());
            Watchers.Add(CreateWatchForProcessEnd());
        }

        public void Start()
        {
            if (Status == EnumStatus.Watchin)
                return;

            foreach (ManagementEventWatcher w in Watchers)
                w.Start();
            Status = EnumStatus.Watchin;
        }
        public void Stop()
        {
            if (Status == EnumStatus.NotWatching)
                return;

            foreach (ManagementEventWatcher w in Watchers)
                w.Stop();
            Status = EnumStatus.NotWatching;
        }


        private ManagementEventWatcher CreateWatchForProcessStart()
        {
            string queryString =
                "SELECT TargetInstance" +
                "  FROM __InstanceCreationEvent " +
                "WITHIN  .025 " +
                " WHERE TargetInstance ISA 'Win32_Process' " +
            "   AND TargetInstance.Name = '" + processName + "'";


            /*queryString =
                "SELECT TargetInstance" +
                "  FROM __InstanceCreationEvent " +
                "WITHIN  .025 " +
                " WHERE TargetInstance ISA 'Win32_Process' "
                + "   AND TargetInstance.Name like '%'";*/


            // The dot in the scope means use the current machine
            string scope = @"\\.\root\CIMV2";

            // Create a watcher and listen for events
            ManagementEventWatcher watcher = new ManagementEventWatcher(scope, queryString);
            watcher.EventArrived += _ProcessStarted;
            return watcher;
        }

        private ManagementEventWatcher CreateWatchForProcessEnd()
        {
            string queryString =
                "SELECT TargetInstance" +
                "  FROM __InstanceDeletionEvent " +
                "WITHIN  .025 " +
                " WHERE TargetInstance ISA 'Win32_Process' " +
                "   AND TargetInstance.Name = '" + processName + "'";

           /* queryString =
                "SELECT TargetInstance" +
                "  FROM __InstanceDeletionEvent " +
                "WITHIN  .025 " +
                " WHERE TargetInstance ISA 'Win32_Process' "
                + "   AND TargetInstance.Name like '%'";*/


            // The dot in the scope means use the current machine
            string scope = @"\\.\root\CIMV2";

            // Create a watcher and listen for events
            ManagementEventWatcher watcher = new ManagementEventWatcher(scope, queryString);
            watcher.EventArrived += _ProcessEnded;
            return watcher;
        }

        private void _ProcessEnded(object sender, EventArrivedEventArgs e)
        {
            ManagementBaseObject targetInstance = (ManagementBaseObject)e.NewEvent.Properties["TargetInstance"].Value;
            string processName = targetInstance.Properties["Name"].Value.ToString();
            //Console.WriteLine(String.Format("{0} process ended", processName));
        }

        private void _ProcessStarted(object sender, EventArrivedEventArgs e)
        {
            ManagementBaseObject targetInstance = (ManagementBaseObject)e.NewEvent.Properties["TargetInstance"].Value;
            string processName = targetInstance.Properties["Name"].Value.ToString();
            //Console.WriteLine(String.Format("{0} process started", processName));
            ProcessStarted?.Invoke(this,processName);

        }

        public event EventHandler<String> ProcessStarted;
        public event EventHandler<String> ProcessEnded;


        public enum EnumStatus
        {
            Watchin,
            NotWatching
        }
    }
   
}
