using RimWorld;
using Verse;

namespace Wendigos
{
	public class GenStep_Ambush_Edge : GenStep_Ambush
	{
		public override int SeedPart => 1412216193;

		protected override SignalAction_Ambush MakeAmbushSignalAction(CellRect rectToDefend, IntVec3 root, GenStepParams parms)
		{

			SignalAction_Ambush signalAction_Ambush = base.MakeAmbushSignalAction(rectToDefend, root, parms);
			signalAction_Ambush.spawnPawnsOnEdge = true;
			signalAction_Ambush.ambushType = SignalActionAmbushType.Normal;
			Log.Message(signalAction_Ambush + " is send");
			return signalAction_Ambush;
		}
	}
}
