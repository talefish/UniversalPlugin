namespace Doffish.MemoryPlugin {
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading;
    using System.Web.Script.Serialization;
    using System.Windows.Forms;

    public class Plugin : IDisposable {
        private IntPtr formIntPtr;
        private ProcessMemory gameMemory;
        private GameverItem gamever_ = new GameverItem();
        private Process process;
        private Profile profile;
        private string profilePath;
        private List<string> shortcut = new List<string>();
        public Dictionary<string, PluginItem> plugins { get; } = new Dictionary<string, PluginItem>();
        private Dictionary<string, BaseAddress> baseAddress = new Dictionary<string, BaseAddress>();
        private Dictionary<string, PluginMemory> lockedMemory = new Dictionary<string, PluginMemory>();
        private Dictionary<string, Memory> memories = new Dictionary<string, Memory>();
        private Dictionary<string, List<PluginMemory>> pluginMemory = new Dictionary<string, List<PluginMemory>>();

        public string actionColor {
            get {
                return this.profile.actionColor;
            }
        }

        public string defaultColor {
            get {
                return this.profile.defaultColor;
            }
        }

        public string gamever {
            get {
                return this.gamever_.version;
            }
        }

        public bool isInit { get; private set; }



        public string title {
            get {
                return this.profile.title;
            }
        }

        public string version {
            get {
                return this.profile.version;
            }
        }

        public Plugin(string profile, IntPtr intPtr) {
            try {
                this.PluginInit(profile, intPtr);
            } catch (Exception) {

                throw;
            }
        }

        public void Close() {
            this.Dispose();
        }

        public void Dispose() {
            this.Dispose(true);
        }

        protected virtual void Dispose(bool disposing) {
            if (disposing && this.isInit) {
                this.lockedMemory.Clear();
                this.memories.Clear();
                this.baseAddress.Clear();
                this.pluginMemory.Clear();
                this.plugins.Clear();
                this.lockedMemory = null;
                this.memories = null;
                this.baseAddress = null;
                this.pluginMemory = null;
                for (int i = 0; i < this.shortcut.Count<string>(); i++) {
                    SysApi.UnregisterHotKey(this.formIntPtr, i + 0x3e8);
                }
                this.isInit = false;
            } else {
                GC.SuppressFinalize(this);
            }
        }

        ~Plugin() {
            this.Dispose(false);
        }

        private string getFileVersion(string fileName) {
            FileInfo info = null;
            try {
                info = new FileInfo(fileName);
                if ((info != null) && info.Exists) {
                    FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(fileName);
                    object[] objArray1 = new object[] { versionInfo.ProductMajorPart, ".", versionInfo.ProductMinorPart, ".", versionInfo.ProductBuildPart, ".", versionInfo.ProductPrivatePart };
                    return string.Concat(objArray1);
                }
            } catch {
            }
            return "";
        }

        public string getMD5HashFromFile(string fileName) {
            try {
                FileStream inputStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                byte[] buffer = new MD5CryptoServiceProvider().ComputeHash(inputStream);
                inputStream.Close();
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < buffer.Length; i++) {
                    builder.Append(buffer[i].ToString("x2"));
                }
                return builder.ToString();
            } catch (Exception) {
                return "";
            }
        }

        public static int getProcessBits() {
            bool flag;
            SysApi.IsWow64Process(SysApi.GetCurrentProcess(), out flag);
            if ((GetSystemBits() == 0x40) && !flag) {
                return 0x40;
            }
            return 0x20;
        }

        public static int getProcessBits(Process process) {
            bool flag;
            IntPtr hProcess = SysApi.OpenProcess(0x400, 0, process.Id);
            SysApi.IsWow64Process(hProcess, out flag);
            SysApi.CloseHandle(hProcess);
            if ((GetSystemBits() == 0x40) && !flag) {
                return 0x40;
            }
            return 0x20;
        }

        private Process[] GetProcesses(string Processe) {
            if (Processe.ToLower().Substring(Processe.Length - 4) == ".exe") {
                Processe = Processe.Substring(0, Processe.Length - 4);
            }
            Process[] processesByName = Process.GetProcessesByName(Processe);
            if ((processesByName != null) && (processesByName.GetUpperBound(0) >= 0)) {
                return processesByName;
            }
            return Process.GetProcessesByName(Processe.Substring(0, 6).ToUpper() + "~1");
        }

        public static int GetSystemBits() {
            SysApi.SYSTEM_INFO system_info;
            SysApi.GetNativeSystemInfo(out system_info);
            if ((system_info.wProcessorArchitecture != 9) && (system_info.wProcessorArchitecture != 6)) {
                return 0x20;
            }
            return 0x40;
        }

        private void lockedMem() {
            do {
                try {
                    foreach (PluginMemory memory in this.lockedMemory.Values) {
                        string baseAddress = this.memories[memory.memory].baseAddress;
                        string module = this.baseAddress[baseAddress].module;
                        uint num = sting2uint(this.baseAddress[baseAddress].address);
                        if (module != "") {
                            num += this.gameMemory.GetModuleAddress(module);
                        }
                        this.memories[memory.memory].offsets.Insert(0, num.ToString());
                        uint[] offset = this.memories[memory.memory].offsets.ToUIntArray();
                        this.memories[memory.memory].offsets.RemoveAt(0);
                        this.gameMemory.WriteMemory(offset, sting2uint(memory.free), memory.size);
                    }
                    SysApi.Sleep(100);
                } catch {
                }
            }
            while (this.isInit);
        }

        private void PluginInit(string profile, IntPtr intPtr) {
            string input = this.readProfile(profile);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            this.profilePath = profile;
            this.profile = serializer.Deserialize<Profile>(input);
            Process[] processes = this.GetProcesses(this.profile.process);
            if (processes.Count<Process>() <= 0) {
                throw new Exception("没有找到指定进程");
            }
            this.process = processes[0];
            bool flag = false;
            foreach (GameverItem item in this.profile.gamever) {
                this.gameMemory = new ProcessMemory(this.process);
                int num = getProcessBits(this.process);
                if (getProcessBits() != num) {
                    throw new Exception("请使用" + num + " bit 外挂程序");
                }
                try {
                    MemoryHelper.Module module = this.gameMemory.GetModule(item.module);
                    string str2 = this.getMD5HashFromFile(module.szExePath);
                    if (item.fileMd5 != "") {
                        if (!(item.fileMd5.ToLower() == str2.ToLower())) {
                            continue;
                        }
                        this.gamever_ = item;
                        flag = true;
                        break;
                    }
                    if (this.getFileVersion(module.szExePath) == item.version) {
                        this.gamever_ = item;
                        flag = true;
                        break;
                    }
                } catch {
                }
            }
            if (!flag) {
                throw new Exception("不支持当前游戏版本");
            }
            foreach (BaseAddresses addresses in this.profile.baseAddress) {
                if (addresses.flag == this.gamever_.baseAddress) {
                    this.baseAddress.Clear();
                    foreach (BaseAddress address in addresses.value) {
                        this.baseAddress.Add(address.dictKey, address);
                    }
                    break;
                }
            }
            foreach (Memories memories in this.profile.memories) {
                if (memories.flag == this.gamever_.memory) {
                    this.memories.Clear();
                    foreach (Memory memory in memories.value) {
                        this.memories.Add(memory.dictKey, memory);
                    }
                    break;
                }
            }
            this.formIntPtr = intPtr;
            this.plugins.Clear();
            this.pluginMemory.Clear();
            foreach (PluginItem item2 in this.profile.plugins) {
                if (item2.hotkey != "") {
                    if (this.registerHotKey(item2.hotkey, this.shortcut.Count<string>() + 0x3e8) == 0) {
                        item2.hotkey = "!!Failed!!";
                    } else {
                        this.shortcut.Add(item2.dictKey);
                    }
                }
                this.pluginMemory.Add(item2.dictKey, item2.memories);
                this.plugins.Add(item2.dictKey, item2);
                if ((item2.verifyKey == "") || (item2.verifyKey == null)) {
                    item2.verifyKey = item2.dictKey;
                }
                item2.verifyDict = new Dictionary<uint, string>();
                if (item2.verifys != null) {
                    foreach (Verify verify in item2.verifys) {
                        if (!item2.verifyDict.ContainsKey(sting2uint(verify.value))) {
                            item2.verifyDict.Add(sting2uint(verify.value), verify.name);
                        }
                    }
                }
            }
            this.lockedMemory.Clear();
            new Thread(new ThreadStart(this.lockedMem)).Start();
            this.isInit = true;
        }

        public bool read(string plugin, out Dictionary<string, uint> value) {
            if (!this.isInit) {
                throw new Exception("外挂配置尚未加载成功");
            }
            value = new Dictionary<string, uint>();
            try {
                foreach (PluginMemory memory in this.plugins[plugin].memories) {
                    string baseAddress = this.memories[memory.memory].baseAddress;
                    string module = this.baseAddress[baseAddress].module;
                    uint num = sting2uint(this.baseAddress[baseAddress].address);
                    if (module != "") {
                        num += this.gameMemory.GetModuleAddress(module);
                    }
                    this.memories[memory.memory].offsets.Insert(0, num.ToString());
                    uint[] offset = this.memories[memory.memory].offsets.ToUIntArray();
                    this.memories[memory.memory].offsets.RemoveAt(0);
                    uint num2 = 0;
                    num2 = this.gameMemory.ReadMemory(offset, memory.size);
                    value.Add(memory.memory, num2);
                    if (this.plugins[plugin].verifyKey == memory.memory) {
                        if (this.plugins[plugin].verifyDict.ContainsKey(num2)) {
                            this.plugins[plugin].verify = this.plugins[plugin].verifyDict[num2];
                        } else {
                            this.plugins[plugin].verify = num2.ToString();
                        }
                    }
                }
                return true;
            } catch (Exception) {
                return false;
            }
        }

        private string readProfile(string profile) {
            string str = "";
            StreamReader reader = new StreamReader(profile);
            while (!reader.EndOfStream) {
                str = str + reader.ReadLine();
            }
            reader.Close();
            return str;
        }

        public bool realize(string plugin, out Dictionary<string, uint> value) {
            if (!this.isInit) {
                throw new Exception("外挂配置尚未加载成功");
            }
            value = new Dictionary<string, uint>();
            try {
                foreach (PluginMemory memory in this.plugins[plugin].memories) {
                    try {
                        string baseAddress = this.memories[memory.memory].baseAddress;
                        string module = this.baseAddress[baseAddress].module;
                        uint num = sting2uint(this.baseAddress[baseAddress].address);
                        if (module != "") {
                            num += this.gameMemory.GetModuleAddress(module);
                        }
                        this.memories[memory.memory].offsets.Insert(0, num.ToString());
                        uint[] offset = this.memories[memory.memory].offsets.ToUIntArray();
                        this.memories[memory.memory].offsets.RemoveAt(0);
                        uint num2 = this.gameMemory.ReadMemory(offset, memory.size);
                        uint num3 = 0;
                        string action = memory.action;
                        if (!(action == "change")) {
                            if (action == "switch") {
                                goto Label_01D1;
                            }
                            if (action == "lock") {
                                goto Label_025D;
                            }
                            if (action == "add") {
                                goto Label_02BD;
                            }
                            if (action == "dec") {
                                goto Label_0310;
                            }
                            if (action == "copy") {
                                goto Label_0363;
                            }
                            goto Label_047E;
                        }
                        if (memory.value.Count >= 1) {
                            this.gameMemory.WriteMemory(offset, sting2uint(memory.value[0]), memory.size);
                            num3 = this.gameMemory.ReadMemory(offset, memory.size);
                        }
                        goto Label_049B;
                    Label_01D1:
                        if (memory.value.Count >= 2) {
                            if (num2 == sting2uint(memory.value[0])) {
                                this.gameMemory.WriteMemory(offset, sting2uint(memory.value[1]), memory.size);
                            } else {
                                this.gameMemory.WriteMemory(offset, sting2uint(memory.value[0]), memory.size);
                            }
                            num3 = this.gameMemory.ReadMemory(offset, memory.size);
                        }
                        goto Label_049B;
                    Label_025D:
                        if (this.lockedMemory.ContainsKey(memory.memory)) {
                            this.lockedMemory.Remove(memory.memory);
                        } else {
                            if (memory.free == "") {
                                memory.free = num2.ToString();
                            }
                            this.lockedMemory.Add(memory.memory, memory);
                        }
                        goto Label_049B;
                    Label_02BD:
                        if (memory.value.Count >= 1) {
                            this.gameMemory.WriteMemory(offset, num2 + sting2uint(memory.value[0]), memory.size);
                            num3 = this.gameMemory.ReadMemory(offset, memory.size);
                        }
                        goto Label_049B;
                    Label_0310:
                        if (memory.value.Count >= 1) {
                            this.gameMemory.WriteMemory(offset, num2 - sting2uint(memory.value[0]), memory.size);
                            num3 = this.gameMemory.ReadMemory(offset, memory.size);
                        }
                        goto Label_049B;
                    Label_0363:
                        if (memory.copyfor != "") {
                            string str4 = this.memories[memory.copyfor].baseAddress;
                            string text1 = this.baseAddress[baseAddress].module;
                            sting2uint(this.baseAddress[baseAddress].address);
                            if (module != "") {
                                str4 = str4 + this.gameMemory.GetModuleAddress(module);
                            }
                            this.memories[memory.copyfor].offsets.Insert(0, num.ToString());
                            uint[] numArray2 = this.memories[memory.copyfor].offsets.ToUIntArray();
                            this.memories[memory.copyfor].offsets.RemoveAt(0);
                            uint num4 = this.gameMemory.ReadMemory(numArray2, memory.size);
                            this.gameMemory.WriteMemory(offset, num4, memory.size);
                            num3 = this.gameMemory.ReadMemory(offset, memory.size);
                        }
                        goto Label_049B;
                    Label_047E:
                        num3 = this.gameMemory.ReadMemory(offset, memory.size);
                        return false;
                    Label_049B:
                        value.Add(memory.memory, num3);
                        if (this.plugins[plugin].verifyKey == memory.memory) {
                            if (this.plugins[plugin].verifyDict.ContainsKey(num3)) {
                                this.plugins[plugin].verify = this.plugins[plugin].verifyDict[num3];
                            } else {
                                this.plugins[plugin].verify = num3.ToString();
                            }
                        }
                    } catch {
                    }
                }
                return true;
            } catch (Exception) {
                return false;
            }
        }

        public bool realize(int hotkey, out Dictionary<string, uint> value, out string plugin) {
            plugin = this.shortcut[hotkey - 0x3e8];
            return this.realize(plugin, out value);
        }

        private int registerHotKey(string hotKey, int hotKeyid) {
            char[] separator = new char[] { '+' };
            Keys none = Keys.None;
            SysApi.KeyModifiers fsModifiers = SysApi.KeyModifiers.None;
            foreach (string str in hotKey.Replace(" ", "").Split(separator)) {
                try {
                    fsModifiers |= (SysApi.KeyModifiers)Enum.Parse(typeof(SysApi.KeyModifiers), str);
                } catch {
                    try {
                        none = (Keys)Enum.Parse(typeof(Keys), str);
                        break;
                    } catch {
                        return 0;
                    }
                }
            }
            fsModifiers |= SysApi.KeyModifiers.Norepeat;
            if (SysApi.RegisterHotKey(this.formIntPtr, hotKeyid, fsModifiers, (int)none)) {
                return hotKeyid;
            }
            return 0;
        }

        public void reload() {
            this.lockedMemory.Clear();
            this.memories.Clear();
            this.baseAddress.Clear();
            this.pluginMemory.Clear();
            this.plugins.Clear();
            for (int i = 0; i < this.shortcut.Count<string>(); i++) {
                SysApi.UnregisterHotKey(this.formIntPtr, i + 0x3e8);
            }
            this.PluginInit(this.profilePath, this.formIntPtr);
        }

        internal static uint sting2uint(string uintString) {
            uint result = 0;
            if ((uintString.Length >= 3) && (uintString.Substring(0, 2).ToLower() == "0x")) {
                try {
                    result = uint.Parse(uintString.Substring(2), NumberStyles.HexNumber);
                } catch {
                }
                return result;
            }
            uint.TryParse(uintString, out result);
            return result;
        }
    }
}

