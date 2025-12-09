using Godot;
using System;

public class ShieldedEnemy : PatternBasedEnemy
{
    Vector2 targetPos;

    public override void _Ready()
    {
        base._Ready();
        m_Margin = 200f;
        targetPos = (screensize * .5f) + (Vector2.Up * m_Margin);
    }
    protected override void Pattern(float delta)
    {
        if (IsOutOfBounds(out Vector2 pBounds))
        {
            GlobalPosition += -pBounds.Normalized() * m_MoveSpeed * delta;
        }
        Vector2 distanceToTarget = targetPos - GlobalPosition;
        
        if(distanceToTarget.Length() >= 1f)
        {
            GlobalPosition += distanceToTarget.Normalized() * m_MoveSpeed * delta;
        }
        else
        {
            m_CurrentState = SecondaryPattern;
        }

    }

    protected void SecondaryPattern(float delta)
    {
        HorizontalSine(delta);
    }
}
