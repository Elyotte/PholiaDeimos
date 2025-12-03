using Godot;
using System;

public class PopCorn : Enemy
{
    [Export] protected float moveOffset = 200f;
    [Export] protected float sinusSpeed = 3.4f;

    public override void _Ready()
    {
        base._Ready();
        m_CurrentState = Pattern;
    }

    virtual protected void Pattern(float delta)
    {
        float timePassed = (float)(0.000001 * Time.GetTicksUsec());
        GlobalPosition += new Vector2(Mathf.Sin(timePassed * sinusSpeed) * moveOffset, m_MoveSpeed) * delta;
    }

}
