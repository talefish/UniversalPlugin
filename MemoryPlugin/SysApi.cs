namespace Doffish.MemoryPlugin {
    using System;
    using System.Runtime.InteropServices;

    public static class SysApi {
        public const int PROCESS_QUERY_INFORMATION = 0x400;
        public const int PROCESSOR_ARCHITECTURE_AMD64 = 9;
        public const int PROCESSOR_ARCHITECTURE_IA64 = 6;
        public const int TH32CS_SNAPmodule = 8;
        public const int TH32CS_SNAPPROCESS = 0x200;
        public const int WM_LBUTTONDOWN = 0x201;
        public const int WM_LBUTTONUP = 0x202;
        public const int WM_SETTEXT = 12;

        [DllImport("user32")]
        public static extern int ClientToScreen(int hwnd, ref Point lpPoint);
        [DllImport("kernel32")]
        public static extern int CloseHandle(IntPtr hObject);
        [DllImport("kernel32")]
        public static extern IntPtr CreateToolhelp32Snapshot(int dwFlags, int th32ProcessID);
        [DllImport("user32")]
        public static extern int FindWindowEx(IntPtr hWnd1, int hWnd2, string lpsz1, string lpsz2);
        [DllImport("kernel32")]
        public static extern int FreeLibrary(int hLibModule);
        [DllImport("kernel32")]
        public static extern IntPtr GetCurrentProcess();
        [DllImport("user32")]
        public static extern int GetCursorPos(ref Point lpPoint);
        [DllImport("user32")]
        public static extern int GetForegroundWindow();
        [DllImport("kernel32")]
        public static extern int GetLastError();
        [DllImport("kernel32")]
        public static extern int GetModuleFileNameExA(int hProcess, int hModule, out string lpFileName, int nSize);
        [DllImport("kernel32")]
        public static extern int GetNativeSystemInfo(out SYSTEM_INFO lpSystemInfo);
        [DllImport("user32")]
        public static extern int GetParent(int hWnd);
        [DllImport("kernel32")]
        public static extern bool IsWow64Process(IntPtr hProcess, out bool Wow64Process);
        [DllImport("user32")]
        public static extern int MessageBoxTimeout(IntPtr hwnd, string lpText, string lpCaption, int wType, int wlange, int dwTimeout);
        [DllImport("kernel32")]
        public static extern int Module32First(IntPtr hSnapShot, ref MODULEENTRY32 lppe);
        [DllImport("kernel32")]
        public static extern int Module32Next(IntPtr hSnapShot, ref MODULEENTRY32 lppe);
        [DllImport("NTDLL")]
        public static extern int NtUnmapViewOfSection(int ProcessHandle, int BaseAddress);
        [DllImport("kernel32")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, int bInheritHandle, int dwProcessId);
        [DllImport("user32")]
        public static extern int PostMessage(IntPtr hWnd, int wMsg, int wParam, sPoint lParam);
        [DllImport("user32")]
        public static extern int PostMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);
        [DllImport("kernel32")]
        public static extern int Process32First(IntPtr hSnapShot, ref PROCESSENTRY32 uProcess);
        [DllImport("kernel32")]
        public static extern int Process32Next(IntPtr hSnapShot, ref PROCESSENTRY32 uProcess);
        [DllImport("kernel32")]
        public static extern int ReadProcessMemory(IntPtr hProcess, uint lpBaseAddress, out uint lpBuffer, int nSize, int lpNumberOfBytesWritten);
        [DllImport("user32")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, KeyModifiers fsModifiers, int vk);
        [DllImport("user32")]
        public static extern int SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);
        [DllImport("user32")]
        public static extern int SendMessage(IntPtr hWnd, int wMsg, int wParam, string lParam);
        [DllImport("kernel32")]
        public static extern int Sleep(int dwMilliseconds);
        [DllImport("user32")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        [DllImport("user32")]
        public static extern int WindowFromPoint(Point xPoint);
        [DllImport("kernel32")]
        public static extern int WriteProcessMemory(IntPtr hProcess, uint lpBaseAddress, ref uint lpBuffer, int nSize, int lpNumberOfBytesWritten);

        public enum KeyModifiers {
            Alt = 1,
            Ctrl = 2,
            None = 0,
            Norepeat = 0x4000,
            Shift = 4,
            WindowsKey = 8
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MODULEENTRY32 {
            public int dwSize;
            public int th32ModuleID;
            public int th32ProcessID;
            public int GlblcntUsage;
            public int ProccntUsage;
            public IntPtr modBaseAddr;
            public int modBaseSize;
            public IntPtr hModule;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x100)]
            public string szModule;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szExePath;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Point {
            public int X;
            public int Y;
            public Point(int X, int Y) {
                this.X = X;
                this.Y = Y;
            }
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct PROCESSENTRY32 {
            public int dwSize;
            public int cntUseage;
            public int th32ProcessID;
            public int th32DefaultHeapID;
            public int th32ModuleID;
            public int cntThreads;
            public int th32ParentProcessID;
            public int pcPriClassBase;
            public int swFlags;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szExeFile;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct sPoint {
            public short X;
            public short Y;
            public sPoint(short X, short Y) {
                this.X = X;
                this.Y = Y;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SYSTEM_INFO {
            public short wProcessorArchitecture;
            public short wReserved;
            public int dwPageSize;
            public int lpMinimumApplicationAddress;
            public int lpMaximumApplicationAddress;
            public int dwActiveProcessorMask;
            public int dwNumberOrfProcessors;
            public int dwProcessorType;
            public int dwAllocationGranularity;
            public short wProcessorLevel;
            public short wProcessorRevision;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct THREADENTRY32 {
            public int dwSize;
            public int cntusage;
            public int th32threadID;
            public int th32OwnerProcessID;
            public int tpBasePri;
            public int tpDeltaPri;
            public int dwFlags;
        }
    }
}

