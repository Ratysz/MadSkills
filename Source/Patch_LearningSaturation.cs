using Harmony;
using RimWorld;
using Verse;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace RTMadSkills
{
	[HarmonyPatch(typeof(SkillRecord))]
	[HarmonyPatch("LearnRateFactor")]
	static class Patch_LearningSaturation
	{
		static void Postfix(SkillRecord __instance, ref float __result, bool direct = false)
		{
			if (!direct)
			{
				if (__instance.LearningSaturatedToday)
				{
					__result /= 0.2f;
				}
				if (ModSettings.saturatedXPMultiplier != 1.0f
					&& __instance.xpSinceMidnight > ModSettings.dailyXPSaturationThreshold)
				{
					__result *= ModSettings.saturatedXPMultiplier;
				}
			}
		}
	}
}