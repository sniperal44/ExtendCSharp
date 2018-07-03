using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace ExtendCSharp.ExtendedClass
{
    public class GifImage
    {
        //Image[] Frames;
        int FramesCount = 0;
        int delay = 0;
        Image img = null;

        Stopwatch watch = null;
        int CurrentFrame = 0;

        int _Width;
        public int Width
        {
            get => _Width;
        }

        int _Height;
        public int Height
        {
            get => _Height;
        }

        bool _Revers;
        public bool Revers
        {
            get => _Revers;
            set
            {
                _Revers = value;
            }
        }

        int totalTime = 0;
        Dictionary<int, int> listDelay = null;
        

        FrameDimension _dimension;
        public GifImage(Image i)
        {
            Init( i); 
        }
        public GifImage(String Path)
        {
            Init(Image.FromFile(Path));
        }

        private void Init(Image img)
        {
            this.img = img;
            _Width = img.Width;
            _Height = img.Height;

            _dimension = new FrameDimension(img.FrameDimensionsList[0]);
            FramesCount = img.GetFrameCount(_dimension);

            byte[] times = img.GetPropertyItem(0x5100).Value;
            delay = BitConverter.ToInt32(times, 4 * 0)*10;  //TODO: implementare velocità variabile
            watch = new Stopwatch();
            listDelay = new Dictionary<int, int>();

            int currentTime = 0;                        //trovo la durata totale della gif e memorizzo tutte le durate
            for (var i = 0; i < FramesCount; i++)
            {
                currentTime = BitConverter.ToInt32(times, i * 4);
                listDelay.Add(totalTime, currentTime);
                totalTime += currentTime;
            }
            totalTime *= 10;



        }


        public bool IsRunning
        {
            get
            {
                return watch.IsRunning;
            }
        }


        public void Start()
        {
            watch.Start();
        }
        public void Restart()
        {
            CurrentFrame = 0;
            watch.Restart();
        }
        public void Stop()
        {
            watch.Stop();
        }



        public Image GetImage()
        {
            if (watch.IsRunning)
            {
                int OldFrame = CurrentFrame;
                int FrameElapsed = (int)(watch.ElapsedMilliseconds / delay);
                if(_Revers)
                {
                    CurrentFrame = CurrentFrame - (FrameElapsed % FramesCount);
                    if (CurrentFrame < 0)
                        CurrentFrame += FramesCount;
                }
                else
                {
                    CurrentFrame = (CurrentFrame + FrameElapsed) % FramesCount;
                }
               
               
                if (OldFrame != CurrentFrame)
                {
                    watch.Restart();
                    img.SelectActiveFrame(_dimension, CurrentFrame);
                }
            }

            return img;
        }


        //TODO: penso ad un idea migliore per gestire i delay per ogni singolo frame
        private int FindDelayIndex(int CurrentTime)
        {
            int[] keys = listDelay.Keys.ToArray();
            //TODO :penso se implementare la binary search
            /*if (keys.Length == 0)
                return -1;
            else if( keys.Length==1)
            {
                return keys[0];
            }
            else
            {
                int i = 0, f = keys.Length - 1;
                //TODO: FINISCO BINARY SEARCH
                while (true)
                {
                    int m = (i + f) / 2;
                    if (CurrentTime>keys[m])
                        i = m;
                    else
                        f = m;
                }
            }*/


            if (keys.Length == 0)
                return -1;
            else if (keys.Length == 1)
            {
                return keys[0];
            }
            else
            {
                int l = keys.Length;
                for (int i = 0; i < l; i++)
                {
                    if (CurrentTime < keys[i] )
                        return i==0?-1:keys[i - 1];
                }
            }
            return -1;
        }

    }
}
