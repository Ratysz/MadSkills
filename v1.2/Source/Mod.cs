using System.Reflection;
using HarmonyLib;
using UnityEngine;
using Verse;

namespace RTMadSkills
{
	public class Mod : Verse.Mod
	{
		public ModSettings settings;

		public Mod(ModContentPack content) : base(content)
		{
			var harmony = new Harmony("io.github.ratysz.madskills");
			harmony.PatchAll(Assembly.GetExecutingAssembly());

			settings = GetSettings<ModSettings>();
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
