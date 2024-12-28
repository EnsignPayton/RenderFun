using RenderFun.Shared;

namespace RenderFun.Avalonia;

public static class ExampleLayout
{
    private const int FontIdBody16 = 1;

    public static void Layout()
    {
        var bgColor = new Color { R = 43, G = 41, B = 51, A = 255 };
        var contentBgColor = new Color { R = 90, G = 90, B = 90, A = 255 };

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
                        Sizing = new Sizing { Height = Clay.SizingFixed(60), Width = Clay.SizingGrow(), },
                        Padding = new Padding { X = 16 },
                        ChildGap = 16,
                        ChildAlignment = new ChildAlignment { Y = LayoutAlignmentY.Center }
                    })
                    .WithChildren(() =>
                    {
                        RenderHeaderButton("File");
                        RenderHeaderButton("Edit");

                        Clay.UI()
                            .WithLayout(new LayoutConfig { Sizing = new Sizing { Width = Clay.SizingGrow() } })
                            .Build();

                        RenderHeaderButton("Upload");
                        RenderHeaderButton("Media");
                        RenderHeaderButton("Support");
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
                                LayoutDirection = LayoutDirection.TopToBottom,
                                Sizing = new Sizing { Width = Clay.SizingFixed(250), Height = Clay.SizingGrow(), }
                            })
                            .WithChildren(() =>
                            {
                                // TODO: Stuff
                            })
                            .Build();

                        Clay.UI()
                            .WithId("MainContent")
                            .WithRectangle(contentBackgroundConfig)
                            .WithLayout(new LayoutConfig { Sizing = layoutExpand })
                            .Build();
                    })
                    .Build();
            })
            .Build();
    }

    private static void RenderHeaderButton(string text)
    {
        Clay.UI()
            .WithLayout(new LayoutConfig { Padding = new Padding(16, 8) })
            .WithRectangle(new RectangleElementConfig
            {
                Color = new Color(140, 140, 140, 255),
                CornerRadius = new CornerRadius(5)
            })
            .WithChildren(() =>
            {
                Clay.Text(text)
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
}