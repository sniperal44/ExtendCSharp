using System;
using System.Threading;
using System.Threading.Tasks;

namespace ExtendCSharp.ExtendedClass
{
    public class ProcessPlus
    {
	
        public ProcessStatus _Status;
        ProcessStatus Status { get { return _Status; } }

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

        public bool Async { get; set; }

        public delegate void ProcessStatusChanged(ProcessStatus s);
        public delegate void ProcessStatusNewLine(String line);


        public event ProcessStatusChanged OnStatusChanged;
        public event ProcessStatusNewLine OnNewLine;



        public ProcessPlus(String Command, String Params = "")
        {
            _Command = Command;
            _Params = Params;
        }

        public async Task Start()
        {
           
            System.Diagnostics.Process pProcess = new System.Diagnostics.Process();
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
                pProcess.ErrorDataReceived += (sender, args) =>
                {
                    if (OnNewLine != null)
                        OnNewLine(args.Data);
                };
            }

            pProcess.Start();
            if (RedirectStandardOutput)
            {
                pProcess.BeginOutputReadLine();
                pProcess.BeginErrorReadLine();
            }
            SetProcessStatusInvoke(ProcessStatus.Running);

            
           

            if (Async)
            {
                new Thread(() =>
                {
                    pProcess.WaitForExit();
                    SetProcessStatusInvoke(ProcessStatus.Stop);
                }).Start();
            }
            else
            {
                await pProcess.WaitForExitAsync();
                SetProcessStatusInvoke(ProcessStatus.Stop);
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
