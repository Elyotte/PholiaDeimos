using Godot;
using System;
using System.Drawing.Design;

public class Bullet : Area2D
{
    [Export] Vector2 velocity;
    [Export] public Vector2 direction;
    [Export] int damage;
    [Export] string GroupName;

    public override void _Ready()
    {
        base._Ready();
        Connect(SignalNames.AREA_ENTERED, this, nameof(OnAreaEntered));
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        GlobalPosition += velocity * delta;
    }

    public int GetDamage() => damage;
    public void SetDamage(int pDamage) { damage = pDamage; }
    public void SetSpeed(Vector2 pVelocity) { velocity = pVelocity; }  

    protected void OnAreaEntered(Area2D pArea)
    {
        if (pArea is LifeComponentCollision pLife)
        {
            Disconnect(SignalNames.AREA_ENTERED, this, nameof(OnAreaEntered));
            QueueFree();
        }
        
    }

}
