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
    [HarmonyPatch(typeof(JobGiver_Manhunter), "FindPawnTarget")]
    public class Patch_FindPawnTarget
    {
        private static void Postfix(Pawn pawn, ref Pawn __result)
        {
            if (__result.IsWendigo())
            {
                __result = (Pawn)AttackTargetFinder.BestAttackTarget(pawn, TargetScanFlags.NeedThreat | TargetScanFlags.NeedAutoTargetable, 
                    (Thing x) => x is Pawn victim && !victim.IsWendigo() && (int)x.def.race.intelligence >= 1, 0f, 9999f, default(IntVec3), float.MaxValue, canBash: true);
            }
        }
    }
}
