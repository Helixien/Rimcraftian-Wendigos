using RimWorld;
using Verse;
using Verse.AI;

namespace Wendigos
{
	public class JobGiver_EatDeadAnimal : ThinkNode_JobGiver
	{
		protected override Job TryGiveJob(Pawn pawn)
		{
			var mentalState = pawn.MentalState as MentalState_FrenzyHunt;
			if (mentalState == null)
            {
				Log.Message(pawn + " - return null 1", true);
				return null;
            }
			if (mentalState.prey != null)
            {
				if (!mentalState.prey.Dead)
                {
					Log.Message(pawn + " - melee attack " + mentalState.prey, true);
					return MeleeAttackJob(pawn, mentalState.prey);
				}
				else
                {
					Log.Message(pawn + " - ingest " + mentalState.prey, true);
					return JobMaker.MakeJob(JobDefOf.Ingest, mentalState.prey);
                }
            }
			Log.Message(pawn + " - return null 2", true);
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
	}
}
