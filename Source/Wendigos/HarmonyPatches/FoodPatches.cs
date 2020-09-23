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
    [HarmonyPatch(typeof(FoodUtility), "ThoughtsFromIngesting")]
    public class Patch_ThoughtsFromIngesting
    {
        private static void Postfix(Pawn ingester, Thing foodSource, ThingDef foodDef)
        {
            if (ingester.RaceProps.Humanlike)
            {
                if (FoodUtility.IsHumanlikeMeat(foodDef))
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
                        need.CurLevel += foodSource.GetStatValue(StatDefOf.Nutrition);
                    }
                }
                else
                {
                    CompIngredients compIngredients = foodSource.TryGetComp<CompIngredients>();
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
                            need.CurLevel += foodSource.GetStatValue(StatDefOf.Nutrition) / (float)humanIngredients;
                        }
                    }
                }
            }

        }
    }
}
