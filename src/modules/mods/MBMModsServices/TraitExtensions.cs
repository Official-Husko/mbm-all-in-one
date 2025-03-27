using System;
using MBMModsServices;
using MBMScripts;

namespace mbm_all_in_one.src.modules.mods.MBMModsServices
{
    // Token: 0x0200000C RID: 12
    public static class TraitExtensions
    {
        // Token: 0x06000023 RID: 35 RVA: 0x00002704 File Offset: 0x00000904
        public static ETrait ToETrait(this ETraitReadable traitReadable)
        {
            switch (traitReadable)
            {
                case ETraitReadable.BreedingTime:
                    return ETrait.Trait99;
                case ETraitReadable.ConceptionRate:
                    return ETrait.Trait93;
                case ETraitReadable.GrowthTime:
                    return ETrait.Trait94;
                case ETraitReadable.MaintenanceCost:
                    return ETrait.Trait95;
                case ETraitReadable.MaxBirthCount:
                    return ETrait.Trait97;
                case ETraitReadable.MaxHealth:
                    return ETrait.Trait96;
                case ETraitReadable.MultiplePregnancy:
                    return ETrait.Trait98;
                case ETraitReadable.SlavesHealthConsumptionDuringBreeding:
                    return ETrait.Trait100;
                case ETraitReadable.AceOfBrothel:
                    return ETrait.Trait71;
                case ETraitReadable.AdditionalPregnancy:
                    return ETrait.Trait59;
                case ETraitReadable.Bulimia:
                    return ETrait.Trait64;
                case ETraitReadable.CharmOfAllRaces:
                    return ETrait.Trait44;
                case ETraitReadable.Coward:
                    return ETrait.Trait66;
                case ETraitReadable.DwarfBastards:
                    return ETrait.Trait51;
                case ETraitReadable.ElfBastards:
                    return ETrait.Trait56;
                case ETraitReadable.ExcessiveWrithing:
                    return ETrait.Trait60;
                case ETraitReadable.GamblersBadLuck:
                    return ETrait.Trait75;
                case ETraitReadable.GeneSelection:
                    return ETrait.Trait70;
                case ETraitReadable.GeneticEngineering:
                    return ETrait.Trait76;
                case ETraitReadable.Gluttony:
                    return ETrait.Trait62;
                case ETraitReadable.Heat:
                    return ETrait.Trait47;
                case ETraitReadable.IronWill:
                    return ETrait.Trait72;
                case ETraitReadable.ManyMilk:
                    return ETrait.Trait61;
                case ETraitReadable.RelaxedThinking:
                    return ETrait.Trait65;
                case ETraitReadable.Robbery:
                    return ETrait.Trait74;
                case ETraitReadable.SteelUterus:
                    return ETrait.Trait73;
                case ETraitReadable.StrongGenes:
                    return ETrait.Trait67;
                case ETraitReadable.Superfetation:
                    return ETrait.Trait63;
                case ETraitReadable.WarriorsSoul:
                    return ETrait.Trait52;
                case ETraitReadable.WeakRace:
                    return ETrait.Trait45;
                case ETraitReadable.Humiliation:
                    return ETrait.Trait77;
                case ETraitReadable.Sadism:
                    return ETrait.Trait80;
                case ETraitReadable.OverExcitement:
                    return ETrait.Trait82;
                case ETraitReadable.SemenTank:
                    return ETrait.Trait83;
                case ETraitReadable.Grooming:
                    return ETrait.Trait85;
                case ETraitReadable.ViolentCopulation:
                    return ETrait.Trait87;
                case ETraitReadable.GiveBirthGodsChild:
                    return ETrait.Trait90;
                case ETraitReadable.Miscarriage:
                    return ETrait.Trait92;
                case ETraitReadable.Prejudice:
                    return ETrait.Trait101;
                case ETraitReadable.Pride:
                    return ETrait.Trait102;
                default:
                    return ETrait.None;
            }
        }

        // Token: 0x06000024 RID: 36 RVA: 0x00002834 File Offset: 0x00000A34
        public static ETraitReadable ToETraitReadable(this ETrait trait)
        {
            switch (trait)
            {
                case ETrait.Trait44:
                    return ETraitReadable.CharmOfAllRaces;
                case ETrait.Trait45:
                    return ETraitReadable.WeakRace;
                case ETrait.Trait47:
                    return ETraitReadable.Heat;
                case ETrait.Trait51:
                    return ETraitReadable.DwarfBastards;
                case ETrait.Trait52:
                    return ETraitReadable.WarriorsSoul;
                case ETrait.Trait56:
                    return ETraitReadable.ElfBastards;
                case ETrait.Trait59:
                    return ETraitReadable.AdditionalPregnancy;
                case ETrait.Trait60:
                    return ETraitReadable.ExcessiveWrithing;
                case ETrait.Trait61:
                    return ETraitReadable.ManyMilk;
                case ETrait.Trait62:
                    return ETraitReadable.Gluttony;
                case ETrait.Trait63:
                    return ETraitReadable.Superfetation;
                case ETrait.Trait64:
                    return ETraitReadable.Bulimia;
                case ETrait.Trait65:
                    return ETraitReadable.RelaxedThinking;
                case ETrait.Trait66:
                    return ETraitReadable.Coward;
                case ETrait.Trait67:
                    return ETraitReadable.StrongGenes;
                case ETrait.Trait70:
                    return ETraitReadable.GeneSelection;
                case ETrait.Trait71:
                    return ETraitReadable.AceOfBrothel;
                case ETrait.Trait72:
                    return ETraitReadable.IronWill;
                case ETrait.Trait73:
                    return ETraitReadable.SteelUterus;
                case ETrait.Trait74:
                    return ETraitReadable.Robbery;
                case ETrait.Trait75:
                    return ETraitReadable.GamblersBadLuck;
                case ETrait.Trait76:
                    return ETraitReadable.GeneticEngineering;
                case ETrait.Trait77:
                    return ETraitReadable.Humiliation;
                case ETrait.Trait80:
                    return ETraitReadable.Sadism;
                case ETrait.Trait82:
                    return ETraitReadable.OverExcitement;
                case ETrait.Trait83:
                    return ETraitReadable.SemenTank;
                case ETrait.Trait85:
                    return ETraitReadable.Grooming;
                case ETrait.Trait87:
                    return ETraitReadable.ViolentCopulation;
                case ETrait.Trait90:
                    return ETraitReadable.GiveBirthGodsChild;
                case ETrait.Trait92:
                    return ETraitReadable.Miscarriage;
                case ETrait.Trait93:
                    return ETraitReadable.ConceptionRate;
                case ETrait.Trait94:
                    return ETraitReadable.GrowthTime;
                case ETrait.Trait95:
                    return ETraitReadable.MaintenanceCost;
                case ETrait.Trait96:
                    return ETraitReadable.MaxHealth;
                case ETrait.Trait97:
                    return ETraitReadable.MaxBirthCount;
                case ETrait.Trait98:
                    return ETraitReadable.MultiplePregnancy;
                case ETrait.Trait99:
                    return ETraitReadable.BreedingTime;
                case ETrait.Trait100:
                    return ETraitReadable.SlavesHealthConsumptionDuringBreeding;
                case ETrait.Trait101:
                    return ETraitReadable.Prejudice;
                case ETrait.Trait102:
                    return ETraitReadable.Pride;
            }
            return ETraitReadable.UnknowTrait;
        }
    }
}
