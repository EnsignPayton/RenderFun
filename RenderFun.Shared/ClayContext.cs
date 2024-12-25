using System.Diagnostics;
using System.Runtime.InteropServices;

namespace RenderFun.Shared;

internal sealed class ClayContext : IDisposable
{
    private readonly IntPtr _pArena;
    private bool _disposed;

    public ClayContext(Dimensions initialDimensions)
    {
        _pArena = InitClay(initialDimensions);
    }

    ~ClayContext()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private unsafe void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            Debug.WriteLine("Freeing Memory");
            NativeMemory.Free((void*)_pArena);
            _disposed = true;
        }
    }

    private static unsafe IntPtr InitClay(Dimensions dimensions)
    {
        var clayRequiredMemory = Interop.MinMemorySize();
        Debug.WriteLine($"Allocating {clayRequiredMemory} Bytes");
        var clayMemory = new Interop.Arena
        {
            Memory = NativeMemory.Alloc(clayRequiredMemory),
            Capacity = clayRequiredMemory,
        };

        Interop.Initialize(clayMemory, dimensions);

        Interop.SetMeasureTextFunction((pString, pConfig) =>
        {
            var len = pString->Length;
            var size = pConfig->FontSize;
            return new Dimensions(
                0.75f * (len * size),
                size);
        });

        return (IntPtr)clayMemory.Memory;
    }
}