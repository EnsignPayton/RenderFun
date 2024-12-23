using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

[assembly: DisableRuntimeMarshalling]

namespace RenderFun;

/// <summary>
/// Low level Clay interop
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
        public int Length;
        public unsafe void* Chars;

        public readonly unsafe ReadOnlySpan<byte> AsSpan() => new(Chars, Length);
        public override string ToString() => Encoding.ASCII.GetString(AsSpan());
    }
    
    [StructLayout(LayoutKind.Sequential)]
    public struct Array<T> where T : unmanaged
    {
        public uint Capacity;
        public uint Length;
        public unsafe T* InternalArray;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Arena
    {
        public String Label;
        public ulong NextAllocation;
        public ulong Capacity;
        public unsafe void* Memory;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ElementId
    {
        public uint Id;
        public uint Offset;
        public uint BaseId;
        public String StringId;
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

    [StructLayout(LayoutKind.Explicit)]
    public struct ElementConfigUnion
    {
        [FieldOffset(0)] public unsafe RectangleElementConfig* RectangleElementConfig;
        [FieldOffset(0)] public unsafe TextElementConfig* TextElementConfig;
        [FieldOffset(0)] public unsafe ImageElementConfig* ImageElementConfig;
        [FieldOffset(0)] public unsafe FloatingElementConfig* FloatingElementConfig;
        [FieldOffset(0)] public unsafe CustomElementConfig* CustomElementConfig;
        [FieldOffset(0)] public unsafe ScrollElementConfig* ScrollElementConfig;
        [FieldOffset(0)] public unsafe BorderElementConfig* BorderElementConfig;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ScrollContainerData
    {
        public unsafe Vector2* ScrollPosition;
        public Dimensions ScrollContainerDimensions;
        public Dimensions ContentDimensions;
        public ScrollElementConfig Config;
        [MarshalAs(UnmanagedType.I1)]
        public bool Found;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RenderCommand
    {
        public BoundingBox BoundingBox;
        public ElementConfigUnion Config;
        public String Text;
        public uint Id;
        public RenderCommandType CommandType;
    }
    
    #endregion
}