using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace RenderFun.Shared;

public sealed class ClayNetServer : IDisposable
{
    private readonly TcpListener _listener = new(IPAddress.Any, 42069);
    private readonly CancellationTokenSource _cts = new();
    private Thread? _listenerThread;

    public event Action<RenderCommand2[]>? LayoutReceived;

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
        Span<byte> recvbuf = stackalloc byte[35565];

        while (!token.IsCancellationRequested)
        {
            // One client at a time
            using var client = _listener.AcceptTcpClient();
            var stream = client.GetStream();

            while (client.Connected)
            {
                var received = stream.Read(recvbuf);
                var actual = recvbuf[..received];
                var commands = JsonSerializer.Deserialize<RenderCommand2[]>(actual, JsonSerializerOptions.Default);
                if (commands is not null)
                    LayoutReceived?.Invoke(commands);
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

    public void SendBatch(ReadOnlySpan<RenderCommand> value)
    {
        var json = JsonSerializer.SerializeToUtf8Bytes(RenderCommand2.FromRenderCommands(value));
        _client.Client.Send(json);
    }
}
