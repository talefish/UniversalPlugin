namespace Doffish.MemoryPlugin {
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    internal static class TestExtension {
        public static uint[] ToUIntArray(this List<string> strary) {
            List<uint> list = new List<uint>();
            foreach (string str in strary) {
                list.Add(Plugin.sting2uint(str));
            }
            return list.ToArray();
        }
    }
}

