using Godot;
using System;

public class LinkedLifeComponent : LifeComponentCollision
{
    [Export] LifeComponentCollision LifeComponentToApplyDamage;

    public override void _Ready()
    {
        base._Ready();
    }

    public override void Damage(int pAmount)
    {
        
    }
}
