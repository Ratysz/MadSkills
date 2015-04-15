using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Verse;
using RimWorld;

namespace RTMadSkills
{
    public class MapComponent_RTSkillIncreaser : MapComponent
    {
        public override void MapComponentTick()
        {
            foreach (Pawn pawn in Find.Map.listerPawns.AllPawns)
            {       // Iterate over each pawn.
                if (pawn != null && pawn.skills != null && pawn.skills.skills != null && (Find.TickManager.TicksGame + pawn.thingIDNumber) % 200 == 0)
                {       // NRE safety, because screw it; same check is done by Pawn_SkillTracker.SkillsTick()
                    foreach (SkillRecord skillRecord in pawn.skills.skills)
                    {       // Iterate over pawn's skills.
                        switch (skillRecord.level)
                        {       // Similar thing happens in SkillRecord.Interval()
                            case 10:
                                skillRecord.Learn(0.5f * 0.1f / skillRecord.LearningFactor);     // SkillRecord.Interval() pre-multiplies positive gains.
                                break;
                            case 11:
                                skillRecord.Learn(0.5f * 0.2f / skillRecord.LearningFactor);
                                break;
                            case 12:
                                skillRecord.Learn(0.5f * 0.4f / skillRecord.LearningFactor);
                                break;
                            case 13:
                                skillRecord.Learn(0.5f * 0.65f / skillRecord.LearningFactor);
                                break;
                            case 14:
                                skillRecord.Learn(0.5f * 1f / skillRecord.LearningFactor);
                                break;
                            case 15:
                                skillRecord.Learn(0.5f * 1.5f / skillRecord.LearningFactor);
                                break;
                            case 16:
                                skillRecord.Learn(0.5f * 2f / skillRecord.LearningFactor);
                                break;
                            case 17:
                                skillRecord.Learn(0.5f * 3f / skillRecord.LearningFactor);
                                break;
                            case 18:
                                skillRecord.Learn(0.5f * 4f / skillRecord.LearningFactor);
                                break;
                            case 19:
                                skillRecord.Learn(0.5f * 6f / skillRecord.LearningFactor);
                                break;
                            case 20:
                                skillRecord.Learn(0.5f * 8f / skillRecord.LearningFactor);
                                break;
                        }
                    }
                }
            }
        }
    }
}