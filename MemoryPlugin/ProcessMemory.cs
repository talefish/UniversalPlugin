namespace Doffish.MemoryPlugin {
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    public class ProcessMemory {
        private Dictionary<string, MemoryHelper.Module> moduleDict;
        private Process process;

        public ProcessMemory(Process process) {
            this.moduleDict = new Dictionary<string, MemoryHelper.Module>();
            this.process = process;
        }

        public ProcessMemory(int processID) {
            this.moduleDict = new Dictionary<string, MemoryHelper.Module>();
            this.process = Process.GetProcessById(processID);
        }

        public MemoryHelper.Module GetModule(string ModuleName) {
            if (this.moduleDict.ContainsKey(ModuleName)) {
                return this.moduleDict[ModuleName];
            }
            MemoryHelper.Module module = MemoryHelper.GetModule(this.process, ModuleName);
            this.moduleDict.Add(ModuleName, module);
            return module;
        }

        public uint GetModuleAddress(string ModuleName) {
            try {
                return (uint)((int)this.GetModule(ModuleName).BaseAddr);
            } catch {
                return 0;
            }
        }

        private void p_Exited(object sender, EventArgs e) {
            this.moduleDict.Clear();
        }

        public uint ReadMemory(uint Address, int size) {
            return MemoryHelper.ReadMemory(this.process, Address, size);
        }

        public uint ReadMemory(uint[] Offset, int size) {
            return MemoryHelper.ReadMemory(this.process, Offset, size);
        }

        public uint ReadMemory(string module, uint[] Offset, int size) {
            return MemoryHelper.ReadMemory(this.process, module, Offset, size);
        }

        public int WriteMemory(uint Address, uint Value, int size) {
            return MemoryHelper.WriteMemory(this.process, Address, Value, size);
        }

        public int WriteMemory(uint[] Offset, uint Value, int size) {
            return MemoryHelper.WriteMemory(this.process, Offset, Value, size);
        }

        public int WriteMemory(string module, uint[] Offset, uint Value, int size) {
            return MemoryHelper.WriteMemory(this.process, module, Offset, Value, size);
        }
    }
}

