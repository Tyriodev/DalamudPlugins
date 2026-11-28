// Attrappen der Dalamud-Oberflaeche. Kein echtes Dalamud, nur gerade so viel, dass der
// C#-Compiler die Plugin-Quellen ohne das Spiel pruefen kann.
#nullable enable
using System.Numerics;
using Dalamud.Game.ClientState.JobGauge.Types;
using Dalamud.Game.ClientState.Objects.Types;
using Lumina.Excel;

namespace Dalamud.IoC
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class PluginServiceAttribute : Attribute { }
}

namespace Dalamud.Configuration
{
    public interface IPluginConfiguration { int Version { get; set; } }
}

namespace Dalamud.Plugin
{
    using Dalamud.Configuration;

    public interface IDalamudPlugin : IDisposable { }

    public sealed class UiBuilder
    {
        public event Action? Draw;
        public event Action? OpenMainUi;
        public event Action? OpenConfigUi;
    }

    public interface IDalamudPluginInterface
    {
        UiBuilder UiBuilder { get; }
        DirectoryInfo ConfigDirectory { get; }
        IPluginConfiguration? GetPluginConfig();
        void SavePluginConfig(IPluginConfiguration? config);
    }
}

namespace Dalamud.Game.Command
{
    public delegate void CommandInfoHandler(string command, string arguments);

    public sealed class CommandInfo
    {
        public CommandInfo(CommandInfoHandler handler) { }
        public CommandInfoHandler Handler => default!;
        public string HelpMessage { get; set; } = string.Empty;
        public bool ShowInHelp { get; set; } = true;
    }
}

namespace Dalamud.Game.ClientState.Conditions
{
    public enum ConditionFlag
    {
        None = 0,
        BoundByDuty = 34,
        BoundByDuty56 = 56,
        BoundByDuty95 = 95,
        InCombat = 26,
        InDutyQueue = 41,
        WaitingForDuty = 42,
        WaitingForDutyFinder = 43,
        Crafting = 5,
        Gathering = 6,
        Occupied = 1,
        WatchingCutscene = 35,
        BetweenAreas = 45,
        Mounted = 4,
        RidingPillion = 75,
        Fishing = 43,
        PreparingToCraft = 40,
        ExecutingCraftingAction = 41,
        ExecutingGatheringAction = 42,
        Unconscious = 2,
        OccupiedInCutSceneEvent = 32,
        WatchingCutscene78 = 78,
    }
}

namespace Dalamud.Game.Inventory
{
    public enum GameInventoryType : uint
    {
        Inventory1 = 0,
        Inventory2 = 1,
        Inventory3 = 2,
        Inventory4 = 3,
        Currency = 2000,
        Crystals = 2001,
        KeyItems = 2004,
    }

    public readonly struct GameInventoryItem
    {
        public bool IsEmpty => false;
        public GameInventoryType ContainerType => default;
        public uint InventorySlot => default;
        public uint ItemId => default;
        public uint BaseItemId => default;
        public int Quantity => default;
        public bool IsHq => false;
        public bool IsCollectable => false;
        public bool IsRelic => false;
        public uint Spiritbond => default;
        public uint Condition => default;
    }
}

namespace Dalamud.Interface.Textures.TextureWraps
{
    public interface IDalamudTextureWrap : IDisposable
    {
        nint Handle { get; }
        int Width { get; }
        int Height { get; }
        Vector2 Size { get; }
    }
}

namespace Dalamud.Interface.Utility
{
    public static class ImGuiHelpers
    {
        public static float GlobalScale => 1f;
        public static Vector2 ScaledVector2(float x, float y) => new(x, y);
    }
}

namespace Dalamud.Plugin.Services
{
    using Dalamud.Game.ClientState.Conditions;
    using Dalamud.Game.ClientState.Objects.SubKinds;
    using Dalamud.Game.ClientState.Party;
    using Dalamud.Game.Command;
    using Dalamud.Game.Inventory;
    using Dalamud.Interface.Textures;
    using Dalamud.Interface.Textures.TextureWraps;

    public interface IPluginLog
    {
        void Information(string message);
        void Information(Exception exception, string message);
        void Warning(string message);
        void Warning(Exception exception, string message);
        void Error(string message);
        void Error(Exception exception, string message);
        void Debug(string message);
        void Debug(Exception exception, string message);
        void Verbose(string message);
        void Verbose(Exception exception, string message);
    }

    public interface IChatGui
    {
        void Print(string message);
        void PrintError(string message);
    }

    public interface IClientState
    {
        bool IsLoggedIn { get; }
        bool IsPvP { get; }
        bool IsGPosing { get; }
        ushort TerritoryType { get; }
        IPlayerCharacter? LocalPlayer { get; }
    }

    public interface ICommandManager
    {
        bool AddHandler(string command, CommandInfo info);
        bool RemoveHandler(string command);
    }

    public interface ICondition { bool this[ConditionFlag flag] { get; } }

    public interface IFramework
    {
        event Action<IFramework>? Update;
        TimeSpan UpdateDelta { get; }
        DateTime LastUpdate { get; }
    }

    public interface IDataManager
    {
        ExcelSheet<T>? GetExcelSheet<T>() where T : struct, IExcelRow<T>;
        GameDataStub GameData { get; }
    }

    /// <summary>Platzhalter fuer Lumina.GameData; die Plugins greifen nur lesend zu.</summary>
    public sealed class GameDataStub
    {
        public ExcelSheet<T>? GetExcelSheet<T>() where T : struct, IExcelRow<T> => null;
        public object? GetFile(string path) => null;
        public IReadOnlyDictionary<string, RepositoryStub> Repositories => new Dictionary<string, RepositoryStub>();
    }

    public sealed class RepositoryStub
    {
        public string? Version => null;
        public string? Name => null;
    }

    public interface IGameInventory
    {
        ReadOnlySpan<GameInventoryItem> GetInventoryItems(GameInventoryType type);
    }

    public interface IJobGauges
    {
        T Get<T>() where T : JobGaugeBase;
    }

    public interface IObjectTable : IReadOnlyList<IGameObject?>
    {
        IPlayerCharacter? LocalPlayer { get; }
        IGameObject? SearchById(ulong entityId);
    }

    public interface ITargetManager
    {
        IGameObject? Target { get; set; }
        IGameObject? FocusTarget { get; set; }
        IGameObject? SoftTarget { get; set; }
        IGameObject? MouseOverTarget { get; set; }
    }

    public interface ITextureProvider
    {
        ISharedImmediateTexture GetFromGameIcon(in GameIconLookup lookup);
        ISharedImmediateTexture GetFromGameIcon(uint iconId);
        ISharedImmediateTexture GetFromFile(string path);
    }

    /// <summary>Spielstand des eigenen Charakters.</summary>
    public interface IPlayerState
    {
        bool IsLoaded { get; }
        byte Level { get; }
        RowRef<Lumina.Excel.Sheets.ClassJob> ClassJob { get; }
        RowRef<Lumina.Excel.Sheets.World> HomeWorld { get; }
        short GetClassJobLevel(Lumina.Excel.Sheets.ClassJob classJob);
        short GetClassJobLevel(uint classJobId);
        int GetClassJobExperience(Lumina.Excel.Sheets.ClassJob classJob);
        int GetClassJobExperience(uint classJobId);
    }

    public interface IPartyList : IReadOnlyCollection<IPartyMember>
    {
        int Length { get; }
        IPartyMember? this[int index] { get; }
        bool IsAlliance { get; }
        nint GetAllianceMemberAddress(int index);
        IPartyMember? CreateAllianceMemberReference(nint address);
    }
}

namespace Dalamud.Interface.Textures
{
    using Dalamud.Interface.Textures.TextureWraps;

    public readonly struct GameIconLookup
    {
        public GameIconLookup(uint iconId) { }
        public uint IconId => default;
    }

    public interface ISharedImmediateTexture
    {
        IDalamudTextureWrap? GetWrapOrDefault();
        IDalamudTextureWrap GetWrapOrEmpty();
        bool TryGetWrap(out IDalamudTextureWrap? wrap, out Exception? exception);
    }
}


namespace Dalamud.Utility
{
    public static class Util
    {
        public static void OpenLink(string url) { }
        public static string ToStringSafe(this object? value) => value?.ToString() ?? string.Empty;
    }
}
