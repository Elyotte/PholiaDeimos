using Godot;
using System;

public class ExternalVelocity : BorderCheck
{
    protected Vector2 m_ExternalVel;
    protected float m_DepletionRate = 33f;

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        GlobalPosition += m_ExternalVel * delta;
        DepleteVelocity(delta);
    }

    public void DepleteVelocity(float fixedDelta)
    {
        m_ExternalVel = new Vector2(
                Mathf.MoveToward(m_ExternalVel.x, 0, m_DepletionRate * fixedDelta), 
                Mathf.MoveToward(m_ExternalVel.y, 0, m_DepletionRate * fixedDelta)
            );
    }

    public void AddForce(Vector2 force)
    {
        m_ExternalVel += force;
    }
}
