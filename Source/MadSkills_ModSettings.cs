using Verse;
using UnityEngine;

namespace RTMadSkills
{
	public class MadSkills_ModSettings : ModSettings
	{
		public bool tiered = false;
		public int multiplierPercentage = 0;

		public override void ExposeData()
		{
			float multiplier = multiplierPercentage / 100.0f;
			base.ExposeData();
			Scribe_Values.Look(ref tiered, "tiered");
			Scribe_Values.Look(ref multiplier, "multiplier");
			MadSkills_Patch.ApplySettings(tiered, multiplier);
			Log.Message("[MadSkills]: settings initialized, multiplier is " + multiplier
				+ ", " + (tiered ? "tiered" : "not tiered") + ".");
			multiplierPercentage = Mathf.RoundToInt(multiplier * 100);
		}

		public string SettingsCategory()
		{
			return "MadSkills_SettingsCategory".Translate();
		}

		public void DoSettingsWindowContents(Rect rect)
		{
			string buffer = multiplierPercentage.ToString();
			Listing_Standard list = new Listing_Standard(GameFont.Small);
			list.ColumnWidth = rect.width / 3;
			list.Begin(rect);
			list.Gap();
			{
				Rect rectLine = list.GetRect(Text.LineHeight);
				Rect rectLeft = rectLine.LeftHalf().Rounded();
				Rect rectRight = rectLine.RightHalf().Rounded();
				Rect rectPercent = rectRight.RightPartPixels(Text.LineHeight);
				rectRight = rectRight.LeftPartPixels(rectRight.width - Text.LineHeight);
				Widgets.DrawHighlightIfMouseover(rectLine);
				TooltipHandler.TipRegion(rectLine, "MadSkills_MultiplierTip".Translate());
				TextAnchor anchorBuffer = Text.Anchor;
				Text.Anchor = TextAnchor.MiddleLeft;
				Widgets.Label(rectLeft, "MadSkills_MultiplierLabel".Translate());
				Text.Anchor = anchorBuffer;
				Widgets.TextFieldNumeric(rectRight, ref multiplierPercentage, ref buffer, 0, 10000);
				Widgets.Label(rectPercent, "%");
			}
			list.CheckboxLabeled(
				"MadSkills_TieredLabel".Translate(),
				ref tiered,
				"MadSkills_TieredTip".Translate());
			list.End();
		}
	}
}
