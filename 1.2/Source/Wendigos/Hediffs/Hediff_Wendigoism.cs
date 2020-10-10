using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Wendigos
{
    public class Hediff_Wendigoism : HediffWithComps
    {
        public override void Tick()
        {
            base.Tick();
            //if (this.Severity >= 1f)
            //{
            //    bool selected = Find.Selector.IsSelected(this.pawn);
            //    this.pawn.health.RemoveHediff(this);
            //    Pawn newPawn = WendigosUtils.GetPawnDuplicate(this.pawn, RCW_PawnKindDefOf.RCW_WendigoFledgling);
            //    GenSpawn.Spawn(newPawn, this.pawn.Position, this.pawn.Map);
            //    newPawn.health.AddHediff(WendigosDefOf.RCW_HumanMeatAddiction);
            //    newPawn.health.AddHediff(WendigosDefOf.RCW_WendigoismActive);
            //    this.pawn.Destroy(DestroyMode.Vanish);
            //    if (selected) Find.Selector.Select(newPawn);
            //}
        }
    }
}
