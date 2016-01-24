using ExtendCSharp.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExtendCSharp.Services
{
    static public class FFmpeg
    {
        static bool _Loaded = false;
        public static bool Loaded { get { return _Loaded; } }

        static String _Path;
        static String FFmpegPath { get { return _Path; } }

        public delegate void FFmpegConvertStatusChanged(FFmpegStatus Status, String Source, String Destination);
        public delegate void FFmpegConvertProgressChanged(int Percent,String Source,String Destination,FFmpegError Error= FFmpegError.nul);

        public delegate void FFmpegGetMetadataEnd(String Source,FFmpegMetadata metadata);

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


        static public bool ToMp3(String Input,String Output,bool OverrideIfExist,FFmpegConvertStatusChanged OnStatusChanged=null, FFmpegConvertProgressChanged OnProgressChanged=null, bool Async = true )
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


                MyProcess p = new MyProcess(_Path, "-i \"" + Input + "\" -map 0:0 -map 0:1? -c:a:0 libmp3lame  -ab 320k -map_metadata 0 -id3v2_version 3   -c:v copy \"" + Output + "\"");
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

        /// <summary>
        /// Ottiene i Metadata da un file Media in modalità sincrona
        /// </summary>
        /// <param name="Input">Path del file Media</param>
        /// <returns></returns>
        static public FFmpegMetadata GetMetadata(String Input)
        {
            if (!_Loaded)
                return null;

            if (CheckValidInput(Input))
            {
                MyProcess p = new MyProcess(_Path, "-i \"" + Input + "\"");
                FFmpegMetadata temp = new FFmpegMetadata();
                p.OnNewLine += (string line) =>
                {

                    if (line == null)
                        return;
                    else if (line.StartsWith("    LANGUAGE        : "))
                    {
                        temp.Language = line.RemoveLeft("    LANGUAGE        : ").Trim();
                    }
                    else if (line.StartsWith("    YEAR            : "))
                    {
                        temp.Year = line.RemoveLeft("    YEAR            : ").Trim();
                    }
                    else if (line.StartsWith("    TITLE           : "))
                    {
                        temp.Title = line.RemoveLeft("    TITLE           : ").Trim();
                    }
                    else if (line.StartsWith("    ARTIST          : "))
                    {
                        temp.Artist = line.RemoveLeft("    ARTIST          : ").Trim();
                    }
                    else if (line.StartsWith("    ALBUM           : "))
                    {
                        temp.Album = line.RemoveLeft("    ALBUM           : ").Trim();
                    }
                    else if (line.StartsWith("    DATE            : "))
                    {
                        temp.Date = line.RemoveLeft("    DATE            : ").Trim();
                    }
                    else if (line.StartsWith("    GENRE           : "))
                    {
                        temp.Genre = line.RemoveLeft("    GENRE           : ").Trim();
                    }
                    else if (line.StartsWith("    COMMENT         : "))
                    {
                        temp.Comment = line.RemoveLeft("    COMMENT         : ").Trim();
                    }
                    else if (line.StartsWith("    track           : "))
                    {
                        temp.track = line.RemoveLeft("    track           : ").Trim();
                    }
                    else if (line.StartsWith("    ENSEMBLE        : "))
                    {
                        temp.Ensemble = line.RemoveLeft("    ENSEMBLE        : ").Trim();
                    }
                    else if (line.StartsWith("  Duration:"))
                    {
                        String[] sss = line.Split(',');
                        if (sss.Length == 3)
                        {
                            temp.Duration = sss[0].RemoveLeft("  Duration: ").Trim();
                            temp.start = sss[1].RemoveLeft(" start: ").Trim();
                            temp.bitrate = sss[2].RemoveLeft(" bitrate: ").Trim();
                        }
                    }
                };



                p.UseShellExecute = false;
                p.RedirectStandardOutput = true;
                p.RedirectStandardError = true;
                p.CreateNoWindow = true;
                p.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

                p.Async = false;
                p.Start();

                return temp;
            }
            else
                return null;

            
        }

        /// <summary>
        /// Ottiene i Metadata da un file Media in modalità Asincrona
        /// </summary>
        /// <param name="Input">Path del file Media</param>
        /// <returns></returns>
        static public void GetMetadata(String Input, FFmpegGetMetadataEnd OnGetMetadataEnd = null)
        {
            new Thread(() =>
            {
                OnGetMetadataEnd(Input, GetMetadata(Input));
            }).Start();     
        }


        static bool CheckValidInput(String Path)
        {
            if(SystemService.FileExist(Path))
                return true;
            return false;
        }
        static bool CheckValidOutput(String Path)
        {

            return true;

        }

    }
    public class FFmpegMetadata :ICloneablePlus
    {
        public String Language = "";
        public String Year = "";
        public String Title = "";
        public String Artist = "";
        public String Album = "";
        public String Date = "";
        public String Genre = "";
        public String Comment = "";
        public String track = "";
        public String Ensemble = "";
        public String Duration = "";
        public String start = "";
        public String bitrate = "";


        object ICloneablePlus.Clone()
        {
            FFmpegMetadata t = new FFmpegMetadata();
            t.Language = Language;
            t.Year = Year;
            t.Title = Title;
            t.Artist = Artist;
            t.Album = Album;
            t.Date = Date;
            t.Genre = Genre;
            t.Comment = Comment;
            t.track = track;
            t.Ensemble = Ensemble;
            t.Duration = Duration;
            t.start = start;
            t.bitrate = bitrate;
            return t;
        }

        public FFmpegMetadata CloneClass()
        {
            return (FFmpegMetadata)(this as ICloneablePlus).Clone();
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
