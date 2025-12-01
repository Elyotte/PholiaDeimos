using Godot;
using System;
using System.Drawing.Design;

public class Bullet : Area2D
{
    [Export] float speed;
    [Export] public Vector2 direction;
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

    public int GetDamage() => damage;
    public void SetDamage(int pDamage) { damage = pDamage; }
    public void SetSpeed(float pSpeed) { speed = pSpeed; }  

    protected void OnAreaEntered(Area2D pArea)
    {
        Disconnect(SignalNames.AREA_ENTERED, this, nameof(OnAreaEntered));
        QueueFree();
    }

}
