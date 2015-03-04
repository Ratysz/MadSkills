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
        private CompProperties_RTMapComponentAdder compProps
        {
            get
            {
                return (CompProperties_RTMapComponentAdder)props;
            }
        }

        public override void PostSpawnSetup()
        {
            if (compProps.assemblyName == null || compProps.mapComponentName == null)
            {
                Log.Warning("MapComponent Injector: target not specified!");
                return;
            }
            if (Find.Map.components.FindAll(x => x.GetType().ToString() == compProps.mapComponentName).Count != 0)
            {
                Log.Message("MapComponent Injector: map already has a " + compProps.mapComponentName + "!");
                parent.Destroy(DestroyMode.Vanish);
            }
            else
            {
                Log.Message("MapComponent Injector: looking for " + compProps.mapComponentName + " in " + compProps.assemblyName + "...");
                Assembly assembly = Assembly.LoadFile(compProps.assemblyName);
                Type type = assembly.GetType(compProps.mapComponentName);
                object mapComponent = Activator.CreateInstance(type, new object[0] { });
                Log.Message("MapComponent Injector: adding " + compProps.mapComponentName + "...");
                Find.Map.components.Add((MapComponent)mapComponent);
                Log.Message("MapComponent Injector: success!");
                parent.Destroy(DestroyMode.Vanish);
            }
        }
    }
}
