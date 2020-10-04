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
    [HarmonyPatch(typeof(InspirationHandler), "TryStartInspiration_NewTemp")]
    public class Patch_TryStartInspiration_NewTemp
    {
        private static bool Prefix(ref bool __result, InspirationHandler __instance)
        {
            if (__instance.pawn.IsWendigo())
            {
                __result = false;
                return false;
            }
            return true;
        }
    }
}
