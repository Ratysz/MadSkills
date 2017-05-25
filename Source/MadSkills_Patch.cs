using Harmony;
using RimWorld;

namespace RTMadSkills
{
	[HarmonyPriority(Priority.HigherThanNormal)]
	[HarmonyPatch(typeof(SkillRecord), "Interval")]
	static class MadSkills_Patch
	{
		private static bool tiered = false;
		private static float multiplier = 0.0f;

		static bool Prefix(SkillRecord __instance)
		{
			if (!tiered || __instance.XpProgressPercent > 0.1f)
			{
				float xpToLearn = VanillaMultiplier(__instance.levelInt) * multiplier;
				if (xpToLearn != 0.0f)
				{
					__instance.Learn(xpToLearn, false);
				}
			}
			return false;
		}

		public static void ApplySettings(bool tiered, float multiplier)
		{
			MadSkills_Patch.tiered = tiered;
			MadSkills_Patch.multiplier = multiplier;
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
