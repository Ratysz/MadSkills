using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Verse;
using RimWorld;

namespace RTMadSkills
{
    public class CompProperties_RTMapComponentAdder : CompProperties
    {
        public string assemblyName;
        public string mapComponentName;

        public CompProperties_RTMapComponentAdder()
        {
            this.compClass = typeof(CompProperties_RTMapComponentAdder);
        }
    }
}
