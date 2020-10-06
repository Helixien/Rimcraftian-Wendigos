using RimWorld;
using Verse;
using Verse.AI;

namespace Wendigos
{
	public class JobGiver_DigGraves : ThinkNode_JobGiver
	{
		protected override Job TryGiveJob(Pawn pawn)
		{
			var corpse = CorpseObsessionMentalStateUtility.GetClosestCorpseToDigUp(pawn);
			if (corpse != null && corpse.ParentHolder is Building building_Grave)
            {
				if (pawn.MentalState is MentalState_CorpseConsumption corpseConsumption)
                {
					corpseConsumption.corpse = corpse;
                }
				Job job = JobMaker.MakeJob(WendigosDefOf.RCW_DigUpGrave, corpse, building_Grave);
				job.count = 1;
				return job;
			}
			return null;
		}
	}
}
