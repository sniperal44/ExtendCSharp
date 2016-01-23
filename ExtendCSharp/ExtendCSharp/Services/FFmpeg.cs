using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtendCSharp.Services
{
    static public class FFmpeg
    {
        static bool _Loaded = false;
        public static bool Loaded { get { return _Loaded; } }

        static String _Path;
        static String FFmpegPath { get { return _Path; } }

        public delegate void FFmpegStatusChanged(FFmpegStatus Status, String Source, String Destination);
        public delegate void FFmpegProgressChanged(int Percent,String Source,String Destination,FFmpegError Error= FFmpegError.nul);

        static public bool Initialize(String Path)
        {
            if (!_Loaded && CheckValidFFmpeg(Path))
            {
                _Loaded = true;
                _Path = Path;
            }
            return _Loaded;
        }
        static public void Reset(String Path)
        {
            _Loaded = false;
            _Path = null;
        }


        static bool CheckValidFFmpeg(String Path)
        {
            MyProcess p = new MyProcess(Path);
            bool valid = false;
            p.OnNewLine += (string line) => {
                if (line == null)
                    return;
                else if (line.StartsWith("ffmpeg version"))
                {
                    valid = true;
                }
            };


            p.UseShellExecute = false;
            p.RedirectStandardOutput = true;
            p.RedirectStandardError = true;
            p.CreateNoWindow = true;
            p.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

            p.Async = false;
            p.Start();

            return valid;
        }


        static public bool ToMp3(String Input,String Output,bool OverrideIfExist,FFmpegStatusChanged OnStatusChanged=null, FFmpegProgressChanged OnProgressChanged=null, bool Async = true )
        {
            if (!_Loaded)
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


                MyProcess p = new MyProcess(_Path, "-i \"" + Input + "\" -map 0:0 -map 0:1 -c:a:0 libmp3lame  -ab 320k -map_metadata 0 -id3v2_version 3   -c:v copy \"" + Output + "\"");
                if (OnStatusChanged != null)
                {
                    p.OnStatusChanged += (ProcessStatus s) => {
                        if (s == ProcessStatus.Running)
                            OnStatusChanged(FFmpegStatus.Running, Input, Output);
                        else if (s == ProcessStatus.Stop)
                            OnStatusChanged(FFmpegStatus.Stop, Input, Output);
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
                        else if(AspettaProgress && line.Contains("No such file or directory"))
                        {
                            OnProgressChanged(-1, Input, Output,FFmpegError.DestFolderNotFound);
                        }
                        else if (AspettaProgress && ( line.StartsWith("frame") || line.StartsWith("size") ))
                        {
                            
                            line = line.Substring(line.IndexOf("time=") + 5, 11);
                            string[] ss = line.Split(':', '.');
                            if (ss.Length == 4)
                            {
                                long current = ss[3].ParseInt() * 10 + ss[2].ParseInt() * 1000 + ss[1].ParseInt() * 60000 + ss[0].ParseInt() * 3600000;
                                OnProgressChanged((int)((double)current / TotalMilliSec * 100), Input, Output);
                            }
                        }


                    };
                }
                
                p.UseShellExecute = false;
                p.RedirectStandardOutput = true;
                p.RedirectStandardError = true;
                p.CreateNoWindow = true;
                p.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                SystemService.CreateFolderSecure(SystemService.GetParent(Output));
                

                p.Async = Async;
                p.Start();
            }
            else
                return false;

            return true;
        }


        static bool CheckValidInput(String Path)
        {
            if(File.Exists(Path))
                return true;
            return false;
        }
        static bool CheckValidOutput(String Path)
        {

            return true;

        }

    }
    public enum FFmpegStatus
    {
        Running,
        Stop,
    }

    public enum FFmpegError
    {
        nul=0,
        DestFolderNotFound=1,
    }


}
