using RimWorld;
using System.Collections.Generic;
using Verse;

namespace Wendigos
{
	public enum HumanMeatDesireCategory : byte
	{
		Withdrawal,
		Desire,
		Satisfied
	}
	public class Need_HumanMeat : Need
	{
		private const float ThreshDesire = 0.01f;

		private const float ThreshSatisfied = 0.1f;

		public override int GUIChangeArrow => -1;

		public HumanMeatDesireCategory CurCategory
		{
			get
			{
				if (CurLevel > ThreshSatisfied)
				{
					return HumanMeatDesireCategory.Satisfied;
				}
				if (CurLevel > ThreshDesire)
				{
					return HumanMeatDesireCategory.Desire;
				}
				return HumanMeatDesireCategory.Withdrawal;
			}
		}

		public override float CurLevel
		{
			get
			{
				return base.CurLevel;
			}
			set
			{
				HumanMeatDesireCategory curCategory = CurCategory;
				base.CurLevel = value;
				if (CurCategory != curCategory)
				{
					CategoryChanged();
				}
			}
		}

		public Hediff_Addiction AddictionHediff
		{
			get
			{
				List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
				for (int i = 0; i < hediffs.Count; i++)
				{
					Hediff_Addiction hediff_Addiction = hediffs[i] as Hediff_Addiction;
					if (hediff_Addiction != null && hediff_Addiction.def.causesNeed == def)
					{
						return hediff_Addiction;
					}
				}
				return null;
			}
		}

		private float NeedFallPerTick => def.fallPerDay / 60000f;

		public Need_HumanMeat(Pawn pawn)
			: base(pawn)
		{
			threshPercents = new List<float>();
			threshPercents.Add(0.1f);
		}

		public override void SetInitialLevel()
		{
			base.CurLevelPercentage = Rand.Range(0.8f, 1f);
		}

		public override void NeedInterval()
		{
			if (!IsFrozen)
			{
				CurLevel -= NeedFallPerTick * 150f;
			}
		}

		private void CategoryChanged()
		{
			AddictionHediff?.Notify_NeedCategoryChanged();
		}
	}
}
