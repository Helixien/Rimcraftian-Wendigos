﻿using RimWorld;
using Verse;
using Verse.AI;

namespace Wendigos
{
	public class JobGiver_HuntAnyWildAnimal : ThinkNode_JobGiver
	{
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.TryGetAttackVerb(null) == null)
			{
				return null;
			}
			var mentalState = pawn.MentalState as MentalState_FrenzyHunt;
			if (mentalState == null)
            {
				Log.Message(pawn + " - return null 0", true);
				return null;
            }

			Pawn prey = null;
			if (mentalState.prey != null)
            {
				if (!mentalState.prey.Dead)
                {
					prey = mentalState.prey;
				}
				else
                {
					Log.Message(pawn + " - ingest " + mentalState.prey, true);
					return JobMaker.MakeJob(JobDefOf.Ingest, mentalState.prey);
				}
			}
			else
            {
				prey = FindPawnTarget(pawn);
			}
			if (prey != null)
            {
				mentalState.prey = prey;
				if (pawn.CanReach(prey, PathEndMode.Touch, Danger.Deadly))
				{
					Log.Message(pawn + " - melee attack 2 - " + prey, true);
					return MeleeAttackJob(pawn, prey);
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

		private Job MeleeAttackJob(Pawn pawn, Thing target)
		{
			Job job = JobMaker.MakeJob(JobDefOf.AttackMelee, target);
			job.maxNumMeleeAttacks = 1;
			job.expiryInterval = Rand.Range(420, 900);
			job.attackDoorIfTargetLost = true;
			return job;
		}

		private Pawn FindPawnTarget(Pawn pawn)
		{
			return (Pawn)AttackTargetFinder.BestAttackTarget(pawn, TargetScanFlags.NeedThreat | TargetScanFlags.NeedAutoTargetable, (Thing x) => x is Pawn victim
			&& victim.RaceProps.Animal && victim.Faction == null, 0f, 9999f, default(IntVec3), float.MaxValue, canBash: true);
		}
	}
}