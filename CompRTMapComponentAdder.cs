using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Verse;
using RimWorld;

namespace RTMadSkills
{
    class CompRTMapComponentAdder : ThingComp
    {
        public override void PostSpawnSetup()
        {
            if (Find.Map.GetComponent<MapComponent_RTSkillIncreaser>() != null)
            {
                return;
            }
            else
            {
                Find.Map.components.Add(new MapComponent_RTSkillIncreaser());
                parent.Destroy(DestroyMode.Kill);
            }
        }
    }
}
