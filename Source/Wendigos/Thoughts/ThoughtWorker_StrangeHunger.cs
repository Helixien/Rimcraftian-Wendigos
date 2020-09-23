using RimWorld;
using Verse;

namespace Wendigos
{
	public class ThoughtWorker_StrangeHunger : ThoughtWorker
	{
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return p.health.hediffSet.HasHediff(WendigosDefOf.RCW_Wendigoism) && !p.story.traits.HasTrait(TraitDefOf.Cannibal);
		}
	}
}
