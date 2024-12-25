namespace RenderFun.WinForms;

public static class Mapping
{
    public static Color ToGdi(this RenderFun.Shared.Color value) =>
        Color.FromArgb((int)value.A, (int)value.R, (int)value.G, (int)value.B);

    public static RectangleF ToGdi(this RenderFun.Shared.BoundingBox value) =>
        new(value.X, value.Y, value.Width, value.Height);

    public static PointF ToGdi(this RenderFun.Shared.Vector2 value) =>
        new(value.X, value.Y);

    public static RenderFun.Shared.Dimensions ToClayDimensions(this Rectangle value) =>
        new(value.Width, value.Height);
}