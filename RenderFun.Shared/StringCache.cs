using System.Text;

namespace RenderFun.Shared;

internal class StringCache
{
    public static StringCache Default { get; } = new();

    private readonly Dictionary<string, ReadOnlyMemory<byte>> _cache = [];

    public ReadOnlyMemory<byte> GetOrAdd(string value)
    {
        if (_cache.TryGetValue(value, out var result)) return result;

        ReadOnlyMemory<byte> bytes = Encoding.ASCII.GetBytes(value);
        _cache[value] = bytes;
        return bytes;
    }
}