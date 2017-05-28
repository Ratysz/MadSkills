using Harmony;
using RimWorld;
using Verse;
using System.Collections.Generic;

namespace RTMadSkills
{
	[HarmonyPatch(typeof(SkillRecord))]
	[HarmonyPatch("LearningSaturatedToday", PropertyMethod.Getter)]
	static class Patch_LearningSaturation
	{
		static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			return instructions.MethodReplacer(
				typeof(SkillRecord).GetProperty("LearningSaturatedToday").GetAccessors()[0],
				typeof(Patch_LearningSaturation).GetMethod("IsSaturated"));
		}

		static bool IsSaturated(SkillRecord __instance)
		{
			Log.Message("ping");
			return true;
		}
	}
}
