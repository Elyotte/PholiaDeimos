using Godot;
using System;

public class LinkedLifeComponent : LifeComponentCollision
{
    [Export] NodePath lifeComponentToApplyDamagePath;
    LifeComponentCollision LifeComponentToApplyDamage;
    

    public override void _Ready()
    {
        base._Ready();
        CallDeferred(nameof(Init));
    }

    void Init()
    {
        LifeComponentToApplyDamage = GetNode<LifeComponentCollision>(lifeComponentToApplyDamagePath);
    }


    public override void Heal(int pHealAmount)
    {
        if (LifeComponentToApplyDamage == null) return;
        LifeComponentToApplyDamage?.Heal(pHealAmount);
    }
    public override void Damage(int pAmount)
    {
        if (LifeComponentToApplyDamage == null) return;
        LifeComponentToApplyDamage.Damage(pAmount);
    }
}
