using System.Diagnostics;
using System.Runtime.InteropServices;

namespace RenderFun;

public sealed class ClayContext : IDisposable
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
            memory = NativeMemory.Alloc(clayRequiredMemory),
            capacity = clayRequiredMemory,
        };

        Interop.Initialize(clayMemory, dimensions);
        
        return (IntPtr)clayMemory.memory;
    }
}