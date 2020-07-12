namespace Doffish.MemoryPlugin {
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;

    public static class MemoryHelper {
        public static Module GetModule(Process process, string ModuleName) {
            SysApi.MODULEENTRY32 lppe = new SysApi.MODULEENTRY32();
            if ((ModuleName == "") || (ModuleName == null)) {
                throw new Exception("Can't find module");
            }
            IntPtr hSnapShot = SysApi.CreateToolhelp32Snapshot(8, process.Id);
            if (hSnapShot != IntPtr.Zero) {
                lppe.dwSize = Marshal.SizeOf(typeof(SysApi.MODULEENTRY32));
                if (SysApi.Module32First(hSnapShot, ref lppe) != 0) {
                    do {
                        if (lppe.szModule.ToLower() == ModuleName.ToLower()) {
                            Module module1 = new Module {
                                ProcessID = lppe.th32ProcessID,
                                BaseAddr = lppe.modBaseAddr,
                                BaseSize = lppe.modBaseSize,
                                hModule = lppe.hModule,
                                szModule = lppe.szModule,
                                szExePath = lppe.szExePath
                            };
                            SysApi.CloseHandle(hSnapShot);
                            return module1;
                        }
                    }
                    while (SysApi.Module32Next(hSnapShot, ref lppe) != 0);
                }
                SysApi.CloseHandle(hSnapShot);
            }
            throw new Exception("Can't find module. code = " + SysApi.GetLastError());
        }

        public static uint GetModuleAddress(Process process, string ModuleName) {
            try {
                return (uint)((int)GetModule(process, ModuleName).BaseAddr);
            } catch {
                return 0;
            }
        }

        public static uint ReadMemory(Process Process, uint Address, int size) {
            uint num;
            SysApi.ReadProcessMemory(Process.Handle, Address, out num, size, 0);
            return num;
        }

        public static uint ReadMemory(Process Process, uint[] Offset, int size) {
            uint lpBuffer = 0;
            uint num2 = 0;
            uint lpBaseAddress = Offset[0];
            for (int i = 1; i < Offset.GetLength(0); i++) {
                SysApi.ReadProcessMemory(Process.Handle, lpBaseAddress, out num2, 4, 0);
                lpBaseAddress = Offset[i] + num2;
            }
            SysApi.ReadProcessMemory(Process.Handle, lpBaseAddress, out lpBuffer, size, 0);
            return lpBuffer;
        }

        public static uint ReadMemory(Process Process, string module, uint[] Offset, int size) {
            uint moduleAddress = GetModuleAddress(Process, module);
            Offset[0] += moduleAddress;
            return ReadMemory(Process, Offset, size);
        }

        public static int WriteMemory(Process Process, uint Address, uint Value, int size) {
            return SysApi.WriteProcessMemory(Process.Handle, Address, ref Value, size, 0);
        }

        public static int WriteMemory(Process Process, uint[] Offset, uint Value, int size) {
            uint lpBuffer = 0;
            uint lpBaseAddress = Offset[0];
            for (int i = 1; i < Offset.GetLength(0); i++) {
                SysApi.ReadProcessMemory(Process.Handle, lpBaseAddress, out lpBuffer, 4, 0);
                lpBaseAddress = Offset[i] + lpBuffer;
            }
            return SysApi.WriteProcessMemory(Process.Handle, lpBaseAddress, ref Value, size, 0);
        }

        public static int WriteMemory(Process Process, string module, uint[] Offset, uint Value, int size) {
            uint moduleAddress = GetModuleAddress(Process, module);
            Offset[0] += moduleAddress;
            return WriteMemory(Process, Offset, Value, size);
        }

        public class Module {
            public IntPtr BaseAddr;
            public int BaseSize;
            public IntPtr hModule;
            public int ProcessID;
            public string szExePath;
            public string szModule;
        }
    }
}

