using System.Diagnostics;

namespace RenderFun;

internal static class Program
{
    private const int FontIdBody16 = 1;

    public static void Main(string[] args)
    {
        using (Clay.Initialize(new Dimensions(800, 600)))
        {
            SetDummyMeasureText();

            // frame
            {
                Clay.BeginLayout();

                Layout();

                var renderCommands = Clay.EndLayout();
                Log(renderCommands);
                //Assert(renderCommands);
            }
        }
    }

    private static unsafe void SetDummyMeasureText()
    {
        Interop.SetMeasureTextFunction((pString, pConfig) =>
        {
            var len = pString->Length;
            var size = pConfig->FontSize;
            return new Dimensions(
                0.75f * (len * size),
                size);
        });
    }

    private static void Layout()
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

        Clay.UI()
            .WithId("OuterContainer"u8)
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
                    .WithId("HeaderBar"u8)
                    .WithRectangle(contentBackgroundConfig)
                    .WithLayout(new LayoutConfig
                    {
                        Sizing = new Sizing
                        {
                            Height = Clay.SizingFixed(60),
                            Width = Clay.SizingGrow(),
                        },
                        Padding = new Padding { X = 16 },
                        ChildGap = 16,
                        ChildAlignment = new ChildAlignment { Y = LayoutAlignmentY.Center }
                    })
                    .WithChildren(() =>
                    {
                        RenderHeaderButton("File"u8);
                        RenderHeaderButton("Edit"u8);

                        Clay.UI()
                            .WithLayout(new LayoutConfig
                            {
                                Sizing = new Sizing { Width = Clay.SizingGrow() }
                            })
                            .Build();

                        RenderHeaderButton("Upload"u8);
                        RenderHeaderButton("Media"u8);
                        RenderHeaderButton("Support"u8);
                    })
                    .Build();

                Clay.UI()
                    .WithId("LowerContent"u8)
                    .WithLayout(new LayoutConfig
                    {
                        Sizing = layoutExpand,
                        ChildGap = 8,
                    })
                    .WithChildren(() =>
                    {
                        Clay.UI()
                            .WithId("Sidebar"u8)
                            .WithRectangle(contentBackgroundConfig)
                            .WithLayout(new LayoutConfig
                            {
                                LayoutDirection = LayoutDirection.TopToBottom,
                                Sizing = new Sizing
                                {
                                    Width = Clay.SizingFixed(250),
                                    Height = Clay.SizingGrow(),
                                }
                            })
                            .WithChildren(() =>
                            {
                                // TODO: Stuff
                            })
                            .Build();

                        Clay.UI()
                            .WithId("MainContent"u8)
                            .WithRectangle(contentBackgroundConfig)
                            .WithLayout(new LayoutConfig { Sizing = layoutExpand })
                            .Build();
                    })
                    .Build();
            })
            .Build();
    }

    private static void RenderHeaderButton(ReadOnlySpan<byte> text)
    {
        var mem = new ReadOnlyMemory<byte>(text.ToArray());
        Clay.UI()
            .WithLayout(new LayoutConfig { Padding = new Padding(16, 8) })
            .WithRectangle(new RectangleElementConfig
            {
                Color = new Color(140, 140, 140, 255),
                CornerRadius = new CornerRadius(5)
            })
            .WithChildren(() =>
            {
                Clay.Text(mem)
                    .WithConfig(new TextElementConfig
                    {
                        FontId = FontIdBody16,
                        FontSize = 16,
                        TextColor = new Color(255, 255, 255, 255)
                    })
                    .Build();
            })
            .Build();
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