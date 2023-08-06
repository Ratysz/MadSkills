using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Verse;

namespace RTMadSkills
{
	[HarmonyPriority(Priority.HigherThanNormal)]
	[HarmonyPatch(typeof(SkillRecord))]
	[HarmonyPatch("Interval")]
	internal static class Patch_SkillRecordInterval
	{
		private static FieldInfo pawnField = AccessTools.Field(typeof(SkillRecord), "pawn");
		// VSE compatible
		private static bool VSE = ModLister.HasActiveModWithName("Vanilla Skills Expanded");
        private static MethodInfo ForgetRateFactor = AccessTools.Method("VSE.Passions.PassionManager:ForgetRateFactor");

		private static bool Prefix(SkillRecord __instance)
		{
			if (ModSettings.sleepStopDecaying && !(pawnField.GetValue(__instance) as Pawn).Awake())
			{
				return false;
			}
			if (!ModSettings.tiered || __instance.XpProgressPercent > 0.1f)
			{
				float greatMemMultiplier = (ModSettings.greatMemoryAltered || !(pawnField.GetValue(__instance) as Pawn).story.traits.HasTrait(TraitDefOf.GreatMemory)) ? 1f : 0.5f;
				float xpToLearn = greatMemMultiplier * VanillaMultiplier(__instance.levelInt) * ModSettings.multiplier;
				if (VSE)
				{
					xpToLearn *= (float)ForgetRateFactor.Invoke(null, new object[] { __instance });
				}
				if (xpToLearn != 0.0f)
				{
					__instance.Learn(xpToLearn, false);
				}
			}
			return false;
		}

		private static float VanillaMultiplier(int level)
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
	internal static class Patch_SkillRecordLearnRateFactor
	{
		private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			var _instructions = instructions.MethodReplacer(
				AccessTools.Property(typeof(SkillRecord), nameof(SkillRecord.LearningSaturatedToday)).GetGetMethod(),
				AccessTools.Method(typeof(Patch_SkillRecordLearnRateFactor), nameof(Patch_SkillRecordLearnRateFactor.LearningSaturatedToday))
			);
			var patched = false;
			foreach (var instruction in _instructions)
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

		private static bool LearningSaturatedToday(SkillRecord instance)
		{
			return instance.xpSinceMidnight > ModSettings.dailyXPSaturationThreshold;
		}
	}

	[HarmonyPatch(typeof(SkillRecord))]
	[HarmonyPatch("LearningSaturatedToday", MethodType.Getter)]
	internal static class Patch_SkillRecordLearningSaturatedToday
	{
		private static bool Prefix(SkillRecord __instance, ref bool __result)
		{
			__result = __instance.xpSinceMidnight > ModSettings.dailyXPSaturationThreshold;
			return false;
		}
	}
}