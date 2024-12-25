using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace RenderFun.Shared;

// Stupid Idea
// I can layout on Linux, but I can't yet render
// I can render on Windows, but I can't yet layout
// What if I opened a TCP connection between them,
// ran the layout on Linux, and send all the draw calls
// over the wire to Windows?
//
// I guess it doesn't really matter who's acting as the server
// Backend               Frontend
// ~~~ TCP Secret Handshake ~~~
// <-- Init_Dimensions
// Frame -->
// Frame -->
// ...
// <-- Dimensions_Changed
// <-- Pointer_Event
// <-- Other_Feedback
// Frame -->
// (It's frames all the way down)
public static class ClayNet
{
    public static byte[] Serialize(RenderCommand command)
    {
        // How do I serialize this if it has pointers and stuff?
        // I suppose I can dereference the appropriate pointer and copy the struct it points too to, two
        using var ms = new MemoryStream();
        // 4x4
        ms.Write(StructToBytes(command.BoundingBox));
        // 4
        ms.Write(BitConverter.GetBytes(command.Id));
        // 4
        ms.Write(BitConverter.GetBytes((int)command.CommandType));
        // TODO: Text?

        switch (command.CommandType)
        {
            case RenderCommandType.Rectangle:
                ms.Write(StructToBytes(command.GetRectangleConfig()));
                break;
            // TODO: Other types
        }

        return ms.ToArray();
    }

    public static unsafe RenderCommand DeserializeRenderCommand(ReadOnlySpan<byte> data, out int bytesEaten)
    {
        var nativeCommand = new Interop.RenderCommand();

        nativeCommand.BoundingBox = BytesToStruct<BoundingBox>(data);
        data = data[sizeof(BoundingBox)..];
        nativeCommand.Id = BitConverter.ToUInt32(data[..4]);
        data = data[4..];
        nativeCommand.CommandType = (RenderCommandType)BitConverter.ToInt32(data[..4]);
        data = data[4..];

        bytesEaten = sizeof(BoundingBox) + 4 + 4;

        switch (nativeCommand.CommandType)
        {
            case RenderCommandType.Rectangle:
            {
                var config = BytesToStruct<RectangleElementConfig>(data);
                // Now pin it forever, I guess
                // Man I really need a better way to do this
                var configHandle = GCHandle.Alloc(config, GCHandleType.Pinned);
                var configPtr = (RectangleElementConfig*)configHandle.AddrOfPinnedObject();
                nativeCommand.Config = new() { RectangleElementConfig = configPtr };
                bytesEaten += sizeof(RectangleElementConfig);
                break;
            }
        }

        return new RenderCommand(nativeCommand);
    }

    private static unsafe byte[] StructToBytes<T>(T value) where T : unmanaged
    {
        var ptr = Marshal.AllocHGlobal(sizeof(T));

        try
        {
            var data = new byte[sizeof(T)];
            *(T*)ptr = value;
            Marshal.Copy(ptr, data, 0, sizeof(T));
        }
        finally
        {
            Marshal.FreeHGlobal(ptr);
        }

        return [];
    }

    public static unsafe T BytesToStruct<T>(ReadOnlySpan<byte> value) where T : unmanaged
    {
        fixed (byte* ptr = value)
        {
            return *(T*)(IntPtr)ptr;
        }
    }
}

public sealed class ClayNetServer : IDisposable
{
    private readonly TcpListener _listener = new(IPAddress.Any, 42069);
    private readonly CancellationTokenSource _cts = new();
    private Thread? _listenerThread;

    public event Action<RenderBatch>? LayoutReceived;

    public void Dispose()
    {
        _cts.Cancel();
        if (_listenerThread is not null)
        {
            _listenerThread.Join();
            _listenerThread = null;
        }

        _listener.Dispose();
    }

    public void Start()
    {
        _listener.Start();
        _listenerThread = new Thread(() => Run(_cts.Token));
        _listenerThread.Start();
    }

    private void Run(CancellationToken token)
    {
        Span<byte> recvbuf = stackalloc byte[4096];

        while (!token.IsCancellationRequested)
        {
            // One client at a time
            using var client = _listener.AcceptTcpClient();
            var stream = client.GetStream();

            while (client.Connected)
            {
                var received = stream.Read(recvbuf);
                var actual = recvbuf[received..];
                var batch = RenderBatch.Deserialize(actual);
                LayoutReceived?.Invoke(batch);
            }
        }
    }
}

public sealed class ClayNetClient : IDisposable
{
    private readonly TcpClient _client = new();

    public void Dispose()
    {
    }

    public void Connect(string host)
    {
        _client.Connect(host, 42069);
    }

    public void SendBatch(RenderBatch value)
    {
        var bytes = value.Serialize();
        _client.Client.Send(bytes);
    }
}

public struct RenderBatch
{
    public RenderCommand[] Commands;

    public byte[] Serialize()
    {
        using var ms = new MemoryStream();

        ms.Write(BitConverter.GetBytes(Commands.Length));

        foreach (var command in Commands)
        {
            ms.Write(ClayNet.Serialize(command));
        }

        return ms.ToArray();
    }

    public static RenderBatch Deserialize(ReadOnlySpan<byte> data)
    {
        var count = BitConverter.ToUInt32(data[..4]);
        data = data[4..];

        var result = new RenderBatch { Commands = new RenderCommand[count] };

        for (int i = 0; i < count; i++)
        {
            result.Commands[i] = ClayNet.DeserializeRenderCommand(data, out var eaten);
            data = data[eaten..];
        }

        return result;
    }
}