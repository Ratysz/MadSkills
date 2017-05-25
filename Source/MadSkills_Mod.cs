using System.Reflection;

using Harmony;
using Verse;
using UnityEngine;

namespace RTMadSkills
{
	public class MadSkills_Mod : Mod
	{
		public MadSkills_ModSettings settings;

		public MadSkills_Mod(ModContentPack content) : base(content)
		{
			var harmony = HarmonyInstance.Create("io.github.ratysz.madskills");
			harmony.PatchAll(Assembly.GetExecutingAssembly());

			settings = GetSettings<MadSkills_ModSettings>();
		}

		public override string SettingsCategory()
		{
			return settings.SettingsCategory();
		}

		public override void DoSettingsWindowContents(Rect inRect)
		{
			settings.DoSettingsWindowContents(inRect);
		}
	}
}
