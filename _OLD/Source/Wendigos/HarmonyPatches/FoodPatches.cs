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

    [HarmonyPatch(typeof(FoodUtility), "AddFoodPoisoningHediff")]
    public class Patch_AddFoodPoisoningHediff
    {
        private static bool Prefix(Pawn pawn, Thing ingestible, FoodPoisonCause cause)
        {
            if (FoodUtility.IsHumanlikeMeat(ingestible.def) && pawn.IsWendigo())
            {
                return false;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(FoodUtility), "ThoughtsFromIngesting")]
    public class Patch_ThoughtsFromIngesting
    {
        private static bool Prefix(ref List<ThoughtDef> __result, List<ThoughtDef> ___ingestThoughts, Pawn ingester, Thing foodSource, ThingDef foodDef)
        {
            if (FoodUtility.IsHumanlikeMeat(foodSource.def) && ingester.IsWendigo())
            {
                __result = ThoughtsFromIngesting(ingester, foodSource, foodDef, ___ingestThoughts);
                return false;
            }
            return true;
        }

        public static List<ThoughtDef> ThoughtsFromIngesting(Pawn ingester, Thing foodSource, ThingDef foodDef, List<ThoughtDef> ingestThoughts)
        {
            ingestThoughts.Clear();
            if (ingester.needs == null || ingester.needs.mood == null)
            {
                return ingestThoughts;
            }
            if (!ingester.story.traits.HasTrait(TraitDefOf.Ascetic) && foodDef.ingestible.tasteThought != null)
            {
                ingestThoughts.Add(foodDef.ingestible.tasteThought);
            }
            CompIngredients compIngredients = foodSource.TryGetComp<CompIngredients>();
            Building_NutrientPasteDispenser building_NutrientPasteDispenser = foodSource as Building_NutrientPasteDispenser;
            if (FoodUtility.IsHumanlikeMeat(foodDef) && ingester.RaceProps.Humanlike)
            {
                ingestThoughts.Add(WendigosDefOf.RCW_AteHumanlikeMeatDirectWendigo);
            }
            else if (compIngredients != null)
            {
                for (int i = 0; i < compIngredients.ingredients.Count; i++)
                {
                    AddIngestThoughtsFromIngredient(compIngredients.ingredients[i], ingester, ingestThoughts);
                }
            }
            else if (building_NutrientPasteDispenser != null)
            {
                Thing thing = building_NutrientPasteDispenser.FindFeedInAnyHopper();
                if (thing != null)
                {
                    AddIngestThoughtsFromIngredient(thing.def, ingester, ingestThoughts);
                }
            }
            if (foodDef.ingestible.specialThoughtDirect != null)
            {
                ingestThoughts.Add(foodDef.ingestible.specialThoughtDirect);
            }
            if (foodSource.IsNotFresh())
            {
                ingestThoughts.Add(ThoughtDefOf.AteRottenFood);
            }
            if (ModsConfig.RoyaltyActive && FoodUtility.InappropriateForTitle(foodDef, ingester, allowIfStarving: false))
            {
                ingestThoughts.Add(ThoughtDefOf.AteFoodInappropriateForTitle);
            }
            return ingestThoughts;
        }

        private static void AddIngestThoughtsFromIngredient(ThingDef ingredient, Pawn ingester, List<ThoughtDef> ingestThoughts)
        {
            if (ingredient.ingestible != null)
            {
                if (ingester.RaceProps.Humanlike && FoodUtility.IsHumanlikeMeat(ingredient))
                {
                    ingestThoughts.Add(WendigosDefOf.RCW_AteHumanlikeMeatDirectWendigo);
                }
                else if (ingredient.ingestible.specialThoughtAsIngredient != null)
                {
                    ingestThoughts.Add(ingredient.ingestible.specialThoughtAsIngredient);
                }
            }
        }
    }


}
