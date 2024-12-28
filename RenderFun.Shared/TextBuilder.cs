using System.Runtime.CompilerServices;

namespace RenderFun.Shared;

public ref struct TextBuilder(ReadOnlyMemory<byte> text)
{
    private readonly ReadOnlyMemory<byte> _text = text;
    private ref TextElementConfig _config;

    public unsafe TextBuilder WithConfig(TextElementConfig config)
    {
        _config = ref *Interop._StoreTextElementConfig(config);
        return this;
    }

    public unsafe void Build()
    {
        if (!Unsafe.IsNullRef(ref _config))
            Interop._OpenTextElement(Interop.String.FromMemory(_text), (TextElementConfig*)Unsafe.AsPointer(ref _config));
    }
}