using RimWorld;
using Verse.AI;
using Verse.AI.Group;

namespace Wendigos
{
	public class LordToil_CannibalsAssaultColony : LordToil
	{
		private bool attackDownedIfStarving;
		public override bool ForceHighStoryDanger => true;
		public override bool AllowSatisfyLongNeeds => false;
		public LordToil_CannibalsAssaultColony(bool attackDownedIfStarving = false)
		{
			this.attackDownedIfStarving = attackDownedIfStarving;
		}

		public override void Init()
		{
			base.Init();
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.Drafting, OpportunityType.Critical);
		}

		public override void UpdateAllDuties()
		{
			for (int i = 0; i < lord.ownedPawns.Count; i++)
			{
				lord.ownedPawns[i].mindState.duty = new PawnDuty(WendigosDefOf.RCW_CannibalAssaultColony);
				lord.ownedPawns[i].mindState.duty.attackDownedIfStarving = attackDownedIfStarving;
			}
		}
	}
}
