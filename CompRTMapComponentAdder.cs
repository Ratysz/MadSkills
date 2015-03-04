using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using UnityEngine;
using Verse;
using RimWorld;

namespace RTMadSkills
{
    class CompRTMapComponentAdder : ThingComp
    {
        public override void PostSpawnSetup()
        {
            string assemblyName = "RTMadSkills.dll";
            string mapComponentName = "RTMadSkills.MapComponent_RTSkillIncreaser";
            if (Find.Map.components.FindAll(x => x.GetType().ToString() == mapComponentName).Count != 0)
            {
                Log.Message("MapComponent already exists!");
                parent.Destroy(DestroyMode.Vanish);
            }
            else
            {
                Log.Message("Looking for MapComponent...");
                Assembly assembly = Assembly.LoadFile(assemblyName);
                Type type = assembly.GetType(mapComponentName);
                object mapComponent = Activator.CreateInstance(type, new object[0] { });
                Log.Message("Adding MapComponent...");
                Find.Map.components.Add((MapComponent)mapComponent);
                Log.Message("Success!");
                parent.Destroy(DestroyMode.Vanish);
            }
        }
    }
}
