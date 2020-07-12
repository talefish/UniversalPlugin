namespace Doffish.MemoryPlugin {
    using System.Collections.Generic;

    public class Verify {
        public string name { get; set; }

        public string value { get; set; }
    }

    internal class BaseAddress {
        public string address { get; set; }

        public string dictKey { get; set; }

        public string module { get; set; }
    }

    internal class BaseAddresses {
        public string flag { get; set; }

        public List<BaseAddress> value { get; set; }
    }

    internal class GameverItem {
        public string baseAddress { get; set; }

        public string fileMd5 { get; set; }

        public string memory { get; set; }

        public string module { get; set; }

        public string version { get; set; }
    }

    internal class Memories {
        public string flag { get; set; }

        public List<Memory> value { get; set; }
    }

    internal class Memory {
        public string baseAddress { get; set; }

        public string dictKey { get; set; }

        public List<string> offsets { get; set; }
    }

    public class PluginItem {
        public string description { get; set; }

        public string dictKey { get; set; }

        public string hotkey { get; set; }

        public List<PluginMemory> memories { get; set; }

        public string name { get; set; }

        public string verify { get; set; }

        internal Dictionary<uint, string> verifyDict { get; set; }

        internal string verifyKey { get; set; }

        public Verify[] verifys { get; set; }
    }

    public class PluginMemory {
        public string action { get; set; }

        public string copyfor { get; set; }

        public string free { get; set; }

        public string memory { get; set; }

        public int size { get; set; }

        public List<string> value { get; set; }
    }

    internal class Profile {
        public string actionColor { get; set; }

        public List<BaseAddresses> baseAddress { get; set; }

        public string defaultColor { get; set; }

        public List<GameverItem> gamever { get; set; }

        public List<Memories> memories { get; set; }

        public List<PluginItem> plugins { get; set; }

        public string process { get; set; }

        public string title { get; set; }

        public string version { get; set; }
    }
}

