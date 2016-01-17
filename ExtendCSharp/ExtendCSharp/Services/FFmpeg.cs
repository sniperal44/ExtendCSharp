using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtendCSharp.Services
{
    public class FFmpeg
    {
        bool Loaded = false;

        String _Path;
        String FFmpegPath { get { return _Path; } }

        public delegate void FFmpegStatusChanged(FFmpegStatus Status);
        public delegate void FFmpegProgressChanged(int Percent);

        public FFmpeg(String Path)
        {
            if(CheckValidFFmpeg(Path))
            {
                Loaded = true;
                _Path = Path;
            }
        }
        bool CheckValidFFmpeg(String Path)
        {
            return true;
        }




        public bool FlacToMp3(String Input,String Output,bool OverrideIfExist,FFmpegStatusChanged OnStatusChanged=null, FFmpegProgressChanged OnProgressChanged=null, bool Async = true )
        {
            if (!Loaded)
                return false;


            if (CheckValidInput(Input) && CheckValidOutput(Output))
            {
                if (File.Exists(Output))
                {
                    if (OverrideIfExist)
                        File.Delete(Output);
                    else
                        return false;
                }
                    
                MyProcess p = new MyProcess(_Path, "-i \"" + Input + "\" -ab 320k -map_metadata 0 -id3v2_version 3 \"" + Output + "\"");
                if (OnStatusChanged != null)
                {
                    p.OnStatusChanged += (ProcessStatus s) => {
                        if (s == ProcessStatus.Running)
                            OnStatusChanged(FFmpegStatus.Running);
                        else if (s == ProcessStatus.Stop)
                            OnStatusChanged(FFmpegStatus.Stop);
                    };
                }


                if (OnProgressChanged != null)
                {
                    bool AspettaLaDurata = false;
                    bool AspettaProgress = false;
                    long TotalMilliSec = 0;
                    p.OnNewLine += (string line) => {
                        if (line == null)
                            return;
                        else if (line.StartsWith("Input"))
                        {
                            AspettaLaDurata = true;
                            AspettaProgress = false;
                        }
                        else if (AspettaLaDurata && line.StartsWith("  Duration:"))
                        {
                          


                            line = line.RemoveLeft("  Duration: ");
                            line = line.SplitAndGetFirst(',');

                            string[] ss=line.Split(':', '.');
                            if (ss.Length == 4)
                            {
                                AspettaLaDurata = false;
                                AspettaProgress = true;
                                TotalMilliSec = ss[3].ParseInt() * 10 + ss[2].ParseInt() * 1000 + ss[1].ParseInt() * 60000 + ss[0].ParseInt() * 3600000;
                            }                   
                            else
                            {
                                TotalMilliSec = -1;
                                AspettaLaDurata = false;
                                AspettaProgress = false;
                            }
                        }
                        else if (AspettaProgress && line.StartsWith("frame"))
                        {
                            
                            line = line.Substring(line.IndexOf("time=") + 5, 11);
                            string[] ss = line.Split(':', '.');
                            if (ss.Length == 4)
                            {
                                long current = ss[3].ParseInt() * 10 + ss[2].ParseInt() * 1000 + ss[1].ParseInt() * 60000 + ss[0].ParseInt() * 3600000;
                                OnProgressChanged((int)((double)current / TotalMilliSec * 100));
                            }
                        }


                    };
                }
                
                p.UseShellExecute = false;
                p.RedirectStandardOutput = true;
                p.RedirectStandardError = true;
                p.CreateNoWindow = true;
                p.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

                p.Async = Async;
                p.Start();
            }
            return false;
        }



        



        bool CheckValidInput(String Path)
        {
            return true;
        }
        bool CheckValidOutput(String Path)
        {
            return true;
        }

    }
    public enum FFmpegStatus
    {
        Running,
        Stop,
    }


}
