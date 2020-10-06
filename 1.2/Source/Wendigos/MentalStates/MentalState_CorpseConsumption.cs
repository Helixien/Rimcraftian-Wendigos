using RimWorld;
using Verse;
using Verse.AI;

namespace Wendigos
{
	public class MentalState_CorpseConsumption : MentalState
	{
		public override bool ForceHostileTo(Thing t)
		{
			return true;
		}

		public override bool ForceHostileTo(Faction f)
		{
			return true;
		}

		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.prey, "prey", false);
			Scribe_References.Look<Corpse>(ref this.corpse, "corpse", false);
		}

		public Pawn prey = null;
		public Corpse corpse = null;
	}
}
