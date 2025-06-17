using HarmonyLib;
using RimWorld;
using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace MoreReasonableRoomSpace
{
    [StaticConstructorOnStartup]
    internal static class InitializeHarmony
    {
        static InitializeHarmony()
        {
            var harmony = new Harmony("kagami.MoreReasonableRoomSpace");
            harmony.PatchAll();
        }
    }
    [StaticConstructorOnStartup]
    internal static class MrrsStartUp
    {
        static MrrsSettings settings;
        static MrrsStartUp()
        {
            settings = LoadedModManager.GetMod<MrrsMod>().GetSettings<MrrsSettings>();
            var scoreStages = DefDatabase<RoomStatDef>.GetNamed("Space").scoreStages;
            foreach (var stage in scoreStages)
            {
                //Log.Warning($"Stage: {stage.label} - MinScore: {stage.minScore}");
                switch (stage.untranslatedLabel)
                {
                    case "rather tight":
                        stage.minScore = settings.Rathertight; break;
                    case "averaged-sized":
                        stage.minScore = settings.AverageSized; break;
                    case "somewhat spacious":
                        stage.minScore = settings.SomewhatSpacious; break;
                    case "quite spacious":
                        stage.minScore = settings.QuiteSpacious; break;
                    case "very spacious":
                        stage.minScore = settings.VerySpacious; break;
                    case "extremely spacious":
                        stage.minScore = settings.ExtremelySpacious; break;
                    default:
                        break;
                }
                //Log.Warning($"Updated Stage: {stage.label} - MinScore: {stage.minScore}");
            }
        }
    }

    [HarmonyPatch(typeof(RoomStatWorker_Space), nameof(RoomStatWorker_Space.GetScore))]
    public static class PatchSpace
    {
        public static bool Prefix(Room room, ref float __result)
        {
            if (room.PsychologicallyOutdoors)
            {
                __result = 350f;
                return false;
            }
            float totalCellCount = room.Cells.Count();
            int standableCellCount = 0;
            foreach (var cell in room.Cells)
            {
                if (cell.Standable(room.Map)) standableCellCount++;
            }
            var spacefullness = (350 * (standableCellCount) / totalCellCount);
            var penalty = Math.Min(totalCellCount / MrrsMod.settings.PenaltyMinSize, 1);
            __result = (float)Math.Round(penalty * spacefullness, 2);
            return false;
        }
    }
    [HarmonyPatch(typeof(RoomStatWorker_Impressiveness), nameof(RoomStatWorker_Impressiveness.GetScore))]
    public static class PatchImpressiveness
    {
        public static bool Prefix(Room room, ref float __result)
        {
            double factor1 = (double)GetFactor(room.GetStat(RoomStatDefOf.Wealth) / 1500f);
            float factor2 = GetFactor(room.GetStat(RoomStatDefOf.Beauty) / 3f);
            float factor3 = GetFactor(room.GetStat(RoomStatDefOf.Space) / 35f);
            float factor4 = GetFactor((float)(1.0 + (double)Mathf.Min(room.GetStat(RoomStatDefOf.Cleanliness), 0.0f) / 2.5));
            float a = Mathf.Lerp((float)((factor1 + (double)factor2 + (double)factor3 + (double)factor4) / 4.0), Mathf.Min((float)factor1, Mathf.Min(factor2, Mathf.Min(factor3, factor4))), 0.35f);
            float b = factor3 * 5f;
            if ((double)a > (double)b)
                a = Mathf.Lerp(a, b, 0.75f);
            __result = a * 100f;
            return false;
        }
        private static float GetFactor(float baseFactor)
        {
            if ((double)Mathf.Abs(baseFactor) < 1.0)
                return baseFactor;
            return (double)baseFactor > 0.0 ? 1f + Mathf.Log(baseFactor) : -1f - Mathf.Log(-baseFactor);
        }
    }
}
