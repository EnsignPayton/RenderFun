using System.Runtime.InteropServices;

namespace RenderFun;

public static class Clay
{
    // TODO: Alloc arena, init, free? Currently handled by the one-off ClayContext
    
    #region Odin-like low-level wrapper
    
    internal static unsafe LayoutContext UI(params Interop.TypedConfig[] configs)
    {
        Interop._OpenElement();

        foreach (var config in configs)
        {
            switch (config.type)
            {
                case Interop.TypedConfigType.Id:
                    Interop._AttachId(config.id);
                    break;
                case Interop.TypedConfigType.Layout:
                    Interop._AttachLayoutConfig((Interop.LayoutConfig*)config.config);
                    break;
                default:
                    Interop._AttachElementConfig(config.config, (Interop._ElementConfigType)config.type);
                    break;
            }
        }
        
        Interop._ElementPostConfiguration();
        return new LayoutContext();
    }

    internal static unsafe Interop.TypedConfig Layout(Interop.LayoutConfig config) => new()
    {
        type = Interop.TypedConfigType.Layout,
        config = Interop._StoreLayoutConfig(config)
    };

    internal static unsafe Interop.TypedConfig Rectangle(RectangleElementConfig config) => new()
    {
        type = Interop.TypedConfigType.Rectangle,
        config = Interop._StoreRectangleElementConfig(config)
    };

    internal static Interop.SizingAxis SizingFixed(float min, float max = 0) => new()
    {
        type = Interop.SizingType.Fixed,
        sizeMinMax = { min = min, max = max }
    };

    internal static Interop.SizingAxis SizingGrow() => new()
    {
        type = Interop.SizingType.Grow
    };

    internal static Interop.TypedConfig Id(string label, uint index = 0) => new()
    {
        type = Interop.TypedConfigType.Id,
        id = Interop._HashString(default, index, 0)
    };
    
    #endregion
}

// Public API Types

public readonly record struct Dimensions(float Width, float Height);

public readonly record struct Vector2(float X, float Y);

public readonly record struct Color(float R, float G, float B, float A);

public readonly record struct BoundingBox(float X, float Y, float Width, float Height);

// ElementId

public readonly record struct CornerRadius(float TopLeft, float TopRight, float BottomLeft, float BottomRight);

//public readonly struct SizingMinMax(float Min, float Max);

// TODO: Others

public readonly record struct RectangleElementConfig(Color Color, CornerRadius CornerRadius);