using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace mbm_all_in_one.src.modules.mods.MBMModsServices
{
    public class PeriodicActionGroup
    {
        // Token: 0x06000039 RID: 57 RVA: 0x00002ECF File Offset: 0x000010CF
        public PeriodicActionGroup(float period, PeriodicAction act)
        {
            timeSinceRun = 0f;
            this.period = period;
            actions.Add(act);
        }

        // Token: 0x0600003A RID: 58 RVA: 0x00002F00 File Offset: 0x00001100
        public bool RemoveAction(PeriodicAction act)
        {
            return actions.Remove(act);
        }

        // Token: 0x0600003B RID: 59 RVA: 0x00002F10 File Offset: 0x00001110
        public void Act()
        {
            foreach (PeriodicAction periodicAction in actions)
            {
                if (periodicAction.enabled)
                {
                    periodicAction.perform();
                }
            }
        }

        // Token: 0x04000048 RID: 72
        public Guid id;

        // Token: 0x04000049 RID: 73
        public float timeSinceRun;

        // Token: 0x0400004A RID: 74
        public float period;

        // Token: 0x0400004B RID: 75
        public IList<PeriodicAction> actions = new List<PeriodicAction>();
    }
}
