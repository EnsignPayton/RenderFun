using System.Runtime.InteropServices;

namespace RenderFun;

public static class Clay
{
    // TODO: Alloc arena, init, free? Currently handled by the one-off ClayContext

    public  static SizingAxis SizingFixed(float min, float max = 0) => new()
    {
        Type = SizingType.Fixed,
        SizeMinMax = new() { Min = min, Max = max }
    };

    public static SizingAxis SizingGrow() => new()
    {
        Type = SizingType.Grow
    };

    #region Odin-like low-level wrapper

    internal static unsafe LayoutContext UI(params Interop.TypedConfig[] configs)
    {
        Interop._OpenElement();

        foreach (var config in configs)
        {
            switch (config.Type)
            {
                case Interop.TypedConfigType.Id:
                    Interop._AttachId(config.Id);
                    break;
                case Interop.TypedConfigType.Layout:
                    Interop._AttachLayoutConfig((LayoutConfig*)config.Config);
                    break;
                default:
                    Interop._AttachElementConfig(config.Config, (Interop._ElementConfigType)config.Type);
                    break;
            }
        }

        Interop._ElementPostConfiguration();
        return new LayoutContext();
    }

    internal static unsafe Interop.TypedConfig Layout(LayoutConfig config) => new()
    {
        Type = Interop.TypedConfigType.Layout,
        Config = Interop._StoreLayoutConfig(config)
    };

    internal static unsafe Interop.TypedConfig Rectangle(RectangleElementConfig config) => new()
    {
        Type = Interop.TypedConfigType.Rectangle,
        Config = Interop._StoreRectangleElementConfig(config)
    };

    internal static Interop.TypedConfig Id(string label, uint index = 0) => new()
    {
        Type = Interop.TypedConfigType.Id,
        Id = Interop._HashString(default, index, 0)
    };

    #endregion
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
