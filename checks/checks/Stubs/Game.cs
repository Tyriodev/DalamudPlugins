// Spielobjekte, Gruppe, Statuseffekte.
#nullable enable
using System.Numerics;
using Lumina.Excel;
using Lumina.Excel.Sheets;
using Lumina.Text.ReadOnly;

namespace Dalamud.Game.ClientState.Objects.Enums
{
    public enum ObjectKind : byte
    {
        None = 0,
        Player = 1,
        BattleNpc = 2,
        EventNpc = 3,
        Treasure = 4,
        Companion = 9,
    }

    [Flags]
    public enum StatusFlags : byte
    {
        None = 0,
        Hostile = 1,
        InCombat = 2,
        WeaponOut = 4,
        PartyMember = 16,
        AllianceMember = 32,
        Friend = 64,
    }
}

namespace Dalamud.Game.ClientState.Statuses
{
    public interface IStatus
    {
        uint StatusId { get; }
        float RemainingTime { get; }
        ushort Param { get; }
        uint SourceId { get; }
        Dalamud.Game.ClientState.Objects.Types.IGameObject? SourceObject { get; }
        Status? GameData { get; }
        nint Address { get; }
    }

    public interface IStatusList : IReadOnlyCollection<IStatus>
    {
        int Length { get; }
        IStatus? this[int index] { get; }
    }
}

namespace Dalamud.Game.ClientState.Objects.Types
{
    using Dalamud.Game.ClientState.Objects.Enums;
    using Dalamud.Game.ClientState.Statuses;

    public interface IGameObject : IEquatable<IGameObject>
    {
        nint Address { get; }
        uint EntityId { get; }
        ulong GameObjectId { get; }
        ReadOnlySeString Name { get; }
        ObjectKind ObjectKind { get; }
        Vector3 Position { get; }
        float Rotation { get; }
        bool IsValid();
        bool IsDead { get; }
        bool IsTargetable { get; }
        uint OwnerId { get; }
        byte SubKind { get; }
        float YalmDistanceX { get; }
        IGameObject? TargetObject { get; }
    }

    public interface ICharacter : IGameObject
    {
        uint CurrentHp { get; }
        uint MaxHp { get; }
        uint CurrentMp { get; }
        uint MaxMp { get; }
        RowRef<ClassJob> ClassJob { get; }
        byte Level { get; }
        StatusFlags StatusFlags { get; }
        byte ShieldPercentage { get; }
        IStatusList StatusList { get; }
    }

    public interface IBattleChara : ICharacter
    {
        bool IsCasting { get; }
        uint CastActionId { get; }
        float CurrentCastTime { get; }
        float TotalCastTime { get; }
        bool IsCastInterruptible { get; }
        ulong TargetObjectId { get; }
    }

    public interface IBattleNpc : IBattleChara { }
}

namespace Dalamud.Game.ClientState.Objects.SubKinds
{
    using Dalamud.Game.ClientState.Objects.Types;

    public interface IPlayerCharacter : IBattleChara
    {
        RowRef<World> CurrentWorld { get; }
        RowRef<World> HomeWorld { get; }
    }
}

namespace Dalamud.Game.ClientState.Party
{
    using Dalamud.Game.ClientState.Objects.Types;
    using Dalamud.Game.ClientState.Statuses;

    public interface IPartyMember
    {
        nint Address { get; }
        uint ObjectId { get; }
        IGameObject? GameObject { get; }
        ReadOnlySeString Name { get; }
        uint CurrentHP { get; }
        uint MaxHP { get; }
        uint CurrentMP { get; }
        uint MaxMP { get; }
        byte Level { get; }
        RowRef<ClassJob> ClassJob { get; }
        IStatusList Statuses { get; }
    }
}
