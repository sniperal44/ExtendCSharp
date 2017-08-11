using ExtendCSharp.ExtendedClass;
using ExtendCSharp.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExtendCSharp.Services
{
    public class FFmpeg:IService
    {
        SystemService ss = null;

        bool _Loaded = false;
        public bool Loaded { get { return _Loaded; } }

        String _PathFFmpeg;
        String FFmpegPath { get { return _PathFFmpeg; } }


        String _PathMetaflac;
        String MetaflacPath { get { return _PathMetaflac; } }

        String JpgNameTemp = "out.jpg";

        public delegate void FFmpegConvertStatusChanged(FFmpegStatus Status, String Source, String Destination);
        public delegate void FFmpegConvertProgressChanged(int Percent,String Source,String Destination,FFmpegError Error= FFmpegError.nul);

        public delegate void FFmpegGetMetadataEnd(String Source,FFmpegMetadata metadata);

        public bool Initialize(String PathFFmpeg,String PathMetaflac)
        {
            if (!_Loaded && CheckValidFFmpeg(PathFFmpeg) && CheckValidMetaflac(PathMetaflac))
            {
                _Loaded = true;
                _PathFFmpeg = PathFFmpeg;
                _PathMetaflac = PathMetaflac;
            }
            return _Loaded;
        }
        public FFmpeg()
        {
            ss = ServicesManager.GetOrSet(() => { return new SystemService(); });
        }


        public void Reset()
        {
            _Loaded = false;
            _PathFFmpeg = null;
        }

        public bool CheckValidMetaflac(string pathMetaflac)
        {
            if (!ss.FileExist(pathMetaflac))
                return false;
            ProcessPlus p = new ProcessPlus(pathMetaflac);
            bool valid = false;
            p.OnNewLine += (string line) => {
                if (line == null)
                    return;
                else if (line.StartsWith("metaflac - Command-line FLAC metadata editor version"))
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
        public bool CheckValidFFmpeg(String Path)
        {
            if (!ss.FileExist(Path))
                return false;
            ProcessPlus p = new ProcessPlus(Path);
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

        public bool Mp3ToFlac(String Input, String Output, FFMpegMediaMetadataFlac ConversionParameters, bool OverrideIfExist, FFmpegConvertStatusChanged OnStatusChanged = null, FFmpegConvertProgressChanged OnProgressChanged = null, bool Async = true)
        {
            bool ret = true;
            Thread t = new Thread(() =>
            {
                if (!_Loaded)
                {
                    ret = false; return;
                }
                    

                if (ConversionParameters == null || ConversionParameters.SamplingRate == SamplingRateInfo.nul || ConversionParameters.SamplingRate == 0 || ConversionParameters.Bit == BitInfo.nul)
                {
                    ret = false; return;
                }

                if (CheckValidInput(Input) && CheckValidOutput(Output))
                {
                    if (File.Exists(Output))
                    {
                        if (OverrideIfExist)
                            File.Delete(Output);
                        else
                        {
                            ret = false; return;
                        }
                    }

                    if (OnStatusChanged != null)
                    {
                        OnStatusChanged(FFmpegStatus.Running, Input, Output);
                    }
                    ProcessPlus p = new ProcessPlus(_PathFFmpeg, "-i \"" + Input + "\" -map 0:1? -c copy " + JpgNameTemp+" -y");
                    p.UseShellExecute = false;
                    p.RedirectStandardOutput = false;
                    p.RedirectStandardError = false;


                    p.CreateNoWindow = true;
                    p.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    p.Async = false;
                    p.Start();






                    p = new ProcessPlus(_PathFFmpeg, "-i \"" + Input + "\" -map 0:0 -c:a:0 flac -map_metadata 0 -id3v2_version 3  -ar " + ConversionParameters.SamplingRate.ToStringReplace("_", "") + " \"" + Output + "\"");



                    if (OnProgressChanged != null)
                    {
                        bool AspettaLaDurata = false;
                        bool AspettaProgress = false;
                        long TotalMilliSec = 0;
                        p.OnNewLine += (string line) =>
                        {
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

                                string[] ss = line.Split(':', '.');
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
                            else if (AspettaProgress && line.Contains("No such file or directory"))
                            {
                                OnProgressChanged(-1, Input, Output, FFmpegError.DestFolderNotFound);
                            }
                            else if (AspettaProgress && (line.StartsWith("frame") || line.StartsWith("size")))
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
                    ss.CreateFolderSecure(ss.GetParent(Output));


                    p.Async = false;
                    p.Start();


                    if (ss.FileExist(JpgNameTemp))
                    {
                        p = new ProcessPlus(_PathMetaflac, "--import-picture-from=\"" + JpgNameTemp + "\" \"" + Output+"\"");
                        p.UseShellExecute = false;
                        p.RedirectStandardOutput = false;
                        p.RedirectStandardError = false;
                        p.CreateNoWindow = true;
                        p.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                        p.Async = false;
                        p.Start();
                    }

                    if (OnStatusChanged != null)
                    {
                        OnStatusChanged(FFmpegStatus.Stop, Input, Output);
                    }

                }
                else
                {
                    ret = false; return;
                }

                
                ret = true; 
                

            });
            t.Start();

            if (!Async)
                t.Join();

            return ret;

            /*
            
            ffmpeg.exe -i "07. Magic Box - Scream My Name (Radio  Edit).mp3" -map 0:1? -c copy OUT.jpg
            ffmpeg.exe -i "07. Magic Box - Scream My Name (Radio  Edit).mp3" -map 0:0 -c:a: 0 flac -map_metadata 0 -id3v2_version 3  out.flac
            metaflac --import-picture-from="OUT.jpg" out.flac
            
             */
        }
        public bool Mp3ToWav()
        {
            return false;
        }




        public bool WavtoMp3()
        {
            return false;
        }
        public bool WavtoFlac()
        {
            return false;
        }




        public bool FlacToWav()
        {
            return false;
        }

      
        public bool FlacToMp3(String Input,String Output,FFMpegMediaMetadataMp3 ConversionParameters, bool OverrideIfExist,FFmpegConvertStatusChanged OnStatusChanged=null, FFmpegConvertProgressChanged OnProgressChanged=null, bool Async = true )
        {
            if (!_Loaded)
                return false;

            if (ConversionParameters == null || ConversionParameters.SamplingRate == SamplingRateInfo.nul || ConversionParameters.SamplingRate == 0 || ConversionParameters.BitRateMp3 == 0)
                return false;

            if (CheckValidInput(Input) && CheckValidOutput(Output))
            {
                if (File.Exists(Output))
                {
                    if (OverrideIfExist)
                        File.Delete(Output);
                    else
                        return true;
                }

               


                ProcessPlus p = new ProcessPlus(_PathFFmpeg, "-i \"" + Input + "\" -map 0:0 -map 0:1? -c:a:0 libmp3lame  -ab "+ ConversionParameters.BitRateMp3 + "k -ar "+ConversionParameters.SamplingRate.ToStringReplace("_","")+" -map_metadata 0 -id3v2_version 3   -c:v copy \"" + Output + "\"");
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
                ss.CreateFolderSecure(ss.GetParent(Output));
                

                p.Async = Async;
                p.Start();
            }
            else
                return false;

            return true;
        }

        /// <summary>
        /// Consente di convertire un media da un formato all'altro, di default, un file lossy in lossless ( o da un lossy peggiore ad uno migliore ) 
        /// </summary>
        /// <param name="Source"> File sorgente </param>
        /// <param name="Destination"> file di destinazione ( i MediaMetadati verranno usati come parametri di conversione )</param>
        /// <param name="ForceConvertion">permette di forzare la conversione (viene ignorato l'ottimizzazione di conversione lossy -> lossless </param>
        /// <param name="OverrideIfExist"> permette di cancellare il file destinazione, se presente</param>
        /// <param name="OnStatusChanged"></param>
        /// <param name="OnProgressChanged"></param>
        /// <param name="Async"> Lancia il comando in modalità asincrona</param>
        /// <returns></returns>
        public  bool ConvertTo(ConvertionEntity Source, ConvertionEntity Destination,bool ForceConvertion, bool OverrideIfExist, FFmpegConvertStatusChanged OnStatusChanged = null, FFmpegConvertProgressChanged OnProgressChanged = null, bool Async = true)
        {
            
            if (!ForceConvertion)
            {
                if (Source.MediaMetadata is FFMpegMediaMetadataMp3)
                {
                    if (Destination.MediaMetadata is FFMpegMediaMetadataMp3)
                    {
                        if ((Source.MediaMetadata as FFMpegMediaMetadataMp3).BitRateMp3 < (Destination.MediaMetadata as FFMpegMediaMetadataMp3).BitRateMp3)
                        {
                            OnStatusChanged?.Invoke(FFmpegStatus.Running, Source.Path, Destination.Path);
                            bool b= ss.CopySecure(Source.Path, Destination.Path, OverrideIfExist, (double percent, ref bool cancelFlag)=> { OnProgressChanged?.Invoke((int)percent, Source.Path, Destination.Path); }, (bool copiato, Exception ex)=> 
                            {
                                if (copiato)
                                    OnProgressChanged?.Invoke(100, Source.Path, Destination.Path);
                                else
                                    OnProgressChanged?.Invoke(0, Source.Path, Destination.Path,FFmpegError.CopyError);

                            });
                            OnStatusChanged?.Invoke(FFmpegStatus.Stop, Source.Path, Destination.Path);

                            return b;

                        }
                    }
                    else if (Destination.MediaMetadata is FFMpegMediaMetadataFlac)
                    {

                        OnStatusChanged?.Invoke(FFmpegStatus.Running, Source.Path, Destination.Path);
                        bool b = ss.CopySecure(Source.Path, ss.ChangeExtension(Destination.Path, "mp3"), OverrideIfExist, (double percent, ref bool cancelFlag) => { if (OnProgressChanged != null) OnProgressChanged((int)percent, Source.Path, Destination.Path); }, (bool copiato, Exception ex) =>
                        {
                            if (copiato)
                                OnProgressChanged?.Invoke(100, Source.Path, Destination.Path);
                            else
                                OnProgressChanged?.Invoke(0, Source.Path, Destination.Path, FFmpegError.CopyError);
                        });

                        OnStatusChanged?.Invoke(FFmpegStatus.Stop, Source.Path, Destination.Path);

                        return b;
                    }
                    else if (Destination.MediaMetadata is FFMpegMediaMetadataWav)
                    {
                        OnStatusChanged?.Invoke(FFmpegStatus.Running, Source.Path, Destination.Path);
                        bool b = ss.CopySecure(Source.Path, ss.ChangeExtension(Destination.Path, "mp3"), OverrideIfExist, (double percent, ref bool cancelFlag) => { if (OnProgressChanged != null) OnProgressChanged((int)percent, Source.Path, Destination.Path); }, (bool copiato, Exception ex) =>
                        {
                            if (copiato)
                                OnProgressChanged?.Invoke(100, Source.Path, Destination.Path);
                            else
                                OnProgressChanged?.Invoke(0, Source.Path, Destination.Path, FFmpegError.CopyError);
                        });

                        OnStatusChanged?.Invoke(FFmpegStatus.Stop, Source.Path, Destination.Path);

                        return b;
                    }
                }
                else if (Source.MediaMetadata is FFMpegMediaMetadataFlac)
                {
                    if (Destination.MediaMetadata is FFMpegMediaMetadataFlac)
                    {
                        if ((Source.MediaMetadata as FFMpegMediaMetadataFlac).SamplingRate < (Destination.MediaMetadata as FFMpegMediaMetadataFlac).SamplingRate)
                        {
                            OnStatusChanged?.Invoke(FFmpegStatus.Running, Source.Path, Destination.Path);
                            bool b = ss.CopySecure(Source.Path, Destination.Path, OverrideIfExist, (double percent, ref bool cancelFlag) => { if (OnProgressChanged != null) OnProgressChanged((int)percent, Source.Path, Destination.Path); }, (bool copiato, Exception ex) =>
                            {
                                if (copiato)
                                    OnProgressChanged?.Invoke(100, Source.Path, Destination.Path);
                                else
                                    OnProgressChanged?.Invoke(0, Source.Path, Destination.Path, FFmpegError.CopyError);
                            });

                            OnStatusChanged?.Invoke(FFmpegStatus.Stop, Source.Path, Destination.Path);

                            return b;
                        }
                        else if ((Source.MediaMetadata as FFMpegMediaMetadataFlac).Bit < (Destination.MediaMetadata as FFMpegMediaMetadataFlac).Bit)
                        {
                            OnStatusChanged?.Invoke(FFmpegStatus.Running, Source.Path, Destination.Path);
                            bool b = ss.CopySecure(Source.Path, Destination.Path, OverrideIfExist, (double percent, ref bool cancelFlag) => { if (OnProgressChanged != null) OnProgressChanged((int)percent, Source.Path, Destination.Path); }, (bool copiato, Exception ex) =>
                            {
                                if (copiato)
                                    OnProgressChanged?.Invoke(100, Source.Path, Destination.Path);
                                else
                                    OnProgressChanged?.Invoke(0, Source.Path, Destination.Path, FFmpegError.CopyError);
                            });

                            OnStatusChanged?.Invoke(FFmpegStatus.Stop, Source.Path, Destination.Path);

                            return b;

                        }
                    }
                }
                else if (Source.MediaMetadata is FFMpegMediaMetadataWav)
                {
                    if (Destination.MediaMetadata is FFMpegMediaMetadataWav)
                    {
                        if ((Source.MediaMetadata as FFMpegMediaMetadataWav).SamplingRate < (Destination.MediaMetadata as FFMpegMediaMetadataWav).SamplingRate)
                        {
                            OnStatusChanged?.Invoke(FFmpegStatus.Running, Source.Path, Destination.Path);
                            bool b = ss.CopySecure(Source.Path, Destination.Path, OverrideIfExist, (double percent, ref bool cancelFlag) => { if (OnProgressChanged != null) OnProgressChanged((int)percent, Source.Path, Destination.Path); }, (bool copiato, Exception ex) =>
                            {
                                if (copiato)
                                    OnProgressChanged?.Invoke(100, Source.Path, Destination.Path);
                                else
                                    OnProgressChanged?.Invoke(0, Source.Path, Destination.Path, FFmpegError.CopyError);
                            });

                            OnStatusChanged?.Invoke(FFmpegStatus.Stop, Source.Path, Destination.Path);

                            return b;
                        }
                        
                    }
                }
            }



            //TODO: implementare le altre conversioni ( wav ) 
            if ( Source.MediaMetadata is FFMpegMediaMetadataFlac)
            {
                FFMpegMediaMetadataFlac ts = (Source.MediaMetadata as FFMpegMediaMetadataFlac);
                if (Destination.MediaMetadata is FFMpegMediaMetadataMp3)
                    return FlacToMp3(Source.Path, Destination.Path,(Destination.MediaMetadata as FFMpegMediaMetadataMp3), OverrideIfExist, OnStatusChanged, OnProgressChanged, Async);
                else if (Destination.MediaMetadata is FFMpegMediaMetadataFlac)
                {
                    FFMpegMediaMetadataFlac td = (Destination.MediaMetadata as FFMpegMediaMetadataFlac);
                    if (ts.Bit == td.Bit && ts.SamplingRate == td.SamplingRate)
                    {
                        OnStatusChanged?.Invoke(FFmpegStatus.Running, Source.Path, Destination.Path);
                        bool b = ss.CopySecure(Source.Path, Destination.Path, OverrideIfExist, (double percent, ref bool cancelFlag) => { if (OnProgressChanged != null) OnProgressChanged((int)percent, Source.Path, Destination.Path); }, (bool copiato, Exception ex) =>
                        {
                            if (copiato)
                                OnProgressChanged?.Invoke(100, Source.Path, Destination.Path);
                            else
                                OnProgressChanged?.Invoke(0, Source.Path, Destination.Path, FFmpegError.CopyError);
                        });

                        OnStatusChanged?.Invoke(FFmpegStatus.Stop, Source.Path, Destination.Path);

                        return b;
                    }
                    else
                    {
                        return Mp3ToFlac(Source.Path, Destination.Path, td, OverrideIfExist, OnStatusChanged, OnProgressChanged, Async);
                    }
                }
            }
            else if (Source.MediaMetadata is FFMpegMediaMetadataMp3)
            {
                FFMpegMediaMetadataMp3 ts = (Source.MediaMetadata as FFMpegMediaMetadataMp3);

                if (Destination.MediaMetadata is FFMpegMediaMetadataFlac)
                    return Mp3ToFlac(Source.Path, Destination.Path, (Destination.MediaMetadata as FFMpegMediaMetadataFlac), OverrideIfExist, OnStatusChanged, OnProgressChanged, Async);
                else if (Destination.MediaMetadata is FFMpegMediaMetadataMp3)
                {
                    FFMpegMediaMetadataMp3 td = (Destination.MediaMetadata as FFMpegMediaMetadataMp3);
                    if (ts.BitRateMp3 == td.BitRateMp3 && ts.SamplingRate == td.SamplingRate)
                    {
                        OnStatusChanged?.Invoke(FFmpegStatus.Running, Source.Path, Destination.Path);
                        bool b = ss.CopySecure(Source.Path, Destination.Path, OverrideIfExist, (double percent, ref bool cancelFlag) => { if (OnProgressChanged != null) OnProgressChanged((int)percent, Source.Path, Destination.Path); }, (bool copiato, Exception ex) =>
                        {
                            if (copiato)
                                OnProgressChanged?.Invoke(100, Source.Path, Destination.Path);
                            else
                                OnProgressChanged?.Invoke(0, Source.Path, Destination.Path, FFmpegError.CopyError);
                        });

                        OnStatusChanged?.Invoke(FFmpegStatus.Stop, Source.Path, Destination.Path);

                        return b;

                    }
                    else
                    {
                        return FlacToMp3(Source.Path, Destination.Path, td, OverrideIfExist, OnStatusChanged, OnProgressChanged, Async);
                    }
                }
            }

            return false;
        }



        /// <summary>
        /// Ottiene i Metadata da un file Media in modalità sincrona
        /// </summary>
        /// <param name="Input">Path del file Media</param>
        /// <returns></returns>
        public FFmpegMetadata GetMetadata(String Input)
        {
            if (!_Loaded)
                return null;

            if (CheckValidInput(Input))
            {
                ProcessPlus p = new ProcessPlus(_PathFFmpeg, "-i \"" + Input + "\"");
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
                    else if (line.StartsWith("    title           : "))
                    {
                        temp.Title = line.RemoveLeft("    title           : ").Trim();
                    }


                    else if (line.StartsWith("    ARTIST          : "))
                    {
                        temp.Artist = line.RemoveLeft("    ARTIST          : ").Trim();
                    }
                    else if (line.StartsWith("    artist          :"))
                    {
                        temp.Artist = line.RemoveLeft("    artist          :").Trim();
                    }


                    else if (line.StartsWith("    ALBUM           : "))
                    {
                        temp.Album = line.RemoveLeft("    ALBUM           : ").Trim();
                    }
                    else if (line.StartsWith("    album           :"))
                    {
                        temp.Album = line.RemoveLeft("    album           :").Trim();
                    }


                    else if (line.StartsWith("    DATE            : "))
                    {
                        temp.Date = line.RemoveLeft("    DATE            : ").Trim();
                    }
                    else if (line.StartsWith("    date            : "))
                    {
                        temp.Date = line.RemoveLeft("    date            : ").Trim();
                    }

                    else if (line.StartsWith("    GENRE           : "))
                    {
                        temp.Genre = line.RemoveLeft("    GENRE           : ").Trim();
                    }
                    else if (line.StartsWith("    genre           :"))
                    {
                        temp.Genre = line.RemoveLeft("    genre           :").Trim();
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
                            temp.MediaMetadata.Duration = sss[0].RemoveLeft("  Duration: ").Trim();
                            temp.MediaMetadata.start = sss[1].RemoveLeft(" start: ").Trim();
                            temp.MediaMetadata.bitrate = sss[2].RemoveLeft(" bitrate: ").Trim();
                        }
                    }
                    else if (line.StartsWith("    Stream"))
                    {
                        String[] sss = line.Split(':');
                        if (sss.Length == 4 && sss[2].Contains("Audio"))
                        {

                            sss = sss[3].Split(',');
                            if(sss.Length>1)
                            {
                                if(sss[0].Contains("flac"))
                                {
                                    temp.MediaMetadata = new FFMpegMediaMetadataFlac(temp.MediaMetadata);

                                    try
                                    {
                                        (temp.MediaMetadata  as FFMpegMediaMetadataFlac).SamplingRate= (SamplingRateInfo) Enum.Parse(typeof(SamplingRateInfo), "_" + sss[1].RemoveRight("Hz", " ").Trim());

                                        if (sss[3].Contains("("))
                                        {
                                            string bittemp = sss[3].Substring("(", ")").RemoveRight("bit", " ").Trim();

                                            (temp.MediaMetadata as FFMpegMediaMetadataFlac).Bit = (BitInfo)Enum.Parse(typeof(BitInfo), "_" + bittemp);
                                        }

                                    }
                                    catch (Exception ex){

                                        StreamWriter sw = File.AppendText("tetttmp.txt");
                                        sw.WriteLine(Input + " " + ex.Message );
                                        sw.Close();
                                    }

                                }
                                else if (sss[0].Contains("mp3"))
                                {
                                    temp.MediaMetadata = new FFMpegMediaMetadataMp3(temp.MediaMetadata);
                                    try
                                    {
                                        (temp.MediaMetadata as FFMpegMediaMetadataMp3).SamplingRate = (SamplingRateInfo)Enum.Parse(typeof(SamplingRateInfo), "_" + sss[1].RemoveRight("Hz", " ").Trim());

                                        string bitrate = sss[4].RemoveRight("kb/s", " ").Trim();
                                        int.TryParse(bitrate, out (temp.MediaMetadata as FFMpegMediaMetadataMp3).BitRateMp3);


                                    }
                                    catch (Exception ex) {
                                        //MessageBox.Show(ex.Message);
                                        StreamWriter sw = File.AppendText("tetttmp.txt");
                                        sw.WriteLine(Input + " " + ex.Message);
                                        sw.Close();
                                    }


                                }
                                else if (sss[0].Contains("pcm"))
                                {
                                    temp.MediaMetadata = new FFMpegMediaMetadataWav(temp.MediaMetadata);
                                    try
                                    {
                                        (temp.MediaMetadata as FFMpegMediaMetadataWav).SamplingRate = (SamplingRateInfo)Enum.Parse(typeof(SamplingRateInfo), "_" + sss[1].RemoveRight("Hz", " ").Trim());


                                    }
                                    catch (Exception ex) {
                                        StreamWriter sw = File.AppendText("tetttmp.txt");
                                        sw.WriteLine(Input + " " + ex.Message);
                                        sw.Close();
                                    }
                                }
                            }
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
        public void GetMetadata(String Input, FFmpegGetMetadataEnd OnGetMetadataEnd = null)
        {
            new Thread(() =>
            {
                OnGetMetadataEnd(Input, GetMetadata(Input));
            }).Start();     
        }


        bool CheckValidInput(String Path)
        {
            if(ss.FileExist(Path))
                return true;
            return false;
        }
        bool CheckValidOutput(String Path)
        {

            return true;

        }

    }


    public class ConvertionEntity : ICloneablePlus
    {
        public FFMpegMediaMetadata MediaMetadata;
        public String Path;
        public ConvertionEntity(String Path, FFMpegMediaMetadata MediaMetadata)
        {
            this.Path = Path;
            this.MediaMetadata = MediaMetadata;
        }

        object ICloneablePlus.Clone()
        {
            ConvertionEntity t = new ConvertionEntity(Path, MediaMetadata.CloneClass());
            return t;
        }
        public ConvertionEntity CloneClass()
        {
            return (ConvertionEntity)(this as ICloneablePlus).Clone();
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
        public FFMpegMediaMetadata MediaMetadata=null;

        public FFmpegMetadata()
        {
            MediaMetadata = new FFMpegMediaMetadata();
        }

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
            if (MediaMetadata == null)
                t.MediaMetadata = null;
            else
                t.MediaMetadata = MediaMetadata.CloneClass();
            return t;
        }

        public FFmpegMetadata CloneClass()
        {
            return (FFmpegMetadata)(this as ICloneablePlus).Clone();
        }
    }

    public class FFMpegMediaMetadata : ICloneablePlus
    {
        public String Duration = "";
        public String start = "";
        public String bitrate = "";

        public FFMpegMediaMetadata()
        {
            
        }
        public FFMpegMediaMetadata(FFMpegMediaMetadata Clone)
        {
            Duration = Clone.Duration;
            start = Clone.start;
            bitrate = Clone.bitrate;
        }
        object ICloneablePlus.Clone()
        {
            FFMpegMediaMetadata t = new FFMpegMediaMetadata(this);
            return t;
        }

        public FFMpegMediaMetadata CloneClass()
        {
            return (FFMpegMediaMetadata)(this as ICloneablePlus).Clone();
        }

        public virtual String GetDefaultExtension()
        {
            return "";
        }
    }
    public class FFMpegMediaMetadataMp3 : FFMpegMediaMetadata, ICloneablePlus
    {
        public int BitRateMp3;
        public SamplingRateInfo SamplingRate;

        public FFMpegMediaMetadataMp3()
        {

        }
        public FFMpegMediaMetadataMp3(FFMpegMediaMetadata data):base(data)
        {
            if (data is FFMpegMediaMetadataMp3)
            {
                BitRateMp3 = (data as FFMpegMediaMetadataMp3).BitRateMp3;
                SamplingRate = (data as FFMpegMediaMetadataMp3).SamplingRate;
            }
        }

        object ICloneablePlus.Clone()
        {
            FFMpegMediaMetadataMp3 t = new FFMpegMediaMetadataMp3(this);
            return t;
        }
        public new FFMpegMediaMetadataMp3 CloneClass()
        {
            return (FFMpegMediaMetadataMp3)(this as ICloneablePlus).Clone();
        }

        public override string GetDefaultExtension()
        {
            return "mp3";
        }
    }
    public class FFMpegMediaMetadataFlac : FFMpegMediaMetadata, ICloneablePlus
    {
        public BitInfo Bit;
        public SamplingRateInfo SamplingRate;

        public FFMpegMediaMetadataFlac()
        {

        }
        public FFMpegMediaMetadataFlac(FFMpegMediaMetadata data):base(data)
        {
            if (data is FFMpegMediaMetadataFlac)
            {
                Bit = (data as FFMpegMediaMetadataFlac).Bit;
                SamplingRate = (data as FFMpegMediaMetadataFlac).SamplingRate;
            }
        }


        object ICloneablePlus.Clone()
        {
            FFMpegMediaMetadataFlac t = new FFMpegMediaMetadataFlac(this);
            return t;
        }
        public new FFMpegMediaMetadataFlac CloneClass()
        {
            return (FFMpegMediaMetadataFlac)(this as ICloneablePlus).Clone();
        }
        public override string GetDefaultExtension()
        {
            return "flac";
        }
    }
    public class FFMpegMediaMetadataWav : FFMpegMediaMetadata, ICloneablePlus
    {
        public SamplingRateInfo SamplingRate;

        public FFMpegMediaMetadataWav()
        {

        }
        public FFMpegMediaMetadataWav(FFMpegMediaMetadata data) : base(data)
        {
            if(data is FFMpegMediaMetadataWav)
            {
                SamplingRate = (data as FFMpegMediaMetadataWav).SamplingRate;
            }
        }


        object ICloneablePlus.Clone()
        {
            FFMpegMediaMetadataWav t = new FFMpegMediaMetadataWav(this);
            return t;
        }
        public new FFMpegMediaMetadataWav CloneClass()
        {
            return (FFMpegMediaMetadataWav)(this as ICloneablePlus).Clone();
        }
        public override string GetDefaultExtension()
        {
            return "wav";
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
        CopyError = 2,
    }



   




    public enum BitInfo
    {
        nul,
        _16 = 1,
        _24 = 2,
        _32 = 4,
        _64=5,
    }
    public enum SamplingRateInfo
    {
        nul,
        _8000,
        _11025,
        _16000,
        _22050,
        _24000,
        _32000,
        _44100,
        _48000,
        _64000,
        _88200,
        _96000,
        _192000,
    }





}
