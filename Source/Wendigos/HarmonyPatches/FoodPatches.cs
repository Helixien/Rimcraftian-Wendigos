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
    [HarmonyPatch(typeof(Thing), "Ingested")]
    public class Patch_ThoughtsFromIngesting
    {
        private static void Prefix(Thing __instance, Pawn ingester, float nutritionWanted)
        {
            if (ingester.RaceProps.Humanlike)
            {
                if (FoodUtility.IsHumanlikeMeat(__instance.def))
                {
                    if (!ingester.IsWendigo())
                    {
                        float severity = 0.1f;
                        if (ingester.story.traits.HasTrait(TraitDefOf.Cannibal))
                        {
                            Log.Message("Found cannibal: initial severity: " + severity, true);
                            severity = severity - (severity * 30f / 100f); // Cannibals should gain severity 30% slower
                            Log.Message("Found cannibal: severity after: " + severity, true);
                        }
                        HealthUtility.AdjustSeverity(ingester, WendigosDefOf.RCW_Wendigoism, severity);
                    }
                    else
                    {
                        var need = ingester.needs.TryGetNeed<Need_HumanMeat>();
                        need.CurLevel += __instance.GetStatValue(StatDefOf.Nutrition);
                    }
                }
                else
                {
                    CompIngredients compIngredients = __instance.TryGetComp<CompIngredients>();
                    if (compIngredients != null)
                    {
                        var humanIngredients = compIngredients.ingredients.Where(x => FoodUtility.IsHumanlikeMeat(x)).Count();
                        if (!ingester.IsWendigo())
                        {
                            float severity = 0.1f;
                            if (ingester.story.traits.HasTrait(TraitDefOf.Cannibal))
                            {
                                Log.Message("Found cannibal: initial severity: " + severity, true);
                                severity = severity - (severity * 30f / 100f); // Cannibals should gain severity 30% slower
                                Log.Message("Found cannibal: severity after: " + severity, true);
                            }
                            HealthUtility.AdjustSeverity(ingester, WendigosDefOf.RCW_Wendigoism, severity);
                        }
                        else
                        {
                            var need = ingester.needs.TryGetNeed<Need_HumanMeat>();
                            need.CurLevel += __instance.GetStatValue(StatDefOf.Nutrition) * ((float)compIngredients.ingredients.Count * (float)humanIngredients / 100f);
                        }
                    }
                }

                if (ingester.IsWendigo())
                {
                    if (!__instance.def.IsMeat)
                    {
                        Log.Message(ingester + " - " + __instance + " - " + __instance.def + " - " + __instance.def.IsMeat , true);
                        HealthUtility.AdjustSeverity(ingester, HediffDefOf.FoodPoisoning, 0.1f);
                        ingester.jobs.StartJob(JobMaker.MakeJob(JobDefOf.Vomit), JobCondition.InterruptForced);
                    }
                }
            }
        }
    }
}
