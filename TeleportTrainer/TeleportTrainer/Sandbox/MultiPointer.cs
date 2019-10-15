using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ReadWriteProcessMemory
{
    public sealed class MultiPointer
    {
        const int PROCESS_ALL_ACCESS = 0x1F0FFF;

        public MultiPointer (Process process, string moduleName, long baseOffset, long[] offsets)
        {
            Process = process;
            ModuleName = moduleName;
            BaseOffset = baseOffset;
            Offsets = offsets;
        }
        public Process Process { get; }
        public string ModuleName { get; }
        public long BaseOffset { get; }
        public long[] Offsets { get; }

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(int hProcess, long lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteProcessMemory(int hProcess, long lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesWritten);

        public float ReadValue()
        {
            IntPtr processHandle = OpenProcess(PROCESS_ALL_ACCESS, false, Process.Id);
            try{
                long address = GetAddress(processHandle);
                int bytesRead = 0;
                byte[] buffer = new byte[4];
                ReadProcessMemory((int)processHandle, address, buffer, buffer.Length, ref bytesRead);
                float value = BitConverter.ToSingle(buffer, 0);
                return Convert.ToSingle(value);
            }
            catch
            {
                MessageBox.Show("Couldn't read address");
            }
            return 0;
        }

        public void WriteValue(float value)
        {
            IntPtr processHandle = OpenProcess(PROCESS_ALL_ACCESS, false, Process.Id);
            try
            {
                long address = GetAddress(processHandle);
                int bytesRead = 0;
                byte[] buffer = BitConverter.GetBytes(value);
                WriteProcessMemory((int)processHandle, address, buffer, buffer.Length, ref bytesRead);
            }
            catch
            {
                MessageBox.Show("Couldn't read address");
            }
        }

        private long GetAddress(IntPtr processHandle)
        {
            ProcessModule module = null;
            foreach (ProcessModule item in Process.Modules)
            {
                if (item.ModuleName.ToLower() == ModuleName.ToLower())
                {
                    module = item;
                    break;
                }
            }

            int bytesRead = 0;
            byte[] buffer = new byte[8];

            long address = module.BaseAddress.ToInt64() + BaseOffset;
            ReadProcessMemory((int)processHandle, address, buffer, buffer.Length, ref bytesRead);
            long value = BitConverter.ToInt64(buffer, 0);

            for (int i = 0; i < Offsets.Length-1; i++)
            {
                //if (value <= 0x1000 || value >= 0xFFFFFFFF)
                //    throw new ArgumentNullException();
                address = value + Offsets[i];
                ReadProcessMemory((int)processHandle, address, buffer, buffer.Length, ref bytesRead);
                value = BitConverter.ToInt64(buffer, 0);
            }

            //if (value <= 0x1000 || value >= 0xFFFFFFFF)
            //    throw new ArgumentNullException();
            address = value + Offsets.Last();

            return address;
        }
    }
}
