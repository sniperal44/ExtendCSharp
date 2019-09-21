using ExtendCSharp.Interfaces;
using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace ExtendCSharp.Services
{
    public class ProcessMemoryService:IService
    {
        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenProcess(ProcessAccessFlags dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out int lpNumberOfBytesWritten);

        [DllImport("kernel32.dll")]
        private static extern Int32 CloseHandle(IntPtr hProcess);

        [DllImport("kernel32.dll")]
        private static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);




        public int WriteMem(IntPtr processHandle, IntPtr address, params byte[] v)
        {
            int wtf = 0;
            WriteProcessMemory(processHandle, address, v, (UInt32)v.Length, out wtf);
            return wtf;
        }


        public int WriteMem(Process p, IntPtr address, params byte[] v)
        {
            var hProc = OpenHandleWrite(p);
            int wtf=WriteMem(hProc, address, v);
            CloseHandle(hProc);
            return wtf;
        }
        public int WriteMem(String ProcessName, IntPtr address, params byte[] v)
        {
            return WriteMem(Process.GetProcessesByName(ProcessName).FirstOrDefault(), address, v);
        }






        public byte[] ReadMem(IntPtr processHandle, IntPtr address, long byteToRead)
        {

            int bytesRead = 0;
            byte[] buffer = new byte[byteToRead];
            ReadProcessMemory(processHandle, address, buffer, buffer.Length, ref bytesRead);

            return buffer;
        }

        /// <summary>
        /// Legge un numero di byte dalla memoria di un processo
        /// </summary>
        /// <param name="p">Processo da cui leggere</param>
        /// <param name="address">Indirizzo di partenza</param>
        /// <param name="byteToRead">Numero di byte da leggere</param>
        /// <returns></returns>
        public byte[] ReadMem(Process p, IntPtr address, long byteToRead)
        {
            IntPtr processHandle = OpenHandleRead(p);

            byte[] buffer = ReadMem(processHandle,address,byteToRead);


            CloseHandle_(processHandle);

            return buffer;
        }
        public byte[] ReadMem(String ProcessName, IntPtr address, long byteToRead)
        {
            return ReadMem(Process.GetProcessesByName(ProcessName).FirstOrDefault(), address, byteToRead);
        }

        





        /// <summary>
        /// Permette di aprire un Handle ad un processo in lettura ( da usare per evitare di chiudere ogni volta l'Handle ) 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public IntPtr OpenHandleRead(Process p)
        {
            IntPtr processHandle = OpenProcess(ProcessAccessFlags.VMRead, false, p.Id);
            return processHandle;
        }
        /// <summary>
        /// Permette di aprire un Handle ad un processo in scrittura ( da usare per evitare di chiudere ogni volta l'Handle ) 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public IntPtr OpenHandleWrite(Process p)
        {
            IntPtr processHandle = OpenProcess(ProcessAccessFlags.VMRead, false, p.Id);
            return processHandle;
        }

        public void CloseHandle_(IntPtr processHandle)
        {
            CloseHandle(processHandle);
        }



    }

    [Flags]
    enum ProcessAccessFlags : uint
    {
        All = 0x001F0FFF,
        Terminate = 0x00000001,
        CreateThread = 0x00000002,
        VMOperation = 0x00000008,
        VMRead = 0x00000010,
        VMWrite = 0x00000020,
        DupHandle = 0x00000040,
        SetInformation = 0x00000200,
        QueryInformation = 0x00000400,
        Synchronize = 0x00100000
    }
}
