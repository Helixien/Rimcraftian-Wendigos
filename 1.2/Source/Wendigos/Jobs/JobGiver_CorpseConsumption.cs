using RimWorld;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;
using static Verse.AI.ReservationManager;

namespace Wendigos
{
	public class JobGiver_CorpseConsumption : ThinkNode_JobGiver
	{
		protected override Job TryGiveJob(Pawn pawn)
		{
			var mentalState = pawn.MentalState as MentalState_CorpseConsumption;
			if (mentalState == null)
            {
				Log.Message(pawn + " - return null 0", true);
				return null;
            }
			if (mentalState.corpse != null)
            {
				Job job = JobMaker.MakeJob(JobDefOf.Ingest, mentalState.corpse);
				job.count = 1;
				Log.Message(pawn + " - ingest 3 " + mentalState.corpse, true);
				return job;
			}
			Thing corpse = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.Corpse), PathEndMode.ClosestTouch, 
				TraverseParms.For(pawn), 50f);
			if (corpse != null && ReservationUtility.CanReserve(pawn, corpse))
			{
				mentalState.corpse = (Corpse)corpse;
				Log.Message(pawn + " - ingest 3 " + corpse, true);
				Job job = JobMaker.MakeJob(JobDefOf.Ingest, mentalState.corpse);
				job.count = 1;
				return job;
			}

			Pawn prey = null;
			if (mentalState.prey != null)
			{
				prey = mentalState.prey;
			}
			else
			{
				prey = WendigosUtils.FindPawnTarget(pawn);
				mentalState.prey = prey;
				Log.Message(pawn + " is finding new prey: " + prey);
			}
			if (prey != null)
			{
				if (prey.Dead)
				{
					if (prey.Corpse != null)
					{
						Job job = JobMaker.MakeJob(JobDefOf.Ingest, prey.Corpse);
						job.count = 1;
						Log.Message(pawn + " - ingest " + prey.Corpse, true);
						return job;
					}

					mentalState.prey = null;
					Log.Message(pawn + " - return null 2 " + prey.Corpse, true);
					return null;
				}
				else if (pawn.CanReach(prey, PathEndMode.Touch, Danger.Deadly))
				{
					Log.Message(pawn + " - melee attack 2 - " + prey, true);
					return WendigosUtils.MeleeAttackJob(prey);
				}
				using (PawnPath pawnPath = pawn.Map.pathFinder.FindPath(pawn.Position, prey.Position, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.PassDoors)))
				{
					if (!pawnPath.Found)
					{
						return null;
					}
					if (!pawnPath.TryFindLastCellBeforeBlockingDoor(pawn, out IntVec3 result))
					{
						Log.Error(string.Concat(pawn, " did TryFindLastCellBeforeDoor but found none when it should have been one. Target: ", prey.LabelCap));
						return null;
					}
					IntVec3 randomCell = CellFinder.RandomRegionNear(result.GetRegion(pawn.Map), 9, TraverseParms.For(pawn)).RandomCell;
					if (randomCell == pawn.Position)
					{
						Log.Message(pawn + " - wait - " + prey, true);

						return JobMaker.MakeJob(JobDefOf.Wait, 30);
					}
					Log.Message(pawn + " - goto - " + prey, true);
					return JobMaker.MakeJob(JobDefOf.Goto, randomCell);
				}
			}
			Log.Message(pawn + " - return null 3", true);
			return null;
		}
	}
}
