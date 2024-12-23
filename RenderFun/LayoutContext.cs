namespace RenderFun;

public sealed class LayoutContext : IDisposable
{
    public void Dispose()
    {
        Interop._CloseElement();
    }
}