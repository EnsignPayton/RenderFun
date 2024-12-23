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
        Interop.Sizing layoutExpand = new()
        {
            // CLAY_SIZING_GROW
            width = new() { type = Interop.SizingType.Grow },
            // CLAY_SIZING_GROW
            height = new() { type = Interop.SizingType.Grow },
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
                layoutDirection = Interop.LayoutDirection.TopToBottom,
                sizing = layoutExpand,
                padding = { x = 16, y = 16 },
                childGap = 8,
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
                    sizing = new()
                    {
                        // CLAY_SIZING_FIXED(60)
                        height = new() { type = Interop.SizingType.Fixed, sizeMinMax = { min = 60 } },
                        // CLAY_SIZING_GROW
                        width = new() { type = Interop.SizingType.Grow }
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
                    sizing = layoutExpand,
                    childGap = 8,
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
                        sizing = new()
                        {
                            // CLAY_SIZING_FIXED(250)
                            width = new() { type = Interop.SizingType.Fixed, sizeMinMax = { min = 250 } },
                            // CLAY_SIZING_GROW
                            height = new() { type = Interop.SizingType.Grow }
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
                        sizing = layoutExpand,
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
        Interop.Sizing layoutExpand = new()
        {
            width = Clay.SizingGrow(),
            height = Clay.SizingGrow()
        };

        RectangleElementConfig contentBackgroundConfig = new()
        {
            Color = new() { R = 90, G = 90, B = 90, A = 255 },
            CornerRadius = new() { TopLeft = 8, TopRight = 8, BottomLeft = 8, BottomRight = 8 }
        };

        Interop.BeginLayout();

        using (Clay.UI(
                   Clay.Id("OuterContainer"),
                   Clay.Rectangle(new() { Color = new() { R = 43, G = 41, B = 51, A = 255 } }),
                   Clay.Layout(new()
                   {
                       layoutDirection = Interop.LayoutDirection.TopToBottom,
                       sizing = layoutExpand,
                       padding = { x = 16, y = 16 },
                       childGap = 8,
                   })))
        {
            using (Clay.UI(
                       Clay.Id("HeaderBar"),
                       Clay.Rectangle(contentBackgroundConfig),
                       Clay.Layout(new()
                       {
                           sizing = new()
                           {
                               height = Clay.SizingFixed(60),
                               width = Clay.SizingGrow(),
                           },
                       })))
            {
            }

            using (Clay.UI(
                       Clay.Id("LowerContent"),
                       Clay.Layout(new()
                       {
                           sizing = layoutExpand,
                           childGap = 8,
                       })))
            {
                using (Clay.UI(
                           Clay.Id("Sidebar"),
                           Clay.Layout(new()
                           {
                               sizing = new()
                               {
                                   width = Clay.SizingFixed(250),
                                   height = Clay.SizingGrow(),
                               }
                           }),
                           Clay.Rectangle(contentBackgroundConfig)))
                {
                }

                using (Clay.UI(
                           Clay.Id("MainContent"),
                           Clay.Layout(new() { sizing = layoutExpand }),
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
        Console.WriteLine("Render Command Count: " + renderCommands->length);
        
        for (int i = 0; i < renderCommands->length; i++)
        {
            var pCommand = Interop.RenderCommandArray_Get(renderCommands, i);
            
            Console.WriteLine("Render Command " + i);
            Console.WriteLine("boundingBox");
            Console.WriteLine("\tx: " + pCommand->boundingBox.X);
            Console.WriteLine("\ty: " + pCommand->boundingBox.Y);
            Console.WriteLine("\twidth: " + pCommand->boundingBox.Width);
            Console.WriteLine("\theight: " + pCommand->boundingBox.Height);
            Console.WriteLine("config");
            Console.WriteLine("text");
            Console.WriteLine("id: " + pCommand->id);
            Console.WriteLine("commandType: " + pCommand->commandType);
        }
    }

    private static unsafe void Assert(Interop.Array<Interop.RenderCommand>* renderCommands)
    {
        Debug.Assert(renderCommands->length == 4);

        var c0 = Interop.RenderCommandArray_Get(renderCommands, 0);
        Debug.Assert(c0->boundingBox.X == 0);
        Debug.Assert(c0->boundingBox.Y == 0);
        Debug.Assert(c0->boundingBox.Width == 800);
        Debug.Assert(c0->boundingBox.Height == 600);
        Debug.Assert(c0->commandType == Interop.RenderCommandType.Rectangle);
        
        var c1 = Interop.RenderCommandArray_Get(renderCommands, 1);
        Debug.Assert(c1->boundingBox.X == 16);
        Debug.Assert(c1->boundingBox.Y == 16);
        Debug.Assert(c1->boundingBox.Width == 768);
        Debug.Assert(c1->boundingBox.Height == 60);
        Debug.Assert(c1->commandType == Interop.RenderCommandType.Rectangle);
        
        var c2 = Interop.RenderCommandArray_Get(renderCommands, 2);
        Debug.Assert(c2->boundingBox.X == 16);
        Debug.Assert(c2->boundingBox.Y == 84);
        Debug.Assert(c2->boundingBox.Width == 250);
        Debug.Assert(c2->boundingBox.Height == 500);
        Debug.Assert(c2->commandType == Interop.RenderCommandType.Rectangle);
        
        var c3 = Interop.RenderCommandArray_Get(renderCommands, 3);
        Debug.Assert(c3->boundingBox.X == 274);
        Debug.Assert(c3->boundingBox.Y == 84);
        Debug.Assert(c3->boundingBox.Width == 510);
        Debug.Assert(c3->boundingBox.Height == 500);
        Debug.Assert(c3->commandType == Interop.RenderCommandType.Rectangle);
    }
}