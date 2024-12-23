using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: DisableRuntimeMarshalling]

namespace RenderFun;

/// <summary>
/// Low level Clay interop. Redefine types and function signatures, and recreate macros as C# functions
/// </summary>
internal static partial class Interop
{
    private const string LibName = "clay";
    private const string Prefix = "Clay_";
    
    #region Public API Surface
    
    [LibraryImport(LibName, EntryPoint = Prefix + nameof(MinMemorySize))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial uint MinMemorySize();

    [LibraryImport(LibName, EntryPoint = Prefix + nameof(CreateArenaWithCapacityAndMemory))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static unsafe partial Arena CreateArenaWithCapacityAndMemory(uint capacity, byte* offset);
    
    [LibraryImport(LibName, EntryPoint = Prefix + nameof(SetPointerState))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void SetPointerState(Vector2 position, [MarshalAs(UnmanagedType.I1)] bool pointerDown);

    [LibraryImport(LibName, EntryPoint = Prefix + nameof(Initialize))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void Initialize(Arena arena, Dimensions layoutDimensions);
    
    [LibraryImport(LibName, EntryPoint = Prefix + nameof(UpdateScrollContainers))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void UpdateScrollContainers([MarshalAs(UnmanagedType.I1)] bool enableDragScrolling, Vector2 scrollDelta, float deltaTime);
    
    [LibraryImport(LibName, EntryPoint = Prefix + nameof(SetLayoutDimensions))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void SetLayoutDimensions(Dimensions dimensions);

    [LibraryImport(LibName, EntryPoint = Prefix + nameof(BeginLayout))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void BeginLayout();

    [LibraryImport(LibName, EntryPoint = Prefix + nameof(EndLayout))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial Array<RenderCommand> EndLayout();
    
    [LibraryImport(LibName, EntryPoint = Prefix + nameof(PointerOver))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool PointerOver(ElementId id);
    
    [LibraryImport(LibName, EntryPoint = Prefix + nameof(GetElementId))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial ElementId GetElementId(String id);
    
    [LibraryImport(LibName, EntryPoint = Prefix + nameof(GetScrollContainerData))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial ScrollContainerData GetScrollContainerData(ElementId id);

    public unsafe delegate Dimensions MeasureTextFunctionDelegate(String* text, TextElementConfig* config);
    
    [LibraryImport(LibName, EntryPoint = Prefix + nameof(SetMeasureTextFunction))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void SetMeasureTextFunction(MeasureTextFunctionDelegate measureTextFunction);

    [LibraryImport(LibName, EntryPoint = Prefix + nameof(RenderCommandArray_Get))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static unsafe partial RenderCommand* RenderCommandArray_Get(Array<RenderCommand>* array, int index);
    
    [LibraryImport(LibName, EntryPoint = Prefix + nameof(SetDebugModeEnabled))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void SetDebugModeEnabled([MarshalAs(UnmanagedType.I1)] bool enabled);
    
    #endregion
    
    #region Private API Surface

    [LibraryImport(LibName, EntryPoint = Prefix + nameof(_OpenElement))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void _OpenElement();

    [LibraryImport(LibName, EntryPoint = Prefix + nameof(_CloseElement))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void _CloseElement();

    [LibraryImport(LibName, EntryPoint = Prefix + nameof(_ElementPostConfiguration))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void _ElementPostConfiguration();

    [LibraryImport(LibName, EntryPoint = Prefix + nameof(_OpenTextElement))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static unsafe partial void _OpenTextElement(String text, TextElementConfig* textConfig);
    
    [LibraryImport(LibName, EntryPoint = Prefix + nameof(_AttachId))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void _AttachId(ElementId id);
    
    [LibraryImport(LibName, EntryPoint = Prefix + nameof(_AttachLayoutConfig))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static unsafe partial void _AttachLayoutConfig(LayoutConfig* layoutConfig);
    
    [LibraryImport(LibName, EntryPoint = Prefix + nameof(_AttachElementConfig))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static unsafe partial void _AttachElementConfig(void* config, _ElementConfigType type);
    
    [LibraryImport(LibName, EntryPoint = Prefix + nameof(_StoreLayoutConfig))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static unsafe partial LayoutConfig* _StoreLayoutConfig(LayoutConfig config);
    
    [LibraryImport(LibName, EntryPoint = Prefix + nameof(_StoreRectangleElementConfig))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static unsafe partial RectangleElementConfig* _StoreRectangleElementConfig(RectangleElementConfig config);
    
    [LibraryImport(LibName, EntryPoint = Prefix + nameof(_StoreTextElementConfig))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static unsafe partial TextElementConfig* _StoreTextElementConfig(TextElementConfig config);
    
    [LibraryImport(LibName, EntryPoint = Prefix + nameof(_StoreImageElementConfig))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static unsafe partial ImageElementConfig* _StoreImageElementConfig(ImageElementConfig config);
    
    [LibraryImport(LibName, EntryPoint = Prefix + nameof(_StoreFloatingElementConfig))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static unsafe partial FloatingElementConfig* _StoreFloatingElementConfig(FloatingElementConfig config);
    
    [LibraryImport(LibName, EntryPoint = Prefix + nameof(_StoreCustomElementConfig))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static unsafe partial CustomElementConfig* _StoreCustomElementConfig(CustomElementConfig config);
    
    [LibraryImport(LibName, EntryPoint = Prefix + nameof(_StoreScrollElementConfig))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static unsafe partial ScrollElementConfig* _StoreScrollElementConfig(ScrollElementConfig config);
    
    [LibraryImport(LibName, EntryPoint = Prefix + nameof(_StoreBorderElementConfig))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static unsafe partial BorderElementConfig* _StoreBorderElementConfig(BorderElementConfig config);
    
    [LibraryImport(LibName, EntryPoint = Prefix + nameof(_HashString))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial ElementId _HashString(String toHash, uint index, uint seed);
    
    [LibraryImport(LibName, EntryPoint = Prefix + nameof(_GetOpenLayoutElementId))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial uint _GetOpenLayoutElementId();
    
    #endregion

    #region Data Types
    
    [StructLayout(LayoutKind.Sequential)]
    public struct String
    {
        public int length;
        public unsafe void* chars;
    }
    
    [StructLayout(LayoutKind.Sequential)]
    public struct Array<T> where T : unmanaged
    {
        public uint capacity;
        public uint length;
        public unsafe T* internalArray;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Arena
    {
        public String label;
        public ulong nextAllocation;
        public ulong capacity;
        public unsafe void* memory;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ElementId
    {
        public uint id;
        public uint offset;
        public uint baseId;
        public String stringId;
    }

    [Flags]
    public enum _ElementConfigType : byte
    {
        Rectangle = 1,
        BorderContainer = 2,
        FloatingContainer = 4,
        ScrollContainer = 8,
        Image = 16,
        Text = 32,
        Custom = 64,
    }

    public enum LayoutDirection : byte
    {
        LeftToRight,
        TopToBottom,
    }

    public enum LayoutAlignmentX : byte
    {
        Left,
        Right,
        Center,
    }

    public enum LayoutAlignmentY : byte
    {
        Top,
        Bottom,
        Center,
    }

    public enum SizingType : byte
    {
        Fit,
        Grow,
        Percent,
        Fixed,
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ChildAlignment
    {
        public LayoutAlignmentX x;
        public LayoutAlignmentY y;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SizingMinMax
    {
        public float min;
        public float max;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct SizingAxis
    {
        [FieldOffset(0)] public SizingMinMax sizeMinMax;
        [FieldOffset(0)] public float sizePercent;
        [FieldOffset(8)] public SizingType type;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Sizing
    {
        public SizingAxis width;
        public SizingAxis height;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Padding
    {
        public ushort x;
        public ushort y;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct LayoutConfig
    {
        public Sizing sizing;
        public Padding padding;
        public ushort childGap;
        public ChildAlignment childAlignment;
        public LayoutDirection layoutDirection;
    }

    public enum TextElementConfigWrapMode
    {
        Words,
        Newlines,
        None,
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct TextElementConfig
    {
        public Color textColor;
        public ushort fontId;
        public ushort fontSize;
        public ushort letterSpacing;
        public ushort lineHeight;
        public TextElementConfigWrapMode wrapMode;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ImageElementConfig
    {
        public unsafe void* imageData;
        public Dimensions sourceDimensions;
    }

    public enum FloatingAttachPointType : byte
    {
        LeftTop,
        LeftCenter,
        LeftBottom,
        CenterTop,
        CenterCenter,
        CenterBottom,
        RightTop,
        RightCenter,
        RightBottom,
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct FloatingAttachPoints
    {
        public FloatingAttachPointType element;
        public FloatingAttachPointType parent;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct FloatingElementConfig
    {
        public Vector2 offset;
        public Dimensions expand;
        public ushort zIndex;
        public uint parentId;
        public FloatingAttachPoints attachment;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct CustomElementConfig
    {
        public unsafe void* customData;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ScrollElementConfig
    {
        public bool horizontal;
        public bool vertical;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Border
    {
        public uint width;
        public Color color;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BorderElementConfig
    {
        public Border left;
        public Border right;
        public Border top;
        public Border bottom;
        public Border betweenChildren;
        public CornerRadius cornerRadius;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct ElementConfigUnion
    {
        [FieldOffset(0)] public unsafe RectangleElementConfig* rectangleElementConfig;
        [FieldOffset(0)] public unsafe TextElementConfig* textElementConfig;
        [FieldOffset(0)] public unsafe ImageElementConfig* imageElementConfig;
        [FieldOffset(0)] public unsafe FloatingElementConfig* floatingElementConfig;
        [FieldOffset(0)] public unsafe CustomElementConfig* customElementConfig;
        [FieldOffset(0)] public unsafe ScrollElementConfig* scrollElementConfig;
        [FieldOffset(0)] public unsafe BorderElementConfig* borderElementConfig;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ScrollContainerData
    {
        public unsafe Vector2* scrollPosition;
        public Dimensions scrollContainerDimensions;
        public Dimensions contentDimensions;
        public ScrollElementConfig config;
        [MarshalAs(UnmanagedType.I1)]
        public bool found;
    }

    public enum RenderCommandType
    {
        None,
        Rectangle,
        Border,
        Text,
        Image,
        ScissorStart,
        ScissorEnd,
        Custom,
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RenderCommand
    {
        public BoundingBox boundingBox;
        public ElementConfigUnion config;
        public String text;
        public uint id;
        public RenderCommandType commandType;
    }

    // TODO: Not part of clay.h, remove probably
    public enum TypedConfigType
    {
        Rectangle = 1,
        Border = 2,
        Floating = 4,
        Scroll = 8,
        Image = 16,
        Text = 32,
        Custom = 64,
        // Interop, from Odin
        Id = 65,
        Layout = 66,
    }

    // TODO: Not part of clay.h, remove probably
    [StructLayout(LayoutKind.Sequential)]
    public struct TypedConfig
    {
        public TypedConfigType type;
        public unsafe void* config;
        public ElementId id;
    }
    
    #endregion
}