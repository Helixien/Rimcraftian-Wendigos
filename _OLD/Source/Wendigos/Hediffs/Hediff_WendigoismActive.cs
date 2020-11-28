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
    public class Hediff_WendigoismActive : HediffWithComps
    {
        //public override void PostAdd(DamageInfo? dinfo)
        //{
        //    base.PostAdd(dinfo);
        //    if (pawn.kindDef == RCW_PawnKindDefOf. && pawn.story != null)
        //    {
        //        pawn.story.melanin = 0f;
        //    }
        //}
        public override void Tick()
        {
            base.Tick();
            if (pawn != null)
            {
                int hourInt = GenLocalDate.HourInteger(pawn.Tile);
                if (hourInt >= 5 && hourInt <= 19)
                {
                    this.Severity = 0.19f;
                }
                else if (hourInt >= 19 || hourInt <= 5)
                {
                    this.Severity = 1f;
                }
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
        }
    }
}
