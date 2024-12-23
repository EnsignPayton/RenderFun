using System.Runtime.CompilerServices;

namespace RenderFun;

public ref struct LayoutBuilder
{
    private ref LayoutConfig _layoutConfig;
    private ref RectangleElementConfig _rectConfig;
    private Action? _childBuilder;

    public LayoutBuilder WithId(string value)
    {
        return this;
    }

    public unsafe LayoutBuilder WithLayout(LayoutConfig config)
    {
        var handle = Interop._StoreLayoutConfig(config);
        _layoutConfig = ref *handle;
        return this;
    }

    public unsafe LayoutBuilder WithRectangle(RectangleElementConfig config)
    {
        var handle = Interop._StoreRectangleElementConfig(config);
        _rectConfig = ref *handle;
        return this;
    }

    public LayoutBuilder WithChildren(Action configure)
    {
        _childBuilder = configure;
        return this;
    }

    public void Build()
    {
        Interop._OpenElement();
        Attach();
        Interop._ElementPostConfiguration();
        _childBuilder?.Invoke();
        Interop._CloseElement();
    }

    private unsafe void Attach()
    {
        if (!Unsafe.IsNullRef(ref _layoutConfig))
            Interop._AttachLayoutConfig((LayoutConfig*) Unsafe.AsPointer(ref _layoutConfig));
        
        if (!Unsafe.IsNullRef(ref _rectConfig))
            Interop._AttachElementConfig(Unsafe.AsPointer(ref _rectConfig), Interop._ElementConfigType.Rectangle);
    }
}