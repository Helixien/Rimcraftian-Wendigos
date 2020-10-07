using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.BaseGen;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace Wendigos
{
	public class GenStep_KilledCaravanWithWendigos : GenStep
	{
		public override int SeedPart
		{
			get
			{
				return 398638183;
			}
		}

		public override void Generate(Map map, GenStepParams parms)
		{
			CellRect cellRect = CellRect.CenteredOn(map.Center, 20, 7).ClipInsideMap(map);
			BaseGen.globalSettings.map = map;
			IntVec3 playerStartSpot;
			CellFinder.TryFindRandomEdgeCellWith((IntVec3 v) => GenGrid.Standable(v, map), map, 0f, out playerStartSpot);
			MapGenerator.PlayerStartSpot = playerStartSpot;
			var baseResolveParams = default(ResolveParams);
			baseResolveParams.rect = cellRect;
			BaseGen.Generate();
			Log.Message("parms.sitePart.parms.threatPoints: " + parms.sitePart.parms.threatPoints, true);
			this.MakeTradeCaravan(map, parms.sitePart.parms.threatPoints, cellRect);

		}

		private void MakeTradeCaravan(Map map, float points, CellRect cellRect)
		{
			IncidentParms incidentParms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.FactionArrival, map);
			incidentParms.forced = true;
			incidentParms.points = points;
			incidentParms.spawnCenter = map.Center;
			incidentParms.faction = map.ParentFaction;
			incidentParms.traderKind = DefDatabase<TraderKindDef>.AllDefsListForReading.Where((TraderKindDef t) => t.faction == map.ParentFaction.def).RandomElement();
			Log.Message("incidentParms.traderKind: " + incidentParms.traderKind, true);
			PawnGroupMakerParms defaultPawnGroupMakerParms = IncidentParmsUtility.GetDefaultPawnGroupMakerParms(PawnGroupKindDefOf.Trader, incidentParms, true);
			List<Pawn> list = PawnGroupMakerUtility.GeneratePawns(defaultPawnGroupMakerParms, false).ToList<Pawn>();
			List<Pawn> wendigos = new List<Pawn>();
			List<IntVec3> occupiedCells = new List<IntVec3>();
			Predicate<IntVec3> predicate = delegate (IntVec3 x)
			{
				if (occupiedCells.Contains(x))
				{
					return false;
				}
				else if (!x.Walkable(map))
                {
					return false;
                }
				return true;
			};

			var faction = Find.FactionManager.FirstFactionOfDef(WendigosDefOf.RCW_FeralWendigos);
			for (int i = 0; i < list.Where(x => x.RaceProps.Humanlike).Count() / 3; i++)
			{
				var wendigo = PawnGenerator.GeneratePawn(RCW_PawnKindDefOf.RCW_WendigoFledgling, faction);
				wendigos.Add(wendigo);
			}

			if (WendigosUtils.IsNightNow(map))
            {
				var ancientWendingo = PawnGenerator.GeneratePawn(WendigosUtils.AncientWendingoPawnKindDefs.RandomElement(), faction);
				wendigos.Add(ancientWendingo);
			}

			LordMaker.MakeNewLord(faction, new LordJob_Wendingo_AssaultColony(faction), map, wendigos);
			foreach (Pawn pawn in list)
			{
				if (CellFinder.TryFindRandomCellInsideWith(cellRect, predicate, out IntVec3 result)) 
				{
					GenSpawn.Spawn(pawn, result, map, 0);
					Log.Message(pawn + " - " + pawn.Faction.def + " - " + pawn.kindDef);
					occupiedCells.Add(result);
					pawn.Kill(new DamageInfo(DamageDefOf.Bite, 25f));
					if (Rand.Chance(0.7f))
                    {
						for (int j = 0; j < Rand.RangeInclusive(1, 4); j++)
                        {
							GenSpawn.Spawn(ThingDefOf.Filth_Blood, result, map);
						}
					}
					if (pawn.RaceProps.Humanlike && wendigos.Any())
                    {
						var wendigo = wendigos.RandomElement();
						wendigos.Remove(wendigo);
						GenSpawn.Spawn(wendigo, pawn.Position, map, 0);
					}
				}
			}

			foreach (Pawn wendigo in wendigos)
			{
				if (CellFinder.TryFindRandomCellInsideWith(cellRect, predicate, out IntVec3 result))
				{
					GenSpawn.Spawn(wendigo, result, map, 0);
					occupiedCells.Add(result);
				}
			}
		}
	}
}
