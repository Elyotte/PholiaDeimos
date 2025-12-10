using Godot;
using System;

public class PopCorn : PatternBasedEnemy
{
    [Export] protected float moveOffset = 200f;
    [Export] protected float sinusSpeed = 3.4f;

    override protected void Pattern(float delta)
    {
        GlobalPosition += ToDeimosVector().Normalized() * m_MoveSpeed * delta;
    }
    
}
