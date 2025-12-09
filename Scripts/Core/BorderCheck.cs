using Godot;
using System;
using System.Diagnostics;

public class BorderCheck : Node2D
{
    protected Vector2 screensize;
    protected float m_Margin = -20f;
    public override void _Ready()
    {
        Init();
        base._Ready();
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        CheckBorders();
       
    }

    protected virtual void CheckBorders()
    {
        // Check horizontal borders
        if (GlobalPosition.x > screensize.x - m_Margin)
            GlobalPosition = new Vector2(screensize.x - m_Margin, GlobalPosition.y);
        if (GlobalPosition.x <= m_Margin)
            GlobalPosition = new Vector2(m_Margin, GlobalPosition.y);

        // Check Vertical borders
        if (GlobalPosition.y > screensize.y - m_Margin)
            GlobalPosition = new Vector2(GlobalPosition.x, screensize.y - m_Margin);
        else if (GlobalPosition.y < m_Margin)
            GlobalPosition = new Vector2(GlobalPosition.x, m_Margin);
    }

    protected bool IsOutOfBounds(out Vector2 BoundOut)
    {
        BoundOut = GlobalPosition;

        // axis out of bounds computation
        int XAxis = GlobalPosition.x <= m_Margin ? -1 : 0;
        if(XAxis ==0) XAxis = GlobalPosition.x >= screensize.x - m_Margin? 1 : 0;

        int YAxis = GlobalPosition.y <= m_Margin ? -1 : 0;
        if(YAxis ==0) YAxis = GlobalPosition.y >= screensize.y - m_Margin ? 1 : 0;

        BoundOut = new Vector2(XAxis, YAxis);

        return (BoundOut.x != 0 || BoundOut.y != 0);
    }

    protected void Init()
    {
        screensize = GetViewportRect().Size;
    }

}
