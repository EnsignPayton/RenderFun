using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using RenderFun.Shared;

namespace RenderFun.UI;

public class ClayCanvas : Control
{
    private IDisposable? _clayContext;

    private Dimensions ContentSize => new((float)Bounds.Width, (float)Bounds.Height);

    protected override void OnInitialized()
    {
        _clayContext = Clay.Initialize(ContentSize);
        base.OnInitialized();
    }

    public override void Render(DrawingContext context)
    {
        Clay.SetLayoutDimensions(ContentSize);
        Clay.BeginLayout();

        ExampleLayout.Layout();

        var renderCommands = Clay.EndLayout();

        Renderer.Render(context, renderCommands);
        base.Render(context);
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);
        _clayContext?.Dispose();
    }
}