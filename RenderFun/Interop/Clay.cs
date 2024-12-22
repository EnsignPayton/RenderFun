using System.Runtime.InteropServices;

namespace RenderFun.Interop;

internal static partial class Clay
{
    private const string LibName = "clay";
    
    [LibraryImport(LibName)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    public static partial uint Clay_MinMemorySize();
}