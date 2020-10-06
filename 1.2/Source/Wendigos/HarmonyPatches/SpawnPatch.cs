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

namespace Wendigos
{
    [HarmonyPatch(typeof(Thing), "SpawnSetup")]
    public class Patch_SpawnSetup
    {
        private static void Postfix(Thing __instance)
        {
            if (__instance is Pawn pawn && pawn.IsWendigo() && pawn.health?.hediffSet?.GetFirstHediffOfDef(WendigosDefOf.RCW_WendigoismActive) == null)
            {
                var hediff = HediffMaker.MakeHediff(WendigosDefOf.RCW_WendigoismActive, pawn);
                pawn.health.AddHediff(hediff);
            }
        }
    }
}
