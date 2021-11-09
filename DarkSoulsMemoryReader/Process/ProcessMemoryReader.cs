using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace DarkSoulsMemoryReader
{

    public class ProcessMemoryReader
    {
        public string ProcessName;
        public IntPtr Handle;
        public IntPtr BaseAddress;
        private Process process;

        private const int PROCESS_WM_READ = 0x0010;

        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        private static extern bool ReadProcessMemory(IntPtr hProcess,
        IntPtr lpBaseAddress, [Out] byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);

        public ProcessMemoryReader(string processName) {
            ProcessName = processName;
        }

        public bool IsAttached
        {
            get
            {
                return process != null && Handle != IntPtr.Zero && BaseAddress != IntPtr.Zero;
            }
        }

        public void Attach() {
            var newProcess = Process.GetProcessesByName(ProcessName).FirstOrDefault();
            if (newProcess != null && newProcess != process) {
                try {
                    process = newProcess;
                    Handle = OpenProcess(PROCESS_WM_READ, false, process.Id);
                    BaseAddress = process.MainModule.BaseAddress;
                } catch (Exception) {
                    // Assume the process has just exited
                    Detach();
                }
            }
        }

        public void Detach() {
            process = null;
            Handle = IntPtr.Zero;
            BaseAddress = IntPtr.Zero;
        }

        public bool TryReadRawBytes(MemoryAddress address, int offset, int length, out byte[] buffer) {
            if (IsAttached) {
                try {
                    IntPtr realAddress = ReadIntPtrAtLocation(Handle, BaseAddress + address.BaseAddress);

                    if (address.Offsets.Length > 0) {
                        for (int i = 0; i < address.Offsets.Length - 1; i++) {
                            realAddress = ReadIntPtrAtLocation(Handle, realAddress + address.Offsets[i]);
                        }
                        realAddress += address.Offsets[address.Offsets.Length - 1];
                    }

                    int bytesRead = 0;
                    buffer = new byte[length];
                    ReadProcessMemory(Handle, realAddress + offset, buffer, buffer.Length, ref bytesRead);
                    return true;
                } catch (Exception) { }
            }

            buffer = new byte[0];
            return false;
        }

        // Helper function for reading the memory at the specified location and converting it to an IntPtr
        private static IntPtr ReadIntPtrAtLocation(IntPtr processHandle, IntPtr location) {
            int bytesRead = 0;
            byte[] buffer = new byte[8];
            ReadProcessMemory(processHandle, location, buffer, buffer.Length, ref bytesRead);
            return (IntPtr)BitConverter.ToInt64(buffer);
        }
    }
}
