using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace MoreReasonableRoomSpace
{
    public class MrrsSettings : ModSettings
    {
        public float Rathertight = 15f;
        public float AverageSized = 30f;
        public float SomewhatSpacious = 45f;
        public float QuiteSpacious = 60f;
        public float VerySpacious = 75f;
        public float ExtremelySpacious = 85f;

        public int PenaltyMinSize = 25;

        public override void ExposeData()
        {
            Scribe_Values.Look(ref Rathertight, nameof(Rathertight), 15f);
            Scribe_Values.Look(ref AverageSized, nameof(AverageSized), 30f);
            Scribe_Values.Look(ref SomewhatSpacious, nameof(SomewhatSpacious), 45f);
            Scribe_Values.Look(ref QuiteSpacious, nameof(QuiteSpacious), 60f);
            Scribe_Values.Look(ref VerySpacious, nameof(VerySpacious), 75f);
            Scribe_Values.Look(ref ExtremelySpacious, nameof(ExtremelySpacious), 85f);

            Scribe_Values.Look(ref PenaltyMinSize, nameof(PenaltyMinSize), 25);
            base.ExposeData();
        }
    }
    public class MrrsMod : Mod
    {
        public static MrrsSettings settings;

        public MrrsMod(ModContentPack content) : base(content)
        {
            settings = GetSettings<MrrsSettings>();
        }
        public override string SettingsCategory() => "MoreReasnonableRoomSpace".Translate();

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listing = new Listing_Standard();
            listing.Begin(inRect);
            listing.Label("SpacefullnessExplain".Translate());
            listing.GapLine();
            listing.Label("SpacefullnessLabel".Translate());

            settings.Rathertight = listing.FloatEntry(nameof(settings.Rathertight).Translate(), settings.Rathertight, 0f, 100f);
            settings.AverageSized = listing.FloatEntry(nameof(settings.AverageSized).Translate(), settings.AverageSized, 0f, 100f);
            settings.SomewhatSpacious = listing.FloatEntry(nameof(settings.SomewhatSpacious).Translate(), settings.SomewhatSpacious, 0f, 100f);
            settings.QuiteSpacious = listing.FloatEntry(nameof(settings.QuiteSpacious).Translate(), settings.QuiteSpacious, 0f, 100f);
            settings.VerySpacious = listing.FloatEntry(nameof(settings.VerySpacious).Translate(), settings.VerySpacious, 0f, 100f);
            settings.ExtremelySpacious = listing.FloatEntry(nameof(settings.ExtremelySpacious).Translate(), settings.ExtremelySpacious, 0f, 100f);

            listing.GapLine();
            listing.Label("PenaltyExplain".Translate());
            var penaltyBuffer = settings.PenaltyMinSize.ToString();
            listing.IntEntry(ref settings.PenaltyMinSize, ref penaltyBuffer);
            listing.Label("PenaltyFormula".Translate());

            listing.Label("Comment".Translate());
            listing.GapLine();

            bool buttonPressed = listing.ButtonText("Reset".Translate());

            listing.End();
            base.DoSettingsWindowContents(inRect);
            if (buttonPressed)
            {
                settings.Rathertight = 15f;
                settings.AverageSized = 30f;
                settings.SomewhatSpacious = 45f;
                settings.QuiteSpacious = 60f;
                settings.VerySpacious = 75f;
                settings.ExtremelySpacious = 85f;
                settings.PenaltyMinSize = 25;
            }
        }
    }
    public static class Extensions
    {
        public static float FloatEntry(this Listing_Standard ls, string label, float val, float min, float max)
        {
            var buffer = val.ToString("F2");
            ls.TextFieldNumericLabeled(label, ref val, ref buffer, min, max);
            return val;
        }
    }
}
