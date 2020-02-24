using HarmonyLib;
using RimWorld;
using Verse;
using UnityEngine;
using System.Reflection;
using System.Collections.Generic;

namespace RTMadSkills
{
	[HarmonyPatch(typeof(DefGenerator))]
	[HarmonyPatch("GenerateImpliedDefs_PostResolve")]
	public class ModSettingsDefJockey
	{
		private static FieldInfo greatMemoryDegreeDatasField = AccessTools.Field(typeof(TraitDef), "degreeDatas");

		private static bool valid = false;
		private static TraitDef greatMemory = null;
		private static List<TraitDegreeData> greatMemoryDegreeDatasBackup = null;
		private static List<TraitDegreeData> greatMemoryDegreeDatasNew = null;

		static void Postfix()
		{
			if (!valid)
			{
				greatMemory = DefDatabase<TraitDef>.GetNamed("GreatMemory");
				greatMemoryDegreeDatasBackup = (List<TraitDegreeData>)greatMemoryDegreeDatasField.GetValue(greatMemory);

				StatModifier modifier = new StatModifier();
				modifier.stat = DefDatabase<StatDef>.GetNamed("GlobalLearningFactor");
				modifier.value = 0.25f;
				TraitDegreeData data = new TraitDegreeData();
				data.statOffsets = new List<StatModifier>
				{
					modifier
				};
				data.label = greatMemoryDegreeDatasBackup[0].label;
				data.description = "MadSkills_AlternativeGreatMemoryDescription".Translate();
				greatMemoryDegreeDatasNew = new List<TraitDegreeData>
				{
					data
				};

				valid = true;
			}
			ApplyChanges(ModSettings.greatMemoryAltered);
		}

		static public void ApplyChanges(bool altered)
		{
			if (valid)
			{
				if (altered)
				{
					greatMemory.degreeDatas = greatMemoryDegreeDatasNew;
					Log.Message("[MadSkills]: changed behavior of Great Memory trait.");
				}
				else
				{
					greatMemory.degreeDatas = greatMemoryDegreeDatasBackup;
					Log.Message("[MadSkills]: restored behavior of Great Memory trait.");
				}
			}
		}
	}

	public class ModSettings : Verse.ModSettings
	{
		public static bool tiered = false;
		public static bool greatMemoryAltered = true;
		private static int multiplierPercentage = 0;
		public static float multiplier
		{
			get
			{
				return multiplierPercentage / 100.0f;
			}
			set
			{
				multiplierPercentage = Mathf.RoundToInt(multiplier * 100);
			}
		}
		public static float dailyXPSaturationThreshold = 4000.0f;
		public static float saturatedXPMultiplier
		{
			get
			{
				return saturatedXPmultiplierPercentage / 100.0f;
			}
			set
			{
				saturatedXPmultiplierPercentage = Mathf.RoundToInt(multiplier * 100);
			}
		}
		private static int saturatedXPmultiplierPercentage = 20;

		public override void ExposeData()
		{
			float multiplier_shadow = multiplier;
			float saturatedXPMultiplier_shadow = saturatedXPMultiplier;
			Scribe_Values.Look(ref tiered, "tiered");
			Scribe_Values.Look(ref greatMemoryAltered, "greatMemoryAltered");
			Scribe_Values.Look(ref multiplier_shadow, "multiplier");
			Scribe_Values.Look(ref dailyXPSaturationThreshold, "dailyXPSaturationThreshold");
			Scribe_Values.Look(ref saturatedXPMultiplier_shadow, "saturatedXPMultiplier");
			Log.Message("[MadSkills]: settings initialized, multiplier is " + multiplier_shadow
				+ ", " + (tiered ? "tiered" : "not tiered")
				+ ", daily XP threshold is " + dailyXPSaturationThreshold
				+ ", saturated XP multiplier is " + saturatedXPMultiplier
				+ ", Great Memory trait is " + (greatMemoryAltered ? "" : "not ") + "altered.");
			ModSettingsDefJockey.ApplyChanges(greatMemoryAltered);
			multiplierPercentage = Mathf.RoundToInt(multiplier_shadow * 100);
			saturatedXPmultiplierPercentage = Mathf.RoundToInt(saturatedXPMultiplier_shadow * 100);
			base.ExposeData();
		}

		public string SettingsCategory()
		{
			return "MadSkills_SettingsCategory".Translate();
		}

		public void DoSettingsWindowContents(Rect rect)
		{
			Listing_Standard list = new Listing_Standard(GameFont.Small);
			list.ColumnWidth = rect.width / 3;
			list.Begin(rect);
			list.Gap();
			{
				string buffer = multiplierPercentage.ToString();
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
			list.CheckboxLabeled(
				"MadSkills_AlterGreatMemoryLabel".Translate(),
				ref greatMemoryAltered,
				"MadSkills_AlterGreatMemoryTip".Translate());
			list.Gap();
			{
				string buffer = dailyXPSaturationThreshold.ToString();
				Rect rectLine = list.GetRect(Text.LineHeight);
				Rect rectLeft = rectLine.LeftHalf().Rounded();
				Rect rectRight = rectLine.RightHalf().Rounded();
				Widgets.DrawHighlightIfMouseover(rectLine);
				TooltipHandler.TipRegion(rectLine, "MadSkills_DailyXPSaturationThresholdTip".Translate());
				TextAnchor anchorBuffer = Text.Anchor;
				Text.Anchor = TextAnchor.MiddleLeft;
				Widgets.Label(rectLeft, "MadSkills_DailyXPSaturationThresholdLabel".Translate());
				Text.Anchor = anchorBuffer;
				Widgets.TextFieldNumeric(rectRight, ref dailyXPSaturationThreshold, ref buffer, 0, 100000);
			}
			{
				string buffer = saturatedXPmultiplierPercentage.ToString();
				Rect rectLine = list.GetRect(Text.LineHeight);
				Rect rectLeft = rectLine.LeftHalf().Rounded();
				Rect rectRight = rectLine.RightHalf().Rounded();
				Rect rectPercent = rectRight.RightPartPixels(Text.LineHeight);
				rectRight = rectRight.LeftPartPixels(rectRight.width - Text.LineHeight);
				Widgets.DrawHighlightIfMouseover(rectLine);
				TooltipHandler.TipRegion(rectLine, "MadSkills_SaturatedXPMultiplierTip".Translate());
				TextAnchor anchorBuffer = Text.Anchor;
				Text.Anchor = TextAnchor.MiddleLeft;
				Widgets.Label(rectLeft, "MadSkills_SaturatedXPMultiplierLabel".Translate());
				Text.Anchor = anchorBuffer;
				Widgets.TextFieldNumeric(rectRight, ref saturatedXPmultiplierPercentage, ref buffer, 0, 10000);
				Widgets.Label(rectPercent, "%");
			}
			list.End();
		}
	}
}
