using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;
using Verse;

namespace RTMadSkills
{
	[HarmonyPatch(typeof(SkillUI))]
	[HarmonyPatch("GetSkillDescription")]
	internal static class Patch_LearningSaturationUI
	{
		private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			int patchState = 0;
			foreach (var instruction in instructions)
			{
				if (patchState == 0 && instruction.opcode == OpCodes.Ldc_I4 && System.Convert.ToInt32(instruction.operand) == 4000)
				{
					patchState++;
					yield return new CodeInstruction(OpCodes.Ldsfld, AccessTools.Field(typeof(ModSettings), nameof(ModSettings.dailyXPSaturationThreshold)));
					yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(System.Convert), nameof(System.Convert.ToInt32), new System.Type[] { typeof(float) }));
					continue;
				}
				else if (patchState == 1 && instruction.opcode == OpCodes.Ldc_R4 && System.Convert.ToSingle(instruction.operand) == 0.2f)
				{
					patchState++;
					yield return new CodeInstruction(OpCodes.Call, AccessTools.PropertyGetter(typeof(ModSettings), nameof(ModSettings.saturatedXPMultiplier)));
					continue;
				}
				yield return instruction;
			}
		}
	}
}