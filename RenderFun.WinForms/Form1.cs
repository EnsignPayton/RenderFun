using RenderFun.Shared;

namespace RenderFun.WinForms;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
        SetStyle(ControlStyles.ResizeRedraw, true);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var commands = Clay.GetFakeCommands(e.ClipRectangle.ToClayDimensions());
        Renderer.Render(e.Graphics, commands);
    }
}
