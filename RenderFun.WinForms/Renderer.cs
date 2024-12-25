using RenderFun.Shared;

namespace RenderFun.WinForms;

public static class Renderer
{
    public static void Render(Graphics g, ReadOnlySpan<RenderCommand> renderCommands)
    {
        for (int i = 0; i < renderCommands.Length; i++)
        {
            var renderCommand = renderCommands[i];
            var boundingBox = renderCommand.BoundingBox.ToGdi();

            switch (renderCommand.CommandType)
            {
                case RenderCommandType.Rectangle:
                {
                    var config = renderCommand.GetRectangleConfig();
                    using var brush = new SolidBrush(config.Color.ToGdi());
                    Console.WriteLine($"\t{boundingBox}");
                    g.FillRectangle(brush, boundingBox);
                    break;
                }
            }
        }
    }

    public static void Render(Graphics g, ReadOnlySpan<RenderCommand2> renderCommands)
    {
        for (int i = 0; i < renderCommands.Length; i++)
        {
            var renderCommand = renderCommands[i];
            var boundingBox = renderCommand.BoundingBox.ToGdi();

            switch (renderCommand.CommandType)
            {
                case RenderCommandType.Rectangle:
                {
                    var config = renderCommand.RectangleConfig!;
                    using var brush = new SolidBrush(config.Color.ToGdi());
                    Console.WriteLine($"\t{boundingBox}");
                    g.FillRectangle(brush, boundingBox);
                    break;
                }
            }
        }
    }
}