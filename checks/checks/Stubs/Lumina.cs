// Die Spieltabellen, so weit sie benutzt werden.
#nullable enable

namespace Lumina.Text.ReadOnly
{
    public readonly struct ReadOnlySeString
    {
        public string TextValue => string.Empty;
        public string ExtractText() => string.Empty;
        public override string ToString() => string.Empty;
    }
}

namespace Lumina.Excel
{
    public interface IExcelRow<T> where T : struct { }

    public sealed class ExcelSheet<T> : IEnumerable<T> where T : struct, IExcelRow<T>
    {
        public int Count => 0;
        public T? GetRowOrDefault(uint rowId) => null;
        public T GetRow(uint rowId) => default;
        public bool TryGetRow(uint rowId, out T row) { row = default; return false; }
        public IEnumerator<T> GetEnumerator() => Enumerable.Empty<T>().GetEnumerator();
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => this.GetEnumerator();
    }

    public readonly struct RowRef<T> where T : struct, IExcelRow<T>
    {
        public uint RowId => default;
        public T? ValueNullable => null;
        public T Value => default;
        public bool IsValid => false;
    }
}

namespace Lumina.Excel.Sheets
{
    using Lumina.Excel;
    using Lumina.Text.ReadOnly;

    public readonly struct TerritoryType : IExcelRow<TerritoryType>
    {
        public uint RowId => default;
        public ReadOnlySeString Name => default;
        public ReadOnlySeString Bg => default;
        public RowRef<PlaceName> PlaceName => default;
        public RowRef<ContentFinderCondition> ContentFinderCondition => default;
    }

    public readonly struct PlaceName : IExcelRow<PlaceName>
    {
        public uint RowId => default;
        public ReadOnlySeString Name => default;
    }

    public readonly struct ContentFinderCondition : IExcelRow<ContentFinderCondition>
    {
        public uint RowId => default;
        public ReadOnlySeString Name => default;
        public RowRef<ContentType> ContentType => default;
        public RowRef<TerritoryType> TerritoryType => default;
        public byte ClassJobLevelRequired => default;
        public byte ClassJobLevelSync => default;
        public bool HighEndDuty => false;
    }

    public readonly struct ContentType : IExcelRow<ContentType>
    {
        public uint RowId => default;
        public ReadOnlySeString Name => default;
        public uint Icon => default;
    }

    public readonly struct Item : IExcelRow<Item>
    {
        public uint RowId => default;
        public ReadOnlySeString Name => default;
        public ushort Icon => default;
        public uint PriceLow => default;
        public uint PriceMid => default;
        public bool CanBeHq => false;
        public RowRef<ItemLevel> LevelItem => default;
        public byte LevelEquip => default;
        public byte Rarity => default;
        public bool IsUnique => false;
        public bool IsUntradable => false;
        public bool IsIndisposable => false;
        public byte Desynth => default;
        public RowRef<ItemSearchCategory> ItemSearchCategory => default;
        public RowRef<ItemUICategory> ItemUICategory => default;
        public RowRef<EquipSlotCategory> EquipSlotCategory => default;
    }

    public readonly struct ItemLevel : IExcelRow<ItemLevel>
    {
        public uint RowId => default;
    }

    public readonly struct ItemSearchCategory : IExcelRow<ItemSearchCategory>
    {
        public uint RowId => default;
        public ReadOnlySeString Name => default;
    }

    public readonly struct ItemUICategory : IExcelRow<ItemUICategory>
    {
        public uint RowId => default;
        public ReadOnlySeString Name => default;
    }

    public readonly struct EquipSlotCategory : IExcelRow<EquipSlotCategory>
    {
        public uint RowId => default;
    }

    public readonly struct Recipe : IExcelRow<Recipe>
    {
        public uint RowId => default;
        public RowRef<Item> ItemResult => default;
        public RowRef<RecipeLevelTable> RecipeLevelTable => default;
        public ushort AmountResult => default;
        public IReadOnlyList<RowRef<Item>> Ingredient => Array.Empty<RowRef<Item>>();
    }

    public readonly struct ClassJob : IExcelRow<ClassJob>
    {
        public uint RowId => default;
        public ReadOnlySeString Name => default;
        public ReadOnlySeString Abbreviation => default;
        public byte Role => default;
    }

    public readonly struct World : IExcelRow<World>
    {
        public uint RowId => default;
        public ReadOnlySeString Name => default;
        public bool IsPublic => false;
        public RowRef<WorldDCGroupType> DataCenter => default;
    }

    public readonly struct WorldDCGroupType : IExcelRow<WorldDCGroupType>
    {
        public uint RowId => default;
        public ReadOnlySeString Name => default;
    }

    public readonly struct Status : IExcelRow<Status>
    {
        public uint RowId => default;
        public ReadOnlySeString Name => default;
        public ReadOnlySeString Description => default;
        public ushort Icon => default;
        public bool CanDispel => false;
        public byte StatusCategory => default;
    }

    public readonly struct Action : IExcelRow<Action>
    {
        public uint RowId => default;
        public ReadOnlySeString Name => default;
        public ushort Icon => default;
        public byte ClassJobLevel => default;
        public bool IsRoleAction => false;
    }

    public readonly struct RecipeLevelTable : IExcelRow<RecipeLevelTable>
    {
        public uint RowId => default;
        public ushort ClassJobLevel => default;
    }

    public readonly struct ParamGrow : IExcelRow<ParamGrow>
    {
        public uint RowId => default;
        public int ExpToNext => default;
    }
}
