using Godot;
using System;

public class PatternBasedEnemy : Enemy
{
    protected void HorizontalSine(float delta, float sinusSpeed = 3.4f, float moveOffset = 200f)
    {
        float timePassed = (float)(0.000001 * Time.GetTicksUsec());
        GlobalPosition += new Vector2(Mathf.Sin(timePassed * sinusSpeed) * moveOffset, m_MoveSpeed) * delta;
    }
}
