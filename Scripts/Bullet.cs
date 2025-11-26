using Godot;
using System;

public class Bullet : Area2D
{
    [Export] float speed;
    [Export] Vector2 direction;
    [Export] int damage;

    public override void _Ready()
    {
        base._Ready();
        Connect(SignalNames.AREA_ENTERED, this, nameof(OnAreaEntered));
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        GlobalPosition += direction.Normalized() * speed * delta;
    }

    protected void OnAreaEntered(Area2D pArea)
    {
        QueueFree();
    }

    public override void _ExitTree()
    {
        Disconnect(SignalNames.AREA_ENTERED,this, nameof(OnAreaEntered));
        base._ExitTree();
    }

}
