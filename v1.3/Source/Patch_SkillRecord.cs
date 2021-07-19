using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Verse;

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

		static float VanillaMultiplier(int level)
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

	[HarmonyPatch(typeof(SkillRecord))]
	[HarmonyPatch("LearnRateFactor")]
	static class Patch_LearningSaturation
	{
		static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			var patched = false;
			foreach (var instruction in instructions)
			{
				if (!patched && instruction.opcode == OpCodes.Ldc_R4 && System.Convert.ToSingle(instruction.operand) == 0.2f)
				{
					patched = true;
					yield return new CodeInstruction(OpCodes.Call, AccessTools.PropertyGetter(typeof(ModSettings), nameof(ModSettings.saturatedXPMultiplier)));
					continue;
				}
				yield return instruction;
			}
		}
	}

	[HarmonyPatch(typeof(SkillRecord))]
	[HarmonyPatch("LearningSaturatedToday", MethodType.Getter)]
	static class Patch_SkillRecordLearningSaturatedToday
	{
		static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			var patched = false;
			foreach (var instruction in instructions)
			{
				if (!patched && instruction.opcode == OpCodes.Ldc_R4 && System.Convert.ToSingle(instruction.operand) == 4000f)
				{
					patched = true;
					yield return new CodeInstruction(OpCodes.Ldsfld, AccessTools.Field(typeof(ModSettings), nameof(ModSettings.dailyXPSaturationThreshold)));
					continue;
				}
				yield return instruction;
			}
		}
	}
}