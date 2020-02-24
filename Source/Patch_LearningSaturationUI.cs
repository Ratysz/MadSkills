using Harmony;
using RimWorld;
using System.Text;
using UnityEngine;
using Verse;

namespace RTMadSkills
{
	[HarmonyPatch(typeof(SkillUI))]
	[HarmonyPatch("GetSkillDescription")]
	internal static class Patch_LearningSaturationUI
	{
		private static void Postfix(ref string __result, SkillRecord sk)
		{
			if (ModSettings.saturatedXPMultiplier != 1.0f
				&& sk.xpSinceMidnight > ModSettings.dailyXPSaturationThreshold)
			{
				StringBuilder stringRebuilder = new StringBuilder(__result);
				StringBuilder oldStringBuilder = new StringBuilder();
				StringBuilder newStringBuilder = new StringBuilder();
				if (sk.LearningSaturatedToday)
				{
					oldStringBuilder.Append("LearnedMaxToday".Translate(
						sk.xpSinceMidnight.ToString("F0"),
						4000,
						0.2f.ToStringPercent("F0")
					));
					newStringBuilder.Append("LearnedMaxToday".Translate(
						sk.xpSinceMidnight.ToString("F0"),
						Mathf.RoundToInt(ModSettings.dailyXPSaturationThreshold),
						ModSettings.saturatedXPMultiplier.ToStringPercent("F0")
					));
					stringRebuilder.Replace(oldStringBuilder.ToString(), newStringBuilder.ToString());
				}
				else
				{
					oldStringBuilder.AppendLine();
					oldStringBuilder.AppendLine();
					oldStringBuilder.Append(sk.def.description);
					newStringBuilder.AppendLine();
					newStringBuilder.Append("LearnedMaxToday".Translate(
						sk.xpSinceMidnight.ToString("F0"),
						Mathf.RoundToInt(ModSettings.dailyXPSaturationThreshold),
						ModSettings.saturatedXPMultiplier.ToStringPercent("F0")
					));
					stringRebuilder.Replace(oldStringBuilder.ToString(), newStringBuilder.ToString());
					stringRebuilder.Append(oldStringBuilder);
				}
				__result = stringRebuilder.ToString();
			}
		}
	}
}