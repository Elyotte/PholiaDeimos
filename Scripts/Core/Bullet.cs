using Godot;
using System;
using System.Drawing.Design;

public class Bullet : Area2D
{
    [Export] Vector2 velocity;
    [Export] public Vector2 direction;
    [Export] protected int damage;

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
    public Vector2 GetDirection() => direction;
    public void SetDamage(int pDamage) { damage = pDamage; }
    public void SetSpeed(Vector2 pVelocity) { velocity = pVelocity; }  

    virtual protected void OnAreaEntered(Area2D pArea)
    {
        if (pArea is LifeComponentCollision pLife)
        {
            if (pLife.GetParent() is Pholia)
            {
                // Ignore if pholia is in split state
                return;
            }
            pLife.Damage(new DamageInfo(damage, direction));
            DestroyBullet();
        }
        else if (pArea is Shield)
        {
            DestroyBullet();
        }
        
    }
    
    protected virtual void DestroyBullet()
    {
        Disconnect(SignalNames.AREA_ENTERED, this, nameof(OnAreaEntered));
        QueueFree();
    }

}
