using RenderFun.Shared;

namespace RenderFun.WinForms;

public partial class Form1 : Form
{
    private readonly ClayNetServer _server = new();
    private RenderCommand2[] _pendingRender = [];

    public Form1()
    {
        InitializeComponent();
        SetStyle(ControlStyles.ResizeRedraw, true);

        _server.LayoutReceived += OnLayoutReceived;
        _server.Start();
    }

    private void OnLayoutReceived(RenderCommand2[] commands)
    {
        Console.WriteLine("Received Real Commands");
        Interlocked.Exchange(ref _pendingRender, commands);
        Invalidate();
    }

    protected override void OnClosed(EventArgs e)
    {
        _server.Dispose();
        base.OnClosed(e);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var commands = _pendingRender;

        if (commands.Any())
        {
            Renderer.Render(e.Graphics, commands);
        }
        else
        {
            Renderer.Render(e.Graphics, RenderCommand2.FromRenderCommands(
                Clay.GetFakeCommands(e.ClipRectangle.ToClayDimensions())));
        }
    }
}
