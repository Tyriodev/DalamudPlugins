// Erzeugt aus der tatsaechlichen Nutzung in Glacier/Hud/JobGauge.cs.
// Zahlen stehen als int: das vertraegt sich mit jedem Gebrauch im Plugin, auch mit
// (int)-Umwandlung und Division durch 1000f. Wahrheitswerte stehen da, wo im Plugin
// eine Bedingung daraus gebildet wird.
#nullable enable

namespace Dalamud.Game.ClientState.JobGauge.Types
{
    public abstract class JobGaugeBase { }

    public sealed class ASTGauge : JobGaugeBase
    {
        public int Card1 { get; }
        public int Card2 { get; }
        public int Card3 { get; }
    }

    public sealed class BLMGauge : JobGaugeBase
    {
        public int AstralFireStacks { get; }
        public int EnochianTimer { get; }
        public bool InAstralFire { get; }
        public int PolyglotStacks { get; }
        public int UmbralHearts { get; }
        public int UmbralIceStacks { get; }
    }

    public sealed class BRDGauge : JobGaugeBase
    {
        public int Repertoire { get; }
        public int SongTimer { get; }
        public int SoulVoice { get; }
    }

    public sealed class DNCGauge : JobGaugeBase
    {
        public int Esprit { get; }
        public int Feathers { get; }
    }

    public sealed class DRGGauge : JobGaugeBase
    {
        public int FirstmindsFocusCount { get; }
        public bool IsLOTDActive { get; }
        public int LOTDTimer { get; }
    }

    public sealed class DRKGauge : JobGaugeBase
    {
        public int Blood { get; }
        public int DarksideTimeRemaining { get; }
    }

    public sealed class GNBGauge : JobGaugeBase
    {
        public int Ammo { get; }
    }

    public sealed class MCHGauge : JobGaugeBase
    {
        public int Battery { get; }
        public int Heat { get; }
        public bool IsOverheated { get; }
        public int OverheatTimeRemaining { get; }
        public int SummonTimeRemaining { get; }
    }

    public sealed class MNKGauge : JobGaugeBase
    {
        public int BeastChakra1 { get; }
        public int BeastChakra2 { get; }
        public int BeastChakra3 { get; }
        public int BlitzTimeRemaining { get; }
        public int Chakra { get; }
    }

    public sealed class NINGauge : JobGaugeBase
    {
        public int Kazematoi { get; }
        public int Ninki { get; }
    }

    public sealed class PCTGauge : JobGaugeBase
    {
        public int Paint { get; }
        public int PalleteGauge { get; }
    }

    public sealed class PLDGauge : JobGaugeBase
    {
        public int OathGauge { get; }
    }

    public sealed class RDMGauge : JobGaugeBase
    {
        public int BlackMana { get; }
        public int ManaStacks { get; }
        public int WhiteMana { get; }
    }

    public sealed class RPRGauge : JobGaugeBase
    {
        public int EnshroudedTimeRemaining { get; }
        public int Shroud { get; }
        public int Soul { get; }
    }

    public sealed class SAMGauge : JobGaugeBase
    {
        public bool HasGetsu { get; }
        public bool HasKa { get; }
        public bool HasSetsu { get; }
        public int Kenki { get; }
        public int MeditationStacks { get; }
    }

    public sealed class SCHGauge : JobGaugeBase
    {
        public int Aetherflow { get; }
        public int FairyGauge { get; }
    }

    public sealed class SGEGauge : JobGaugeBase
    {
        public int Addersgall { get; }
        public int Addersting { get; }
    }

    public sealed class SMNGauge : JobGaugeBase
    {
        public int AttunementCount { get; }
        public int SummonTimerRemaining { get; }
    }

    public sealed class VPRGauge : JobGaugeBase
    {
        public int RattlingCoilStacks { get; }
        public int SerpentOffering { get; }
    }

    public sealed class WARGauge : JobGaugeBase
    {
        public int BeastGauge { get; }
    }

    public sealed class WHMGauge : JobGaugeBase
    {
        public int BloodLily { get; }
        public int Lily { get; }
    }

}