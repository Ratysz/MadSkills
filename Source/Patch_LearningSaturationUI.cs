using Harmony;
using RimWorld;
using Verse;
using UnityEngine;
using System.Text;

namespace RTMadSkills
{
	[HarmonyPatch(typeof(SkillUI))]
	[HarmonyPatch("GetSkillDescription")]
	static class Patch_LearningSaturationUI
	{
		static void Postfix(ref string __result, SkillRecord sk)
		{
			StringBuilder stringRebuilder = new StringBuilder(__result);
			StringBuilder oldStringBuilder = new StringBuilder();
			if (sk.LearningSaturatedToday)
			{
				oldStringBuilder.AppendLine();
				oldStringBuilder.Append("LearnedMaxToday".Translate(new object[]
				{
					sk.xpSinceMidnight,
					4000,
					0.2f.ToStringPercent("F0")
				}));
			}
			oldStringBuilder.AppendLine();
			oldStringBuilder.AppendLine();
			oldStringBuilder.Append(sk.def.description);
			StringBuilder newStringBuilder = new StringBuilder();
			if (ModSettings.saturatedXPMultiplier != 1.0f
				&& sk.xpSinceMidnight > ModSettings.dailyXPSaturationThreshold)
			{
				newStringBuilder.AppendLine();
				newStringBuilder.Append("LearnedMaxToday".Translate(new object[]
				{
					sk.xpSinceMidnight,
					Mathf.RoundToInt(ModSettings.dailyXPSaturationThreshold),
					ModSettings.saturatedXPMultiplier.ToStringPercent("F0")
				}));
			}
			stringRebuilder.Replace(oldStringBuilder.ToString(), newStringBuilder.ToString());
			stringRebuilder.AppendLine();
			stringRebuilder.AppendLine();
			stringRebuilder.Append(sk.def.description);
			__result = stringRebuilder.ToString();
		}
	}
}
