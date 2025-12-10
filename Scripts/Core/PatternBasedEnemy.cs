using Godot;
using System;

public class PatternBasedEnemy : Enemy
{
    private float elapsedTimeInPattern;
    Deimos _Deimos;

    public override void _Ready()
    {
        base._Ready();
        CallDeferred(nameof(Init));
    }

    void Init()
    {
        _Deimos = GameManager.deimos;
    }

    protected Vector2 ToDeimosVector()
    {
        if (_Deimos == null) return Vector2.Zero;
        return _Deimos.GlobalPosition - GlobalPosition;
    }

    protected void HorizontalSine(float delta, float sinusSpeed = 3.4f, float moveOffset = 200f)
    {
        elapsedTimeInPattern += delta;
        GlobalPosition += new Vector2(Mathf.Sin(elapsedTimeInPattern * sinusSpeed) * moveOffset, 0) * delta;
    }

    protected void DotCrossedSine(float delta, float sinusSpeed = 3.4f, float moveOffset = 200f)
    {
        elapsedTimeInPattern += delta;

        Vector2 directionToDeimos = ToDeimosVector().Normalized();
        Vector2 perpendiculatDirection = new Vector2(-directionToDeimos.y, -directionToDeimos.x);

        Vector2 movement = directionToDeimos * m_MoveSpeed * delta;
        movement += perpendiculatDirection * Mathf.Sin((elapsedTimeInPattern * sinusSpeed) * moveOffset * delta);

        GlobalPosition += movement;
    }
}
