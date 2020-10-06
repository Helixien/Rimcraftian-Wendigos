using RimWorld;
using RimWorld.Planet;
using Verse;

namespace Wendigos
{
	public class SitePartWorker_WendigoAmbush : SitePartWorker
	{
		private const float ThreatPointsFactor = 0.8f;
		public override SitePartParams GenerateDefaultParams(float myThreatPoints, int tile, Faction faction)
		{
			SitePartParams sitePartParams = base.GenerateDefaultParams(myThreatPoints, tile, faction);
			sitePartParams.threatPoints *= 0.8f;
			return sitePartParams;
		}
        public override void PostMapGenerate(Map map)
        {
            base.PostMapGenerate(map);
			Log.Message(map + " is generated", true);
			var faction = Find.FactionManager.FirstFactionOfDef(WendigosDefOf.RCW_FeralWendigos);
			map.Parent.SetFaction(faction);
        }
    }
}
