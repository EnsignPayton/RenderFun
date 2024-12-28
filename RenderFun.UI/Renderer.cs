using Avalonia;
using Avalonia.Media;
using RenderFun.Shared;

namespace RenderFun.UI;

public static class Renderer
{
    public static void Render(DrawingContext context, RenderCommand2[] commands)
    {
        foreach (var renderCommand in commands)
        {
            var bb = renderCommand.BoundingBox.ToAvalonia();

            switch (renderCommand.CommandType)
            {
                case RenderCommandType.Rectangle:
                {
                    var config = renderCommand.RectangleConfig!.Value;
                    var cr = config.CornerRadius.ToAvalonia();
                    var color = config.Color.ToAvalonia();
                    var brush = new SolidColorBrush(color);
                    context.DrawRectangle(brush, null, new RoundedRect(bb, cr));
                    break;
                }
            }
        }
    }

    private static Avalonia.Media.Color ToAvalonia(this Shared.Color value) =>
        new((byte)value.A, (byte)value.R, (byte)value.G, (byte)value.B);

    private static Avalonia.Rect ToAvalonia(this Shared.BoundingBox value) =>
        new(value.X, value.Y, value.Width, value.Height);

    private static Avalonia.CornerRadius ToAvalonia(this Shared.CornerRadius value) =>
        new(value.TopLeft, value.TopRight, value.BottomRight, value.BottomLeft);
}