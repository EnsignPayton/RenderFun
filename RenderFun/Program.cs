using System.Diagnostics;

namespace RenderFun;

internal static class Program
{
    public static void Main(string[] args)
    {
        using (Clay.Initialize(new Dimensions(800, 600)))
        {
            Frame();
        }
    }

    private static void Frame()
    {
        var bgColor = new Color { R = 90, G = 90, B = 90, A = 255 };
        var contentBgColor = new Color { R = 43, G = 41, B = 51, A = 255 };

        var layoutExpand = new Sizing
        {
            Width = Clay.SizingGrow(),
            Height = Clay.SizingGrow()
        };

        var contentBackgroundConfig = new RectangleElementConfig
        {
            Color = contentBgColor,
            CornerRadius = new CornerRadius(8)
        };

        Clay.BeginLayout();

        Clay.UI()
            .WithId("OuterContainer")
            .WithRectangle(new RectangleElementConfig { Color = bgColor })
            .WithLayout(new LayoutConfig
            {
                LayoutDirection = LayoutDirection.TopToBottom,
                Sizing = layoutExpand,
                Padding = new Padding(16),
                ChildGap = 8,
            })
            .WithChildren(() =>
            {
                Clay.UI()
                    .WithId("HeaderBar")
                    .WithRectangle(contentBackgroundConfig)
                    .WithLayout(new LayoutConfig
                    {
                        Sizing = new Sizing
                        {
                            Height = Clay.SizingFixed(60),
                            Width = Clay.SizingGrow(),
                        },
                    })
                    .Build();

                Clay.UI()
                    .WithId("LowerContent")
                    .WithLayout(new LayoutConfig
                    {
                        Sizing = layoutExpand,
                        ChildGap = 8,
                    })
                    .WithChildren(() =>
                    {
                        Clay.UI()
                            .WithId("Sidebar")
                            .WithRectangle(contentBackgroundConfig)
                            .WithLayout(new LayoutConfig
                            {
                                Sizing = new Sizing
                                {
                                    Width = Clay.SizingFixed(250),
                                    Height = Clay.SizingGrow(),
                                }
                            })
                            .Build();

                        Clay.UI()
                            .WithId("MainContant")
                            .WithRectangle(contentBackgroundConfig)
                            .WithLayout(new LayoutConfig { Sizing = layoutExpand })
                            .Build();
                    })
                    .Build();
            })
            .Build();

        var renderCommands = Clay.EndLayout();
        Log(renderCommands);
        Assert(renderCommands);
    }
    
    private static void Log(ReadOnlySpan<RenderCommand> renderCommands)
    {
        Console.WriteLine("Command Count: " + renderCommands.Length);
        
        for (int i = 0; i < renderCommands.Length; i++)
        {
            var c = renderCommands[i];
            Console.WriteLine($"Command {i}");
            Console.WriteLine(c);
        }
    }

    private static void Assert(ReadOnlySpan<RenderCommand> renderCommands)
    {
        Debug.Assert(renderCommands.Length == 4);

        var c0 = renderCommands[0];
        Debug.Assert(c0.BoundingBox.X == 0);
        Debug.Assert(c0.BoundingBox.Y == 0);
        Debug.Assert(c0.BoundingBox.Width == 800);
        Debug.Assert(c0.BoundingBox.Height == 600);
        Debug.Assert(c0.CommandType == RenderCommandType.Rectangle);
        
        var c1 = renderCommands[1];
        Debug.Assert(c1.BoundingBox.X == 16);
        Debug.Assert(c1.BoundingBox.Y == 16);
        Debug.Assert(c1.BoundingBox.Width == 768);
        Debug.Assert(c1.BoundingBox.Height == 60);
        Debug.Assert(c1.CommandType == RenderCommandType.Rectangle);
        
        var c2 = renderCommands[2];
        Debug.Assert(c2.BoundingBox.X == 16);
        Debug.Assert(c2.BoundingBox.Y == 84);
        Debug.Assert(c2.BoundingBox.Width == 250);
        Debug.Assert(c2.BoundingBox.Height == 500);
        Debug.Assert(c2.CommandType == RenderCommandType.Rectangle);
        
        var c3 = renderCommands[3];
        Debug.Assert(c3.BoundingBox.X == 274);
        Debug.Assert(c3.BoundingBox.Y == 84);
        Debug.Assert(c3.BoundingBox.Width == 510);
        Debug.Assert(c3.BoundingBox.Height == 500);
        Debug.Assert(c3.CommandType == RenderCommandType.Rectangle);
    }
}