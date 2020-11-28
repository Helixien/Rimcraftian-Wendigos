using System.Linq;
using RimWorld;
using Verse;
using Verse.AI;

namespace Wendigos
{
	public class JobGiver_CaptureDownedVictimAndLeaveMap : ThinkNode_JobGiver
	{
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!RCellFinder.TryFindBestExitSpot(pawn, out IntVec3 spot))
			{
				return null;
			}

			Job job = null;
			if (pawn.mindState.meleeThreat != null && pawn.mindState.meleeThreat.RaceProps.Humanlike 
				&& pawn.mindState.meleeThreat.Downed && pawn.mindState.meleeThreat.HostileTo(pawn) && ReservationUtility.CanReserve(pawn, pawn.mindState.meleeThreat))
			{
				job = KidnapOrEat(pawn, pawn.mindState.meleeThreat, spot);
				if (job != null)
				{
					return job;
				}
			}
			else if (pawn.mindState.enemyTarget != null && pawn.mindState.enemyTarget is Pawn pawn2 
				&& pawn2.RaceProps.Humanlike && pawn2.Downed && pawn2.HostileTo(pawn) && ReservationUtility.CanReserve(pawn, pawn2))
			{
				job = KidnapOrEat(pawn, pawn2, spot);
				if (job != null)
				{
					return job;
				}
			}
			var victim = pawn.Map.mapPawns.AllPawns.Where(x => x.RaceProps.Humanlike && x.HostileTo(pawn) && x.Downed
				&& x.Position.DistanceTo(pawn.Position) < 35 && ReservationUtility.CanReserve(pawn, x)).FirstOrDefault();
			job = KidnapOrEat(pawn, victim, spot);
			if (job != null)
			{
				return job;
			}
			var corpseList = pawn.Map.listerThings.ThingsInGroup(ThingRequestGroup.Corpse).Cast<Corpse>();
			var corpse = corpseList.Where(x => x.InnerPawn.RaceProps.Humanlike && x.InnerPawn.HostileTo(pawn) 
				&& x.Position.DistanceTo(pawn.Position) < 65 && ReservationUtility.CanReserve(pawn, x)).FirstOrDefault();
			job = KidnapOrEat(pawn, corpse, spot);
			if (job != null)
            {
				return job;
            }
			return null;
		}

		private Job KidnapOrEat(Pawn pawn, Thing victim, IntVec3 spot)
        {
			if (victim != null)
			{
				if (pawn.needs.food.CurLevel > 0.5f)
                {
					Log.Message(pawn + " - steal victim: " + victim);
					Job job = JobMaker.MakeJob(JobDefOf.Kidnap);
					job.targetA = victim;
					job.targetB = spot;
					job.count = 1;
					return job;
				}
				else
                {
					Log.Message(pawn + " - ingest victim: " + victim);
					Job job = JobMaker.MakeJob(JobDefOf.Ingest);
					job.targetA = victim;
					job.count = 1;
					return job;
				}
			}
			return null;
		}
	}
}
