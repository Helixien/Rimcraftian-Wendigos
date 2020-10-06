using RimWorld;
using Verse;

namespace Wendigos
{
	public class ThoughtWorker_FledlingTemperature : ThoughtWorker
	{
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return p.kindDef == RCW_PawnKindDefOf.RCW_WendigoFledgling && p.AmbientTemperature > 21f;
		}
	}
}
