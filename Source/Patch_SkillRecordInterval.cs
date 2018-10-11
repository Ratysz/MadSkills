using Harmony;
using RimWorld;
using Verse;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace RTMadSkills
{
	[HarmonyPriority(Priority.HigherThanNormal)]
	[HarmonyPatch(typeof(SkillRecord))]
	[HarmonyPatch("Interval")]
	static class Patch_SkillRecordInterval
	{
		static FieldInfo pawnField = AccessTools.Field(typeof(SkillRecord), "pawn");

		static bool Prefix(SkillRecord __instance)
		{
			if (!ModSettings.tiered || __instance.XpProgressPercent > 0.1f)
			{
				float greatMemMultiplier = (ModSettings.greatMemoryAltered || !(pawnField.GetValue(__instance) as Pawn).story.traits.HasTrait(TraitDefOf.GreatMemory)) ? 1f : 0.5f;
				float xpToLearn = greatMemMultiplier * VanillaMultiplier(__instance.levelInt) * ModSettings.multiplier;
				if (xpToLearn != 0.0f)
				{
					__instance.Learn(xpToLearn, false);
				}
			}
			return false;
		}

		public static float VanillaMultiplier(int level)
		{
			switch (level)
			{
				case 10: return -0.1f;
				case 11: return -0.2f;
				case 12: return -0.4f;
				case 13: return -0.6f;
				case 14: return -1.0f;
				case 15: return -1.8f;
				case 16: return -2.8f;
				case 17: return -4.0f;
				case 18: return -6.0f;
				case 19: return -8.0f;
				case 20: return -12.0f;
				default: return 0.0f;
			}
		}
	}
}
