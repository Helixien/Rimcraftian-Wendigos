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
    public class Patch_Ingested
    {
        private static void Prefix(Thing __instance, Pawn ingester, float nutritionWanted)
        {
            Log.Message(" - Prefix - if (ingester.RaceProps.Humanlike) - 1", true);
            if (ingester.RaceProps.Humanlike)
            {
                Log.Message(" - Prefix - if (FoodUtility.IsHumanlikeMeat(__instance.def)) - 2", true);
                if (FoodUtility.IsHumanlikeMeat(__instance.def))
                {
                    Log.Message(" - Prefix - if (!ingester.IsWendigo()) - 3", true);
                    if (!ingester.IsWendigo())
                    {
                        Log.Message(" - Prefix - float severity = 0.01f; - 4", true);
                        float severity = 0.01f;
                        Log.Message(" - Prefix - if (ingester.story.traits.HasTrait(TraitDefOf.Cannibal)) - 5", true);
                        if (ingester.story.traits.HasTrait(TraitDefOf.Cannibal))
                        {
                            Log.Message("Found cannibal: initial severity: " + severity, true);
                            severity = severity - (severity * 30f / 100f); // Cannibals should gain severity 30% slower
                            Log.Message("Found cannibal: severity after: " + severity, true);
                        }
                        severity *= __instance.stackCount;
                        Log.Message(" - Prefix - HealthUtility.AdjustSeverity(ingester, WendigosDefOf.RCW_Wendigoism, severity); - 9", true);
                        HealthUtility.AdjustSeverity(ingester, WendigosDefOf.RCW_Wendigoism, severity);
                    }
                    else
                    {
                        Log.Message(" - Prefix - var need = ingester.needs.TryGetNeed<Need_HumanMeat>(); - 10", true);
                        var need = ingester.needs.TryGetNeed<Need_HumanMeat>();
                        if (need != null)
                        {
                            Log.Message(" - Prefix - need.CurLevel += __instance.GetStatValue(StatDefOf.Nutrition); - 11", true);
                            need.CurLevel += __instance.GetStatValue(StatDefOf.Nutrition);
                        }
                    }
                }
                else
                {
                    Log.Message(" - Prefix - CompIngredients compIngredients = __instance.TryGetComp<CompIngredients>(); - 12", true);
                    CompIngredients compIngredients = __instance.TryGetComp<CompIngredients>();
                    Log.Message(" - Prefix - if (compIngredients != null) - 13", true);
                    if (compIngredients != null)
                    {
                        Log.Message(" - Prefix - var humanIngredients = compIngredients.ingredients.Where(x => FoodUtility.IsHumanlikeMeat(x)).Count(); - 14", true);
                        var humanIngredients = compIngredients.ingredients.Where(x => FoodUtility.IsHumanlikeMeat(x)).Count();
                        Log.Message(" - Prefix - if (!ingester.IsWendigo()) - 15", true);
                        if (!ingester.IsWendigo())
                        {
                            Log.Message(" - Prefix - float severity = 0.01f; - 16", true);
                            float severity = 0.01f;
                            Log.Message(" - Prefix - if (ingester.story.traits.HasTrait(TraitDefOf.Cannibal)) - 17", true);
                            if (ingester.story.traits.HasTrait(TraitDefOf.Cannibal))
                            {
                                Log.Message("Found cannibal: initial severity: " + severity, true);
                                severity = severity - (severity * 30f / 100f); // Cannibals should gain severity 30% slower
                                Log.Message("Found cannibal: severity after: " + severity, true);
                            }
                            severity *= humanIngredients;
                            Log.Message(" - Prefix - HealthUtility.AdjustSeverity(ingester, WendigosDefOf.RCW_Wendigoism, severity); - 21", true);
                            HealthUtility.AdjustSeverity(ingester, WendigosDefOf.RCW_Wendigoism, severity);
                        }
                        else if (compIngredients.ingredients != null)
                        {
                            Log.Message(" - Prefix - var need = ingester.needs.TryGetNeed<Need_HumanMeat>(); - 22", true);
                            var need = ingester.needs.TryGetNeed<Need_HumanMeat>();
                            if (need != null)
                            {
                                Log.Message(" - Prefix - need.CurLevel += __instance.GetStatValue(StatDefOf.Nutrition) * ((float)compIngredients.ingredients.Count * (float)humanIngredients / 100f); - 23", true);
                                need.CurLevel += __instance.GetStatValue(StatDefOf.Nutrition) * ((float)compIngredients.ingredients.Count * (float)humanIngredients / 100f);
                            }
                        }
                    }
                }

                Log.Message(" - Prefix - if (ingester.IsWendigo()) - 24", true);
                if (ingester.IsWendigo())
                {
                    Log.Message(" - Prefix - if (!__instance.def.IsMeat) - 25", true);
                    if (!__instance.def.IsMeat)
                    {
                        Log.Message(ingester + " - " + __instance + " - " + __instance.def + " - " + __instance.def.IsMeat, true);
                        Log.Message(" - Prefix - HealthUtility.AdjustSeverity(ingester, HediffDefOf.FoodPoisoning, 0.1f); - 27", true);
                        HealthUtility.AdjustSeverity(ingester, HediffDefOf.FoodPoisoning, 0.1f);
                        Log.Message(" - Prefix - ingester.jobs.StartJob(JobMaker.MakeJob(JobDefOf.Vomit), JobCondition.InterruptForced); - 28", true);
                        ingester.jobs.StartJob(JobMaker.MakeJob(JobDefOf.Vomit), JobCondition.InterruptForced);
                    }
                }
            }
        }
    }
}
