using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Threading;
using System.Threading.Tasks;

namespace ExtendCSharp.ExtendedClass
{
    public class ProcessPlus
    {
	
        private ProcessStatus _Status;
        public ProcessStatus Status { get { return _Status; } }

        String _Command, _Params;
        public String Command { get { return _Command; } }
        public String Params { get { return _Params; } }

		
        public bool UseShellExecute { get; set; }
        public bool RedirectStandardOutput { get; set; }
        public bool RedirectStandardError { get; set; }
        public bool RedirectStandardInput { get; set; }
        public bool CreateNoWindow { get; set; }
        public System.Diagnostics.ProcessWindowStyle WindowStyle { get; set; }
        public string WorkingDirectory { get; set; }

        System.Diagnostics.Process pProcess = null;



        public delegate void ProcessStatusChanged(ProcessStatus s);
        public delegate void ProcessStatusNewLine(String line);


        public event ProcessStatusChanged OnStatusChanged;
        public event ProcessStatusNewLine OnNewLine;



        public ProcessPlus(String Command, String Params = "")
        {
            _Command = Command;
            _Params = Params;
        }

        
        /// <summary>
        /// se viene richiamato con await, il metodo aspetta la fine del processo
        /// altrimenti viene eseguito tutto in un task
        /// </summary>
        /// <returns></returns>
        public async Task Start()
        {
            pProcess = new System.Diagnostics.Process();
            pProcess.StartInfo.FileName = Command;
            pProcess.StartInfo.Arguments = Params;

            pProcess.StartInfo.UseShellExecute = UseShellExecute;
            pProcess.StartInfo.RedirectStandardInput = RedirectStandardInput;
            pProcess.StartInfo.RedirectStandardOutput = RedirectStandardOutput;
            pProcess.StartInfo.RedirectStandardError = RedirectStandardError;
            pProcess.StartInfo.CreateNoWindow = CreateNoWindow;
            pProcess.StartInfo.WindowStyle = WindowStyle;
            pProcess.StartInfo.WorkingDirectory = WorkingDirectory;
            
            
            if (RedirectStandardOutput)
            {
                
                pProcess.OutputDataReceived += (sender, args) =>
                {
                    if (OnNewLine != null)
                        OnNewLine(args.Data);
                };
            }

            if (RedirectStandardError)
            {
                pProcess.ErrorDataReceived += (sender, args) =>
                {
                    if (OnNewLine != null)
                        OnNewLine(args.Data);
                };
            }

            try
            {
                pProcess.Start();
                if (RedirectStandardOutput)
                {
                    pProcess.BeginOutputReadLine();
                }
                if (RedirectStandardError)
                {
                    pProcess.BeginErrorReadLine();
                }
                SetProcessStatusInvoke(ProcessStatus.Running);

                


                while (!pProcess.HasExited)
                { 
                    await Task.Delay(10);
                }
                //pProcess.WaitForExit();
                SetProcessStatusInvoke(ProcessStatus.Stop);
            }
            catch(Exception ex)
            {

            }
            
            
        }



        public void Stop()
        { 
            if(pProcess!=null && Status!=ProcessStatus.Stop)
            {
                pProcess.Kill();
            }
        }

        private void SetProcessStatusInvoke(ProcessStatus s)
        {
            _Status=s;
            if (OnStatusChanged != null)
                OnStatusChanged(_Status);
        }

    }
    public enum ProcessStatus
    {
        Running,
        Stop,
    }
}
