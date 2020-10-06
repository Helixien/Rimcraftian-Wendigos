using RimWorld;
using Verse.AI;
using Verse.AI.Group;

namespace Wendigos
{
	public class LordToil_NightWendigoAssaultColony : LordToil
	{
		private bool attackDownedIfStarving;
		public override bool ForceHighStoryDanger => true;
		public override bool AllowSatisfyLongNeeds => false;
		public LordToil_NightWendigoAssaultColony(bool attackDownedIfStarving = false)
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
				lord.ownedPawns[i].mindState.duty = new PawnDuty(WendigosDefOf.RCW_WendigoAssaultColony);
				lord.ownedPawns[i].mindState.duty.attackDownedIfStarving = attackDownedIfStarving;
			}
		}
	}
}
