using Godot;
using System;

/// <summary>
/// Responsible to add the life component and behavior for all
/// creature that has HP management
/// </summary>
public class LivingObject : BorderCheck
{
    [Export] int lifePoints = 3;
    public LifeContainer life{ get; protected set; }

    [Export] protected NodePath collisionDamage;
    protected LifeComponentCollision m_lifeComponent;
    public override void _Ready()
    {
        life = new LifeContainer(lifePoints);
        // Init Lifecomponents parents ect
        m_lifeComponent = GetNode<LifeComponentCollision>(collisionDamage);
        m_lifeComponent.life = life;
        life.onNoMoreHealth += Dead;
        life.onHealthChanged += DamageFeedback;

        base._Ready();
    }


    virtual public void Dead()
    {
        Visible = false;

    }

    virtual public void DamageFeedback(int pNewhealth) { }
}
