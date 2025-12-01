using Godot;
using System;

/// <summary>
/// Used to apply damage to a life container instance
/// </summary>
public class LifeComponentCollision  : Area2D
{
    public LifeContainer life;

    public override void _Ready()
    {
        Connect(SignalNames.AREA_ENTERED, this, nameof(OnAreaEntered));
    }

    void OnAreaEntered(Area2D area)
    {
        if (life ==null)
        {
            GD.PrintErr("No life container instance set", this);
            return;
        }

        if(area is Bullet pBullet)
        {
             life.Damage(pBullet.GetDamage());
        }
    }

}