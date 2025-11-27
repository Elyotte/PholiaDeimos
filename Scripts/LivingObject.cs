using Godot;
using System;

public class LivingObject : BorderCheck
{
    [Export] protected NodePath collisionDamage;
    protected LifeComponentCollision m_lifeComponent;
    public override void _Ready()
    {
        // Init Lifecomponents parents ect
        m_lifeComponent = GetNode<LifeComponentCollision>(collisionDamage);
        m_lifeComponent.onNoMoreHealth += Dead;

        base._Ready();
    }


    virtual public void Dead()
    {
        Visible = false;

    }

}
