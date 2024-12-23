using System.Runtime.InteropServices;

namespace RenderFun;

public static class Clay
{
    public static IDisposable Initialize(Dimensions dimensions) => new ClayContext(dimensions);
    
    public static LayoutBuilder UI() => new();

    public static void BeginLayout() => Interop.BeginLayout();

    public static unsafe ReadOnlySpan<RenderCommand> EndLayout()
    {
        var renderCommands = Interop.EndLayout();
        return new Span<RenderCommand>(
            renderCommands.InternalArray,
            (int)renderCommands.Length);
    }

    public static SizingAxis SizingFixed(float min, float max = 0) => new()
    {
        Type = SizingType.Fixed,
        SizeMinMax = new() { Min = min, Max = max }
    };

    public static SizingAxis SizingGrow() => new()
    {
        Type = SizingType.Grow
    };
}

// Public API Types

public readonly record struct Dimensions(
    float Width,
    float Height);

public readonly record struct Vector2(
    float X,
    float Y);

public readonly record struct Color(
    float R,
    float G,
    float B,
    float A);

public readonly record struct BoundingBox(
    float X,
    float Y,
    float Width,
    float Height);

// ElementId

public readonly record struct CornerRadius(
    float TopLeft,
    float TopRight,
    float BottomLeft,
    float BottomRight)
{
    public CornerRadius(float value) : this(value, value, value, value)
    {
    }
}

public enum LayoutDirection : byte
{
    LeftRoRight,
    TopToBottom
}

public enum LayoutAlignmentX : byte
{
    Left,
    Right,
    Center
}

public enum LayoutAlignmentY : byte
{
    Top,
    Bottom,
    Center
}

public enum SizingType : byte
{
    Fit,
    Grow,
    Percent,
    Fixed
}

public readonly record struct ChildAlignment(
    LayoutAlignmentX X,
    LayoutAlignmentY Y);

public readonly record struct SizingMinMax(
    float Min,
    float Max);

[StructLayout(LayoutKind.Explicit)]
public readonly record struct SizingAxis(
    [field: FieldOffset(0)] SizingMinMax SizeMinMax,
    [field: FieldOffset(0)] float SizePercent,
    [field: FieldOffset(8)] SizingType Type);

public readonly record struct Sizing(
    SizingAxis Width,
    SizingAxis Height);

public readonly record struct Padding(
    ushort X,
    ushort Y)
{
    public Padding(ushort value) : this(value, value)
    {
    }
}

public readonly record struct LayoutConfig(
    Sizing Sizing,
    Padding Padding,
    ushort ChildGap,
    ChildAlignment ChildAlignment,
    LayoutDirection LayoutDirection);

public readonly record struct RectangleElementConfig(
    Color Color,
    CornerRadius CornerRadius);

public enum TextElementConfigWrapMode
{
    Words,
    Newlines,
    None
}

public readonly record struct TextElementConfig(
    Color TextColor,
    ushort FontId,
    ushort FontSize,
    ushort LetterSpacing,
    ushort LineHeight,
    TextElementConfigWrapMode WrapMode);
    
public readonly record struct ImageElementConfig(
    IntPtr ImageData,
    Dimensions SourceDimensions);

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

public readonly record struct FloatingAttachPoints(
    FloatingAttachPointType Element,
    FloatingAttachPointType Parent);

public enum PointerCaptureMode
{
    Capture,
    Passthrough,
}

public readonly record struct FloatingElementConfig(
    Vector2 Offset,
    Dimensions Expand,
    ushort ZIndex,
    ushort ParentId,
    FloatingAttachPoints Attachment,
    PointerCaptureMode PointerCaptureMode);

public readonly record struct CustomElementConfig(
    IntPtr CustomData);

public readonly record struct ScrollElementConfig(
    [field: MarshalAs(UnmanagedType.I1)] bool Horizontal,
    [field: MarshalAs(UnmanagedType.I1)] bool Vertical);

public readonly record struct Border(
    uint Width,
    Color Color);

public readonly record struct BorderElementConfig(
    Border Left,
    Border Right,
    Border Top,
    Border Bottom,
    Border BetweenChildren,
    CornerRadius CornerRadius);

// RenderCommands

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
public readonly struct RenderCommand
{
    // Magically works with Span ctor because 0 offset and same size
    // Do not add fields to this or things will get bad!
    internal readonly Interop.RenderCommand NativeCommand;

    public BoundingBox BoundingBox => NativeCommand.BoundingBox;
    public uint Id => NativeCommand.Id;
    public RenderCommandType CommandType => NativeCommand.CommandType;
    
    public unsafe ref RectangleElementConfig GetRectangle() =>
        ref *NativeCommand.Config.RectangleElementConfig;

    public unsafe ref TextElementConfig GetText() =>
        ref *NativeCommand.Config.TextElementConfig;

    public unsafe ref ImageElementConfig GetImage() =>
        ref *NativeCommand.Config.ImageElementConfig;

    public unsafe ref FloatingElementConfig GetFloating() =>
        ref *NativeCommand.Config.FloatingElementConfig;

    public unsafe ref CustomElementConfig GetCustom() =>
        ref *NativeCommand.Config.CustomElementConfig;

    public unsafe ref ScrollElementConfig GetScroll() =>
        ref *NativeCommand.Config.ScrollElementConfig;

    public unsafe ref BorderElementConfig GetBorder() =>
        ref *NativeCommand.Config.BorderElementConfig;
}