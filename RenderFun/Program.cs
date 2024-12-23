using System.Diagnostics;

namespace RenderFun;

internal static class Program
{
    public static void Main(string[] args)
    {
        using var clay = new ClayContext(new Dimensions(800, 600));
        
        Frame();
        Frame2();
    }

    private static unsafe void Frame()
    {
        Sizing layoutExpand = new()
        {
            // CLAY_SIZING_GROW
            Width = new() { Type = SizingType.Grow },
            // CLAY_SIZING_GROW
            Height = new() { Type = SizingType.Grow },
        };

        RectangleElementConfig contentBackgroundConfig = new()
        {
            Color = new() { R = 90, G = 90, B = 90, A = 255 },
            CornerRadius = new() { TopLeft = 8, TopRight = 8, BottomLeft = 8, BottomRight = 8 }
        };
        
        Interop.BeginLayout();

        // CLAY OuterContainer
        {
            // TODO: CLAY_ID
            // CLAY_RECTANGLE
            var rectangleConfig = Interop._StoreRectangleElementConfig(new()
            {
                Color = new() { R = 43, G = 41, B = 51, A = 255 }
            });
            // CLAY_LAYOUT
            var layoutConfig = Interop._StoreLayoutConfig(new()
            {
                LayoutDirection = LayoutDirection.TopToBottom,
                Sizing = layoutExpand,
                Padding = new() { X = 16, Y = 16 },
                ChildGap = 8,
            });
            
            Interop._OpenElement();
            Interop._AttachElementConfig(rectangleConfig, Interop._ElementConfigType.Rectangle);
            Interop._AttachLayoutConfig(layoutConfig);
            Interop._ElementPostConfiguration();
            
            // Child components here
            // CLAY HeaderBar
            {
                // TODO: CLAY_ID
                var headerRectangleConfig = Interop._StoreRectangleElementConfig(contentBackgroundConfig);
                var headerLayoutConfig = Interop._StoreLayoutConfig(new()
                {
                    Sizing = new()
                    {
                        // CLAY_SIZING_FIXED(60)
                        Height = new() { Type = SizingType.Fixed, SizeMinMax = new() { Min = 60 } },
                        // CLAY_SIZING_GROW
                        Width = new() { Type = SizingType.Grow }
                    },
                });
                
                Interop._OpenElement();
                Interop._AttachElementConfig(headerRectangleConfig, Interop._ElementConfigType.Rectangle);
                Interop._AttachLayoutConfig(headerLayoutConfig);
                Interop._ElementPostConfiguration();
                
                // Child components here

                Interop._CloseElement();
            }
            // CLAY LowerContent
            {
                // TODO: CLAY_ID
                var lowerLayoutConfig = Interop._StoreLayoutConfig(new()
                {
                    Sizing = layoutExpand,
                    ChildGap = 8,
                });
                
                Interop._OpenElement();
                Interop._AttachLayoutConfig(lowerLayoutConfig);
                Interop._ElementPostConfiguration();
                
                // Child components here
                
                // CLAY Sidebar
                {
                    // TODO: CLAY_ID
                    var sidebarLayoutConfig = Interop._StoreLayoutConfig(new()
                    {
                        Sizing = new()
                        {
                            // CLAY_SIZING_FIXED(250)
                            Width = new() { Type = SizingType.Fixed, SizeMinMax = new() { Min = 250 } },
                            // CLAY_SIZING_GROW
                            Height = new() { Type = SizingType.Grow }
                        }
                    });
                    var sidebarRectangleConfig = Interop._StoreRectangleElementConfig(contentBackgroundConfig);
                    
                    Interop._OpenElement();
                    Interop._AttachLayoutConfig(sidebarLayoutConfig);
                    Interop._AttachElementConfig(sidebarRectangleConfig, Interop._ElementConfigType.Rectangle);
                    Interop._ElementPostConfiguration();
                    // Child components here
                    Interop._CloseElement();
                }
                
                // CLAY MainContent
                {
                    // TODO: CLAY_ID
                    var mainLayoutConfig = Interop._StoreLayoutConfig(new()
                    {
                        Sizing = layoutExpand,
                    });
                    var mainRectangleConfig = Interop._StoreRectangleElementConfig(contentBackgroundConfig);
                    Interop._OpenElement();
                    Interop._AttachLayoutConfig(mainLayoutConfig);
                    Interop._AttachElementConfig(mainRectangleConfig, Interop._ElementConfigType.Rectangle);
                    Interop._ElementPostConfiguration();
                    // Child components here
                    Interop._CloseElement();
                }
                
                Interop._CloseElement();
            }
            
            Interop._CloseElement();
        }
        
        var renderCommands = Interop.EndLayout();
        Log(&renderCommands);
        Assert(&renderCommands);
    }

    private static unsafe void Frame2()
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

        Interop.BeginLayout();

        using (Clay.UI(
                   Clay.Id("OuterContainer"),
                   Clay.Rectangle(new RectangleElementConfig { Color = bgColor }),
                   Clay.Layout(new LayoutConfig
                   {
                       LayoutDirection = LayoutDirection.TopToBottom,
                       Sizing = layoutExpand,
                       Padding = new Padding(16),
                       ChildGap = 8,
                   })))
        {
            using (Clay.UI(
                       Clay.Id("HeaderBar"),
                       Clay.Rectangle(contentBackgroundConfig),
                       Clay.Layout(new LayoutConfig
                       {
                           Sizing = new Sizing
                           {
                               Height = Clay.SizingFixed(60),
                               Width = Clay.SizingGrow(),
                           },
                       })))
            {
            }

            using (Clay.UI(
                       Clay.Id("LowerContent"),
                       Clay.Layout(new LayoutConfig
                       {
                           Sizing = layoutExpand,
                           ChildGap = 8,
                       })))
            {
                using (Clay.UI(
                           Clay.Id("Sidebar"),
                           Clay.Layout(new LayoutConfig
                           {
                               Sizing = new Sizing
                               {
                                   Width = Clay.SizingFixed(250),
                                   Height = Clay.SizingGrow(),
                               }
                           }),
                           Clay.Rectangle(contentBackgroundConfig)))
                {
                }

                using (Clay.UI(
                           Clay.Id("MainContent"),
                           Clay.Layout(new LayoutConfig { Sizing = layoutExpand }),
                           Clay.Rectangle(contentBackgroundConfig)))
                {
                }
            }
        }

        var renderCommands = Interop.EndLayout();
        Log(&renderCommands);
        Assert(&renderCommands);
    }
    
    // The design with IDisposable gets the job done, but it's ugly as hell. I'd rather something like this:
    // Clay.UI()
    //     .WithId("OuterContainer")
    //     .WithRectangle(foo)
    //     .WithLayout(foo)
    //     .WithChildren(() =>
    //     {
    //         Clay.UI()
    //             .WithId("HeaderBar");
    //     });

    private static unsafe void Log(Interop.Array<Interop.RenderCommand>* renderCommands)
    {
        Console.WriteLine("Render Command Count: " + renderCommands->Length);
        
        for (int i = 0; i < renderCommands->Length; i++)
        {
            var pCommand = Interop.RenderCommandArray_Get(renderCommands, i);
            
            Console.WriteLine("Render Command " + i);
            Console.WriteLine("boundingBox");
            Console.WriteLine("\tx: " + pCommand->BoundingBox.X);
            Console.WriteLine("\ty: " + pCommand->BoundingBox.Y);
            Console.WriteLine("\twidth: " + pCommand->BoundingBox.Width);
            Console.WriteLine("\theight: " + pCommand->BoundingBox.Height);
            Console.WriteLine("config");
            Console.WriteLine("text");
            Console.WriteLine("id: " + pCommand->Id);
            Console.WriteLine("commandType: " + pCommand->CommandType);
        }
    }

    private static unsafe void Assert(Interop.Array<Interop.RenderCommand>* renderCommands)
    {
        Debug.Assert(renderCommands->Length == 4);

        var c0 = Interop.RenderCommandArray_Get(renderCommands, 0);
        Debug.Assert(c0->BoundingBox.X == 0);
        Debug.Assert(c0->BoundingBox.Y == 0);
        Debug.Assert(c0->BoundingBox.Width == 800);
        Debug.Assert(c0->BoundingBox.Height == 600);
        Debug.Assert(c0->CommandType == Interop.RenderCommandType.Rectangle);
        
        var c1 = Interop.RenderCommandArray_Get(renderCommands, 1);
        Debug.Assert(c1->BoundingBox.X == 16);
        Debug.Assert(c1->BoundingBox.Y == 16);
        Debug.Assert(c1->BoundingBox.Width == 768);
        Debug.Assert(c1->BoundingBox.Height == 60);
        Debug.Assert(c1->CommandType == Interop.RenderCommandType.Rectangle);
        
        var c2 = Interop.RenderCommandArray_Get(renderCommands, 2);
        Debug.Assert(c2->BoundingBox.X == 16);
        Debug.Assert(c2->BoundingBox.Y == 84);
        Debug.Assert(c2->BoundingBox.Width == 250);
        Debug.Assert(c2->BoundingBox.Height == 500);
        Debug.Assert(c2->CommandType == Interop.RenderCommandType.Rectangle);
        
        var c3 = Interop.RenderCommandArray_Get(renderCommands, 3);
        Debug.Assert(c3->BoundingBox.X == 274);
        Debug.Assert(c3->BoundingBox.Y == 84);
        Debug.Assert(c3->BoundingBox.Width == 510);
        Debug.Assert(c3->BoundingBox.Height == 500);
        Debug.Assert(c3->CommandType == Interop.RenderCommandType.Rectangle);
    }
}