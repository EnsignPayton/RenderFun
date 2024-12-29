using System.Runtime.CompilerServices;

namespace RenderFun.Shared;

public ref struct LayoutBuilder
{
    private ref LayoutConfig _layoutConfig;
    private ref RectangleElementConfig _rectConfig;
    private ref FloatingElementConfig _floatingConfig;

    private Action? _childBuilder;

    public LayoutBuilder Id(string value)
    {
        var memory = StringCache.Default.GetOrAdd(value);
        var clayString = Interop.String.FromMemory(memory);
        var elementId = Interop._HashString(clayString, 0, 0);
        Interop._AttachId(elementId);
        return this;
    }

    public unsafe LayoutBuilder Layout(LayoutConfig config)
    {
        _layoutConfig = ref *Interop._StoreLayoutConfig(config);
        return this;
    }

    public unsafe LayoutBuilder Rectangle(RectangleElementConfig config)
    {
        _rectConfig = ref *Interop._StoreRectangleElementConfig(config);
        return this;
    }

    public unsafe LayoutBuilder Floating(FloatingElementConfig config)
    {
        _floatingConfig = ref *Interop._StoreFloatingElementConfig(config);
        return this;
    }

    public LayoutBuilder Children(Action configure)
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

        if (!Unsafe.IsNullRef(ref _floatingConfig))
            Interop._AttachElementConfig(Unsafe.AsPointer(ref _floatingConfig), Interop._ElementConfigType.FloatingContainer);
    }
}