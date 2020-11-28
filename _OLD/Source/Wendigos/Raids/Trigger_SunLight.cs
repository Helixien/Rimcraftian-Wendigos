using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace Wendigos
{
	public class Trigger_SunLight : Trigger
	{
		public Trigger_SunLight()
		{

		}

		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.Tick)
			{
				return !WendigosUtils.IsNightNow(lord.Map);
			}
			return false;
		}
	}
}
