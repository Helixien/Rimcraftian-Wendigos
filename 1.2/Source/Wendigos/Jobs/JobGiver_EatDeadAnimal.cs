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
					if (mentalState.prey.Corpse != null)
					{
						Job job = JobMaker.MakeJob(JobDefOf.Ingest, mentalState.prey.Corpse);
						job.count = 1;
						Log.Message(pawn + " - ingest 4 " + mentalState.prey.Corpse, true);
						return job;
					}
					else
					{
						Job job = JobMaker.MakeJob(JobDefOf.Ingest, mentalState.prey);
						job.count = 1;
						Log.Message(pawn + " - ingest 3 " + mentalState.prey, true);
						return job;
					}
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
