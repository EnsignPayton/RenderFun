using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using RenderFun.Shared;

namespace RenderFun.Avalonia;

public class ClayCanvas : Control
{
    private IDisposable? _clayContext;

    protected override void OnInitialized()
    {
        _clayContext = Clay.Initialize(new Dimensions((float)Width, (float)Height));
        base.OnInitialized();
    }

    public override void Render(DrawingContext context)
    {
        Clay.SetLayoutDimensions(new Dimensions((float)Width, (float)Height));
        Clay.BeginLayout();

        ExampleLayout.Layout();

        var renderCommands = Clay.EndLayout();

        Renderer.Render(context, RenderCommand2.FromRenderCommands(renderCommands));
        base.Render(context);
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);
        _clayContext?.Dispose();
    }
}