using RimWorld;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace Wendigos
{
	public class MentalState_GoneFeral : MentalState
	{
        public override void PreStart()
        {
            base.PreStart();
			this.map = pawn.Map;
        }
        public override void PostEnd()
        {
            base.PostEnd();
			this.Arrive();
		}

        public void Arrive()
        {
            if (pawn.Faction != Faction.OfPlayer)
            {
                pawn.SetFaction(Faction.OfPlayer);
            }
            IncidentParms incidentParms = new IncidentParms();
            incidentParms.target = this.map;
            incidentParms.spawnCenter = this.map.Center;
            PawnsArrivalModeDef obj = PawnsArrivalModeDefOf.EdgeWalkIn;
            obj.Worker.TryResolveRaidSpawnCenter(incidentParms);
            obj.Worker.Arrive(new List<Pawn>() { pawn }, incidentParms);
            TaggedString title;
            TaggedString text;
            text = "LetterRefugeeJoins".Translate(pawn.Named("PAWN"));
            title = "LetterLabelRefugeeJoins".Translate(pawn.Named("PAWN"));
            Find.LetterStack.ReceiveLetter(title, text, LetterDefOf.PositiveEvent, pawn, null);
        }
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Map>(ref this.map, "map", false);
		}

		public Map map;
	}
}
