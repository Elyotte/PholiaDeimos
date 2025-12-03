using Godot;
using System;
using System.Diagnostics;

public class BorderCheck : Node2D
{
    protected Vector2 screensize;

    public override void _Ready()
    {
        Init();
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        CheckBorders();
       
    }

    protected virtual void CheckBorders()
    {
        // Check horizontal borders
        if (GlobalPosition.x > screensize.x)
            GlobalPosition = new Vector2(screensize.x, GlobalPosition.y);
        else if (GlobalPosition.x < 0)
            GlobalPosition = new Vector2(0, GlobalPosition.y);

        // Check Vertical borders
        if (GlobalPosition.y > screensize.y)
            GlobalPosition = new Vector2(GlobalPosition.x, screensize.y);
        else if (GlobalPosition.y < 0)
            GlobalPosition = new Vector2(GlobalPosition.x, 0);
    }

    protected bool IsOutOfBounds()
    {
        return GlobalPosition.x > screensize.x ||
               GlobalPosition.x < 0 ||
               GlobalPosition.y > screensize.y ||
               GlobalPosition.y < 0;
    }

    protected void Init()
    {
        screensize = GetViewportRect().Size;
    }

}
