// Die ImGui-Oberflaeche, so weit die Plugins sie benutzen.
#nullable enable
using System.Numerics;

namespace Dalamud.Bindings.ImGui
{
    [Flags]
    public enum ImGuiWindowFlags
    {
        None = 0, NoTitleBar = 1, NoResize = 2, NoMove = 4, NoScrollbar = 8,
        NoScrollWithMouse = 16, NoCollapse = 32, AlwaysAutoResize = 64,
        NoBackground = 128, NoSavedSettings = 256, NoMouseInputs = 512,
        NoInputs = 1024, NoDecoration = 2048, NoFocusOnAppearing = 4096,
        NoBringToFrontOnFocus = 8192, AlwaysVerticalScrollbar = 16384,
        NoNavFocus = 32768, NoNav = 65536, NoDocking = 131072,
    }

    [Flags]
    public enum ImGuiTableFlags
    {
        None = 0, Resizable = 1, Reorderable = 2, Hideable = 4, Sortable = 8,
        RowBg = 16, Borders = 32, BordersInner = 64, BordersOuter = 128,
        ScrollY = 256, SizingFixedFit = 512, SizingStretchProp = 1024,
        SizingStretchSame = 2048, SizingFixedSame = 4096, NoSavedSettings = 8192,
    }

    [Flags]
    public enum ImGuiTableColumnFlags
    {
        None = 0, WidthStretch = 1, WidthFixed = 2, NoResize = 4, NoSort = 8,
    }

    [Flags]
    public enum ImDrawFlags
    {
        None = 0, Closed = 1, RoundCornersTopLeft = 16, RoundCornersTopRight = 32,
        RoundCornersBottomLeft = 64, RoundCornersBottomRight = 128,
        RoundCornersTop = 48, RoundCornersBottom = 192,
        RoundCornersLeft = 80, RoundCornersRight = 160, RoundCornersAll = 240,
        RoundCornersNone = 256,
    }

    [Flags]
    public enum ImGuiInputTextFlags
    {
        None = 0, CharsDecimal = 1, CharsNoBlank = 2, AutoSelectAll = 4,
        EnterReturnsTrue = 8, ReadOnly = 16, Password = 32, AllowTabInput = 64,
    }

    [Flags]
    public enum ImGuiColorEditFlags
    {
        None = 0, NoAlpha = 1, NoPicker = 2, NoOptions = 4, NoInputs = 8,
        NoLabel = 16, AlphaBar = 32, AlphaPreview = 64, Float = 128,
    }

    [Flags]
    public enum ImGuiTreeNodeFlags
    {
        None = 0, Selected = 1, Framed = 2, DefaultOpen = 4, Leaf = 8,
    }

    public enum ImGuiCond { None = 0, Always = 1, Once = 2, FirstUseEver = 4, Appearing = 8 }

    public enum ImGuiMouseButton { Left = 0, Right = 1, Middle = 2 }

    public enum ImGuiCol
    {
        Text = 0, TextDisabled = 1, WindowBg = 2, ChildBg = 3, PopupBg = 4,
        Border = 5, FrameBg = 7, FrameBgHovered = 8, FrameBgActive = 9,
        TitleBg = 10, Button = 21, ButtonHovered = 22, ButtonActive = 23,
        Header = 24, HeaderHovered = 25, HeaderActive = 26, Separator = 27,
        TableRowBg = 42, TableRowBgAlt = 43,
    }

    public enum ImGuiStyleVar
    {
        Alpha = 0, WindowPadding = 1, WindowRounding = 2, WindowBorderSize = 3,
        FramePadding = 10, FrameRounding = 11, FrameBorderSize = 12,
        ItemSpacing = 13, ItemInnerSpacing = 14, IndentSpacing = 15,
        CellPadding = 16, ScrollbarSize = 17, GrabMinSize = 19,
    }

    public readonly struct ImFontPtr { }

    public sealed class ImGuiIOPtr
    {
        public float DeltaTime { get; }
        public float Framerate { get; }
        public Vector2 MousePos { get; }
        public Vector2 MouseDelta { get; }
        public Vector2 MouseWheel { get; }
        public Vector2 DisplaySize { get; }
        public bool KeyShift { get; }
        public bool KeyCtrl { get; }
        public bool KeyAlt { get; }
    }

    public readonly struct ImDrawListPtr
    {
        public void AddLine(Vector2 a, Vector2 b, uint colour, float thickness = 1f) { }
        public void AddRect(Vector2 min, Vector2 max, uint colour, float rounding = 0f, ImDrawFlags flags = ImDrawFlags.None, float thickness = 1f) { }
        public void AddRectFilled(Vector2 min, Vector2 max, uint colour, float rounding = 0f, ImDrawFlags flags = ImDrawFlags.None) { }
        public void AddRectFilledMultiColor(Vector2 min, Vector2 max, uint upperLeft, uint upperRight, uint bottomRight, uint bottomLeft) { }
        public void AddCircleFilled(Vector2 centre, float radius, uint colour, int segments = 0) { }
        public void AddCircle(Vector2 centre, float radius, uint colour, int segments = 0, float thickness = 1f) { }
        public void AddTriangleFilled(Vector2 a, Vector2 b, Vector2 c, uint colour) { }
        public void AddText(Vector2 position, uint colour, string text) { }
        public void AddText(ImFontPtr font, float fontSize, Vector2 position, uint colour, string text) { }
        public void AddImage(nint texture, Vector2 min, Vector2 max) { }
        public void AddImage(nint texture, Vector2 min, Vector2 max, Vector2 uvMin, Vector2 uvMax) { }
        public void AddImage(nint texture, Vector2 min, Vector2 max, Vector2 uvMin, Vector2 uvMax, uint colour) { }
        public void PushClipRect(Vector2 min, Vector2 max, bool intersect = false) { }
        public void PopClipRect() { }
    }

    public static class ImGui
    {
        public static bool Begin(string name) => false;
        public static bool Begin(string name, ImGuiWindowFlags flags) => false;
        public static bool Begin(string name, ref bool open, ImGuiWindowFlags flags) => false;
        public static void End() { }
        public static bool BeginChild(string id, Vector2 size) => false;
        public static void EndChild() { }
        public static bool BeginCombo(string label, string preview) => false;
        public static void EndCombo() { }
        public static void BeginDisabled(bool disabled = true) { }
        public static void EndDisabled() { }
        public static void BeginGroup() { }
        public static void EndGroup() { }
        public static bool BeginTable(string id, int columns) => false;
        public static bool BeginTable(string id, int columns, ImGuiTableFlags flags) => false;
        public static bool BeginTable(string id, int columns, ImGuiTableFlags flags, Vector2 outerSize) => false;
        public static void EndTable() { }
        public static bool BeginTooltip() => false;
        public static void EndTooltip() { }
        public static void Bullet() { }
        public static bool Button(string label) => false;
        public static bool Button(string label, Vector2 size) => false;
        public static bool SmallButton(string label) => false;
        public static bool InvisibleButton(string id, Vector2 size) => false;
        public static Vector2 CalcTextSize(string text) => default;
        public static bool Checkbox(string label, ref bool value) => false;
        public static bool CollapsingHeader(string label) => false;
        public static bool CollapsingHeader(string label, ImGuiTreeNodeFlags flags) => false;
        public static uint ColorConvertFloat4ToU32(Vector4 colour) => 0u;
        public static bool ColorEdit4(string label, ref Vector4 colour) => false;
        public static bool ColorEdit4(string label, ref Vector4 colour, ImGuiColorEditFlags flags) => false;
        public static void Dummy(Vector2 size) { }
        public static string GetClipboardText() => string.Empty;
        public static uint GetColorU32(Vector4 colour) => 0u;
        public static uint GetColorU32(ImGuiCol index) => 0u;
        public static Vector2 GetContentRegionAvail() => default;
        public static Vector2 GetCursorPos() => default;
        public static Vector2 GetCursorScreenPos() => default;
        public static ImFontPtr GetFont() => default;
        public static float GetFontSize() => 13f;
        public static ImGuiIOPtr GetIO() => new();
        public static Vector2 GetItemRectMax() => default;
        public static Vector2 GetItemRectMin() => default;
        public static Vector2 GetItemRectSize() => default;
        public static float GetTextLineHeight() => 13f;
        public static float GetTextLineHeightWithSpacing() => 17f;
        public static double GetTime() => 0d;
        public static ImDrawListPtr GetWindowDrawList() => default;
        public static Vector2 GetWindowPos() => default;
        public static Vector2 GetWindowSize() => default;
        public static bool InputInt(string label, ref int value) => false;
        public static bool InputInt(string label, ref int value, int step, int stepFast) => false;
        public static bool InputText(string label, ref string text, uint maxLength) => false;
        public static bool InputText(string label, ref string text, uint maxLength, ImGuiInputTextFlags flags) => false;
        public static bool InputTextMultiline(string label, ref string text, uint maxLength, Vector2 size) => false;
        public static bool InputTextWithHint(string label, string hint, ref string text, uint maxLength) => false;
        public static bool IsItemActive() => false;
        public static bool IsItemClicked() => false;
        public static bool IsItemClicked(ImGuiMouseButton button) => false;
        public static bool IsItemHovered() => false;
        public static bool IsMouseDown(ImGuiMouseButton button) => false;
        public static bool IsMouseDragging(ImGuiMouseButton button) => false;
        public static void NewLine() { }
        public static void PopID() { }
        public static void PopStyleColor(int count = 1) { }
        public static void PopStyleVar(int count = 1) { }
        public static void PushID(string id) { }
        public static void PushID(int id) { }
        public static void PushStyleColor(ImGuiCol index, Vector4 colour) { }
        public static void PushStyleColor(ImGuiCol index, uint colour) { }
        public static void PushStyleVar(ImGuiStyleVar index, float value) { }
        public static void PushStyleVar(ImGuiStyleVar index, Vector2 value) { }
        public static void SameLine(float offsetFromStartX = 0f, float spacing = -1f) { }
        public static bool Selectable(string label) => false;
        public static bool Selectable(string label, bool selected) => false;
        public static void Separator() { }
        public static void SetClipboardText(string text) { }
        public static void SetCursorPos(Vector2 position) { }
        public static void SetCursorScreenPos(Vector2 position) { }
        public static void SetNextItemWidth(float width) { }
        public static void SetNextWindowPos(Vector2 position) { }
        public static void SetNextWindowPos(Vector2 position, ImGuiCond condition) { }
        public static void SetNextWindowSize(Vector2 size) { }
        public static void SetNextWindowSize(Vector2 size, ImGuiCond condition) { }
        public static void SetTooltip(string text) { }
        public static bool SliderFloat(string label, ref float value, float min, float max) => false;
        public static bool SliderFloat(string label, ref float value, float min, float max, string format) => false;
        public static bool SliderInt(string label, ref int value, int min, int max) => false;
        public static bool SliderInt(string label, ref int value, int min, int max, string format) => false;
        public static void Spacing() { }
        public static void TableHeadersRow() { }
        public static bool TableNextColumn() => false;
        public static void TableNextRow() { }
        public static void TableSetupColumn(string label) { }
        public static void TableSetupColumn(string label, ImGuiTableColumnFlags flags) { }
        public static void TableSetupColumn(string label, ImGuiTableColumnFlags flags, float width) { }
        public static void TableSetupScrollFreeze(int columns, int rows) { }
        public static void Text(string text) { }
        public static void TextColored(Vector4 colour, string text) { }
        public static void TextUnformatted(string text) { }
        public static void TextWrapped(string text) { }
    }
}

namespace Dalamud.Interface.Windowing
{
    using Dalamud.Bindings.ImGui;

    public sealed class WindowSizeConstraints
    {
        public Vector2 MinimumSize { get; set; }
        public Vector2 MaximumSize { get; set; }
    }

    public abstract class Window
    {
        protected Window(string name) => this.WindowName = name;
        protected Window(string name, ImGuiWindowFlags flags) => this.WindowName = name;
        protected Window(string name, ImGuiWindowFlags flags, bool forceMainWindow) => this.WindowName = name;

        public string WindowName { get; set; }
        public bool IsOpen { get; set; }
        public Vector2? Size { get; set; }
        public Vector2? Position { get; set; }
        public ImGuiCond SizeCondition { get; set; }
        public ImGuiCond PositionCondition { get; set; }
        public WindowSizeConstraints? SizeConstraints { get; set; }
        public ImGuiWindowFlags Flags { get; set; }
        public bool RespectCloseHotkey { get; set; }
        public bool DisableWindowSounds { get; set; }
        public bool ForceMainWindow { get; set; }
        public bool AllowClickthrough { get; set; }
        public bool ShowCloseButton { get; set; }

        public abstract void Draw();
        public virtual void PreDraw() { }
        public virtual void PostDraw() { }
        public virtual bool DrawConditions() => true;
        public virtual void OnOpen() { }
        public virtual void OnClose() { }
        public virtual void Update() { }
        public void Toggle() { }
    }

    public sealed class WindowSystem
    {
        public WindowSystem(string name) { }
        public IReadOnlyList<Window> Windows => Array.Empty<Window>();
        public void AddWindow(Window window) { }
        public void RemoveWindow(Window window) { }
        public void RemoveAllWindows() { }
        public void Draw() { }
    }
}
