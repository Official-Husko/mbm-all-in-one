using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MBMScripts;

namespace mbm_all_in_one.src.modules.mods.MBMModsServices
{
    public class UnitTypeUtility
    {
        // Token: 0x0400004F RID: 79
        public static IList<EUnitType> FemaleUnitTypes = new List<EUnitType>
        {
            EUnitType.Human,
            EUnitType.Dwarf,
            EUnitType.Elf,
            EUnitType.Inu,
            EUnitType.Neko,
            EUnitType.Usagi,
            EUnitType.Hitsuji,
            EUnitType.Dragonian
        };

        // Token: 0x04000050 RID: 80
        public static IList<EUnitType> MaleUnitTypes = new List<EUnitType>
        {
            EUnitType.Goblin,
            EUnitType.Orc,
            EUnitType.Werewolf,
            EUnitType.Minotaur,
            EUnitType.Salamander
        };

        // Token: 0x04000051 RID: 81
        public static IList<EUnitType> NormalUnitTypes = (from EUnitType unitType in Enum.GetValues(typeof(EUnitType))
                                                          where FemaleUnitTypes.Contains(unitType) || MaleUnitTypes.Contains(unitType)
                                                          select unitType).ToList();

        // Token: 0x04000052 RID: 82
        public static IList<EUnitType> FemaleSpecialUnitTypes = new List<EUnitType>
        {
            EUnitType.Aure,
            EUnitType.Bella,
            EUnitType.Karen,
            EUnitType.Sylvia,
            EUnitType.Vivi,
            EUnitType.Claire
        };

        // Token: 0x04000053 RID: 83
        public static IList<EUnitType> MaleSpecialUnitTypes = new List<EUnitType>
        {
            EUnitType.Client,
            EUnitType.Player
        };

        // Token: 0x04000054 RID: 84
        public static IList<EUnitType> AdvisorUnitTypes = new List<EUnitType>
        {
            EUnitType.Amilia,
            EUnitType.Flora,
            EUnitType.Niel,
            EUnitType.SenaLena
        };
    }
}
