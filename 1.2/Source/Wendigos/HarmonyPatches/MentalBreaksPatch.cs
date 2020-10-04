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
    [HarmonyPatch(typeof(MentalStateHandler), "TryStartMentalState")]
    public class Patch_TryStartMentalState
    {
        private static bool Prefix(Pawn ___pawn, ref bool __result, MentalStateDef stateDef, string reason = null, bool forceWake = false,
            bool causedByMood = false, Pawn otherPawn = null, bool transitionSilently = false)
        {
            if (___pawn.IsWendigo())
            {
                __result = false;
                return false;
            }
            return true;
        }
    }
}
