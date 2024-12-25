using RenderFun.Shared;

namespace RenderFun.WinForms;

public static class Renderer
{
    public static void Render(Graphics g, ReadOnlySpan<RenderCommand> renderCommands)
    {
        foreach (var renderCommand in renderCommands)
        {
            var boundingBox = renderCommand.BoundingBox.ToGdi();

            switch (renderCommand.CommandType)
            {
                case RenderCommandType.Rectangle:
                {
                    var config = renderCommand.GetRectangleConfig();
                    using var brush = new SolidBrush(config.Color.ToGdi());
                    g.FillRectangle(brush, boundingBox);
                    break;
                }
            }
        }
    }

    public static void Render(Graphics g, ReadOnlySpan<RenderCommand2> renderCommands)
    {
        foreach (var renderCommand in renderCommands)
        {
            var boundingBox = renderCommand.BoundingBox.ToGdi();

            switch (renderCommand.CommandType)
            {
                case RenderCommandType.Rectangle:
                {
                    var config = renderCommand.RectangleConfig!.Value;
                    using var brush = new SolidBrush(config.Color.ToGdi());
                    g.FillRectangle(brush, boundingBox);
                    break;
                }
            }
        }
    }
}