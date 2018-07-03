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



        public int WriteMem(Process p, IntPtr address, params byte[] v)
        {
            var hProc = OpenProcess(ProcessAccessFlags.All, false, p.Id);
            int wtf = 0;
            WriteProcessMemory(hProc, address, v, (UInt32)v.Length, out wtf);

            CloseHandle(hProc);
            return wtf;
        }
        public int WriteMem(String ProcessName, IntPtr address, params byte[] v)
        {
            return WriteMem(Process.GetProcessesByName(ProcessName).FirstOrDefault(), address, v);
        }

        public byte[] ReadMem(Process p, IntPtr address, long v)
        {
            IntPtr processHandle = OpenProcess(ProcessAccessFlags.VMRead, false, p.Id);

            int bytesRead = 0;
            byte[] buffer = new byte[v];
            ReadProcessMemory(processHandle, address, buffer, buffer.Length, ref bytesRead);

            return buffer;
        }
        public byte[] ReadMem(String ProcessName, IntPtr address, long v)
        {
            return ReadMem(Process.GetProcessesByName(ProcessName).FirstOrDefault(), address, v);
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
