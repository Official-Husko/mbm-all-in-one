using System;
using System.Runtime.CompilerServices;

namespace mbm_all_in_one.src.modules.mods.MBMModsServices
{
    public class PeriodicAction
    {
        // Token: 0x06000038 RID: 56 RVA: 0x00002EAE File Offset: 0x000010AE
        public PeriodicAction(Action act)
        {
            id = Guid.NewGuid();
            perform = act;
            enabled = true;
        }

        // Token: 0x04000045 RID: 69
        public Guid id;

        // Token: 0x04000046 RID: 70
        public Action perform;

        // Token: 0x04000047 RID: 71
        public bool enabled;
    }
}
