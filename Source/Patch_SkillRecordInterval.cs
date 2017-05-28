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
		static bool Prefix(SkillRecord __instance)
		{
			if (!ModSettings.tiered || __instance.XpProgressPercent > 0.1f)
			{
				float xpToLearn = VanillaMultiplier(__instance.levelInt) * ModSettings.multiplier;
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
				case 10: return -0.10f;
				case 11: return -0.20f;
				case 12: return -0.40f;
				case 13: return -0.65f;
				case 14: return -1.00f;
				case 15: return -1.5f;
				case 16: return -2.00f;
				case 17: return -3.00f;
				case 18: return -4.00f;
				case 19: return -6.00f;
				case 20: return -8.00f;
				default: return 0.0f;
			}
		}
	}
}
