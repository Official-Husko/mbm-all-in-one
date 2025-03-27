using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MBMModsServices;
using MBMScripts;

namespace mbm_all_in_one.src.modules.mods.MBMModsServices
{
    public class TraitUtility
    {
        // Token: 0x0600003C RID: 60 RVA: 0x00002F6C File Offset: 0x0000116C
        public static float GetTraitIncrement(ETraitReadable trait)
        {
            switch (trait)
            {
                case ETraitReadable.BreedingTime:
                    return 5f;
                case ETraitReadable.ConceptionRate:
                    return 0.05f;
                case ETraitReadable.GrowthTime:
                    return 5f;
                case ETraitReadable.MaintenanceCost:
                    return 5f;
                case ETraitReadable.MaxBirthCount:
                    return 3f;
                case ETraitReadable.MaxHealth:
                    return 15f;
                case ETraitReadable.MultiplePregnancy:
                    return 1f;
                case ETraitReadable.SlavesHealthConsumptionDuringBreeding:
                    return 0.05f;
                default:
                    return 1f;
            }
        }

        // Token: 0x0400004C RID: 76
        public static readonly IList<ETraitReadable> GeneralTraits = new List<ETraitReadable>
        {
            ETraitReadable.BreedingTime,
            ETraitReadable.ConceptionRate,
            ETraitReadable.GrowthTime,
            ETraitReadable.MaintenanceCost,
            ETraitReadable.MaxBirthCount,
            ETraitReadable.MaxHealth,
            ETraitReadable.MultiplePregnancy,
            ETraitReadable.SlavesHealthConsumptionDuringBreeding
        };

        // Token: 0x0400004D RID: 77
        public static readonly IList<ETraitReadable> AllRaceTraits = (from ETraitReadable trait in Enum.GetValues(typeof(ETraitReadable))
                                                                      where !GeneralTraits.Contains(trait) && trait != ETraitReadable.UnknowTrait
                                                                      select trait).ToList();

        // Token: 0x0400004E RID: 78
        public static readonly IList<ETrait> AllTraits = (from ETrait trait in Enum.GetValues(typeof(ETrait))
                                                          where trait > ETrait.None
                                                          select trait).ToList();
    }
}
