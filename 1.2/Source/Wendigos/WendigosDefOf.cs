﻿using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace Wendigos
{
	[DefOf]
	public static class WendigosDefOf
	{
		public static HediffDef RCW_Wendigoism;
		public static HediffDef RCW_WendigoismActive;
		public static HediffDef RCW_HumanMeatAddiction;
		public static NeedDef RCW_NeedHumanMeat;
		public static ThingDef RCW_WendigoFledgling;
		public static ThingDef RCW_FeralWendigo;
		public static ThingDef RCW_AncientFeral;
		public static ThingDef RCW_AncientHerder;
		public static ThingDef RCW_AncientStalker;

	}

	[DefOf]
	public static class RCW_PawnKindDefOf
    {
		public static PawnKindDef RCW_WendigoFledgling;
        public static PawnKindDef RCW_WendigoFeralControlled;
        public static PawnKindDef RCW_AncientFeral;
        public static PawnKindDef RCW_AncientHerder;
        public static PawnKindDef RCW_AncientStalker;
	}
}

