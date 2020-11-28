using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using RimWorld.QuestGen;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using Verse.Grammar;

namespace Wendigos
{
    [HarmonyPatch(typeof(Thing), "SpawnSetup")]
    public class Patch_SpawnSetup
    {
        private static void Postfix(Thing __instance)
        {
            if (__instance is Pawn pawn && pawn.IsWendigo() && pawn.health?.hediffSet != null && !pawn.health.hediffSet.HasHediff(WendigosDefOf.RCW_WendigoismActive))
            {
                Log.Message(pawn + " gets RCW_WendigoismActive as a wendigo");
                var hediff = HediffMaker.MakeHediff(WendigosDefOf.RCW_WendigoismActive, pawn);
                pawn.health.AddHediff(hediff);
            }
        }
    }

    //[HarmonyPatch(typeof(QuestGen_Sites), "GenerateSite")]
    //public class Patch_GenerateSite
    //{
    //    private static void Postfix(IEnumerable<SitePartDefWithParams> sitePartsParams, int tile, Faction faction, bool hiddenSitePartsPossible = false, RulePack singleSitePartRules = null)
    //    {
    //        Log.Message("Generating site: " + tile + " - " + faction, true);
    //    }
    //}
    //
    //[HarmonyPatch(typeof(QuestNode_GenerateSite), "RunInt")]
    //public class Patch_RunInt
    //{
    //    private static void Postfix(QuestNode_GenerateSite __instance)
    //    {
    //        Slate slate = QuestGen.slate;
    //        Log.Message("QuestNode_GenerateSite: " + __instance.tile.GetValue(slate) + " - " + __instance.faction.GetValue(slate));
    //    }
    //}
    //
    //[HarmonyPatch(typeof(QuestNode_GetFactionOf), "RunInt")]
    //public class Patch_RunInt2
    //{
    //    private static void Postfix(QuestNode_GetFactionOf __instance)
    //    {
    //        Slate slate = QuestGen.slate;
    //        Log.Message("QuestNode_GetFactionOf: " + __instance.thing.GetValue(slate) + " - " + __instance.thing.GetValue(slate).Faction);
    //    }
    //}
    //
    //[HarmonyPatch(typeof(QuestNode_GetFaction), "RunInt")]
    //public class Patch_RunInt3
    //{
    //    private static void Postfix(QuestNode_GetFaction __instance)
    //    {
    //        Slate slate = QuestGen.slate;
    //        Log.Message("QuestNode_GetFaction: " + __instance.storeAs.GetValue(slate) + " - " + slate.Get<Faction>("askerFaction"));
    //    }
    //}
}
