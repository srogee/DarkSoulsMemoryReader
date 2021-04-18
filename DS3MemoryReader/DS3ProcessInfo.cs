using System;
using System.Diagnostics;
using System.Linq;

namespace DS3MemoryReader
{

    class DS3ProcessInfo
    {
        public IntPtr Handle;
        public IntPtr BaseAddress;
        private Process process;

        public void FindDS3Process() {
            Attach(Process.GetProcessesByName("DarkSoulsIII").FirstOrDefault());
        }

        public DS3ProcessInfo() {}

        public bool IsValid
        {
            get
            {
                return process != null && Handle != IntPtr.Zero && BaseAddress != IntPtr.Zero;
            }
        }

        public Process Process
        {
            get
            {
                return process;
            }
        }

        public void Attach(Process process) {
            if (process != null && process != this.process) {
                try {
                    this.process = process;
                    Handle = ProcessInterop.OpenProcess(ProcessInterop.PROCESS_WM_READ, false, process.Id);
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
    }
}
