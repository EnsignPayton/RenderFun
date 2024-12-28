using RenderFun.Shared;

namespace RenderFun.UI;

public static class ExampleLayout
{
    private const int FontIdBody16 = 1;

    private static readonly Color BackgroundColor = new(43, 41, 51, 255);
    private static readonly Color ContentColor = new(90, 90, 90, 255);
    private static readonly Color ButtonColor = new(140, 140, 140, 255);

    public static void Layout()
    {
        var layoutExpand = new Sizing
        {
            Width = Clay.SizingGrow(),
            Height = Clay.SizingGrow()
        };

        var contentBackgroundConfig = new RectangleElementConfig
        {
            Color = ContentColor,
            CornerRadius = new CornerRadius(8)
        };

        Clay.UI()
            .WithId("OuterContainer")
            .WithRectangle(new RectangleElementConfig { Color = BackgroundColor })
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
                Color = ButtonColor,
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