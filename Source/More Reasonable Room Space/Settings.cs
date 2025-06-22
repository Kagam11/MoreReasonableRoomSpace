using UnityEngine;
using Verse;
using static MoreReasonableRoomSpace.Extensions;

namespace MoreReasonableRoomSpace
{
    public class MrrsSettings : ModSettings
    {
        public float Rathertight = DefaultRathertight;
        public float AverageSized = DefaultAverageSized;
        public float SomewhatSpacious = DefaultSomewhatSpacious;
        public float QuiteSpacious = DefaultQuiteSpacious;
        public float VerySpacious = DefaultVerySpacious;
        public float ExtremelySpacious = DefaultExtremelySpacious;

        public int PenaltyMinSize = DefaultPenaltyMinSize;
        public float CorrectionFactor = DefaultCorrectionFactor;

        public override void ExposeData()
        {
            Scribe_Values.Look(ref Rathertight, nameof(Rathertight), DefaultRathertight);
            Scribe_Values.Look(ref AverageSized, nameof(AverageSized), DefaultAverageSized);
            Scribe_Values.Look(ref SomewhatSpacious, nameof(SomewhatSpacious), DefaultSomewhatSpacious);
            Scribe_Values.Look(ref QuiteSpacious, nameof(QuiteSpacious), DefaultQuiteSpacious);
            Scribe_Values.Look(ref VerySpacious, nameof(VerySpacious), DefaultVerySpacious);
            Scribe_Values.Look(ref ExtremelySpacious, nameof(ExtremelySpacious), DefaultExtremelySpacious);

            Scribe_Values.Look(ref PenaltyMinSize, nameof(PenaltyMinSize), DefaultPenaltyMinSize);
            Scribe_Values.Look(ref CorrectionFactor, nameof(CorrectionFactor), DefaultCorrectionFactor);
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
            listing.GapLine();

            listing.Label("CorrectionFactorExplain".Translate());
            settings.CorrectionFactor = listing.FloatEntry(nameof(settings.CorrectionFactor).Translate(), settings.CorrectionFactor, 0f);
            listing.GapLine();

            listing.Label("Comment".Translate());
            listing.GapLine();
            bool buttonPressed = listing.ButtonText("Reset".Translate());

            listing.End();
            base.DoSettingsWindowContents(inRect);
            if (buttonPressed)
            {
                settings.Rathertight = DefaultRathertight;
                settings.AverageSized = DefaultAverageSized;
                settings.SomewhatSpacious = DefaultSomewhatSpacious;
                settings.QuiteSpacious = DefaultQuiteSpacious;
                settings.VerySpacious = DefaultVerySpacious;
                settings.ExtremelySpacious = DefaultExtremelySpacious;
                settings.PenaltyMinSize = DefaultPenaltyMinSize;
                settings.CorrectionFactor = DefaultCorrectionFactor;
            }
        }
    }
    public static class Extensions
    {
        public const float DefaultRathertight = 15f;
        public const float DefaultAverageSized = 30f;
        public const float DefaultSomewhatSpacious = 45f;
        public const float DefaultQuiteSpacious = 60f;
        public const float DefaultVerySpacious = 75f;
        public const float DefaultExtremelySpacious = 85f;

        public const int DefaultPenaltyMinSize = 25;
        public const float DefaultCorrectionFactor = 100f;
        public static float FloatEntry(this Listing_Standard ls, string label, float val, float min, float max)
        {
            var buffer = val.ToString("F2");
            ls.TextFieldNumericLabeled(label, ref val, ref buffer, min, max);
            return val;
        }
        public static float FloatEntry(this Listing_Standard ls, string label, float val, float min)
        {
            var buffer = val.ToString("F2");
            ls.TextFieldNumericLabeled(label, ref val, ref buffer, min);
            return val;
        }
    }
}
