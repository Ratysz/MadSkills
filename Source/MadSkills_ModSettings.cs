using Verse;
using UnityEngine;

namespace RTMadSkills
{
	public class MadSkills_ModSettings : ModSettings
	{
		public bool tiered = false;
		public float multiplier = 0.0f;

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look(ref tiered, "tiered");
			Scribe_Values.Look(ref multiplier, "multiplier");
			MadSkills_Patch.ApplySettings(tiered, multiplier);
			Log.Message("[MadSkills]: settings initialized, multiplier is " + multiplier
				+ ", " + (tiered ? "tiered" : "not tiered") + ".");
		}

		public string SettingsCategory()
		{
			return "MadSkills_SettingsCategory".Translate();
		}

		public void DoSettingsWindowContents(Rect rect)
		{
			string buffer = multiplier.ToString();
			Listing_Standard list = new Listing_Standard(GameFont.Small);
			list.ColumnWidth = rect.width / 3;
			list.Begin(rect);
			list.Gap();
			list.Label("MadSkills_MultiplierLabel".Translate());
			list.TextFieldNumeric(ref multiplier, ref buffer, 0.0f, 100.0f);
			list.Label("MadSkills_MultiplierTip".Translate());
			list.GapLine();
			list.CheckboxLabeled(
				"MadSkills_TieredLabel".Translate(),
				ref tiered,
				"MadSkills_TieredTip".Translate());
			list.End();
		}
	}
}
