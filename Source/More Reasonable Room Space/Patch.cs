using HarmonyLib;
using RimWorld;
using System;
using System.Diagnostics;
using System.Linq;
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
    public static class Patch
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
}
