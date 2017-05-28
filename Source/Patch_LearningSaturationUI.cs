using Harmony;
using RimWorld;
using Verse;
using System.Collections.Generic;

namespace RTMadSkills
{
	[HarmonyPatch(typeof(SkillUI))]
	[HarmonyPatch("GetSkillDescription")]
	static class Patch_LearningSaturationUI
	{
		static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			return instructions.MethodReplacer(
				typeof(SkillUI).GetMethod("GetSkillDescription"),
				typeof(Patch_LearningSaturation).GetMethod("PatchedGetSkillDescription"));
		}

		static string PatchedGetSkillDescription(SkillRecord sk)
		{
			Log.Message("PatchedGetSkillDescription");
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			if (sk.TotallyDisabled)
			{
				stringBuilder.Append("DisabledLower".Translate().CapitalizeFirst());
			}
			else
			{
				stringBuilder.AppendLine(string.Concat(new object[]
				{
						"Level".Translate(),
						" ",
						sk.Level,
						": ",
						sk.LevelDescriptor
				}));
				if (Current.ProgramState == ProgramState.Playing)
				{
					string text = (sk.Level != 20) ? "ProgressToNextLevel".Translate() : "Experience".Translate();
					stringBuilder.AppendLine(string.Concat(new object[]
					{
							text,
							": ",
							sk.xpSinceLastLevel.ToString("F0"),
							" / ",
							sk.XpRequiredForLevelUp
					}));
				}
				stringBuilder.Append("Passion".Translate() + ": ");
				switch (sk.passion)
				{
					case Passion.None:
						stringBuilder.Append("PassionNone".Translate(new object[]
						{
								0.333f.ToStringPercent("F0")
						}));
						break;
					case Passion.Minor:
						stringBuilder.Append("PassionMinor".Translate(new object[]
						{
								1f.ToStringPercent("F0")
						}));
						break;
					case Passion.Major:
						stringBuilder.Append("PassionMajor".Translate(new object[]
						{
								1.5f.ToStringPercent("F0")
						}));
						break;
				}
				if (sk.LearningSaturatedToday)
				{
					stringBuilder.AppendLine();
					stringBuilder.Append("LearnedMaxToday".Translate(new object[]
					{
							sk.xpSinceMidnight,
							4000,
							0.2f.ToStringPercent("F0")
					}));
				}
			}
			stringBuilder.AppendLine();
			stringBuilder.AppendLine();
			stringBuilder.Append(sk.def.description);
			return stringBuilder.ToString();
		}
	}
}
