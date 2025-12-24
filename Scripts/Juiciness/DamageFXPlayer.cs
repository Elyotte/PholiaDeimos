using Godot;
using System;

public class DamageFXPlayer : Node
{
    [Export] NodePath lifeCompoPtr;
    LifeComponentCollision life;

    public override void _Ready()
    {
        CallDeferred(nameof(Start));
    }

    void Start()
    {
        life = GetNode<LifeComponentCollision>(lifeCompoPtr);
        life.onDamage += CallDamage;
    }

    void CallDamage(DamageInfo info)
    {
        GameManager.Instance?.pool.PlayFx(life.GlobalPosition, info);
    }

}
