using Godot;
using System;
using System.Drawing.Text;
using System.Threading.Tasks;

public class Enemy : BorderCheck
{
    [Export] protected int m_ScoreOnDeath = 10;
    [Export] protected float m_MoveSpeed = 200f;
    [Export] NodePath lifeComponentPath = "Area2D", rendererPath = "Sprite";
    LifeComponentCollision lifeComponent;
    Sprite sprite;

    protected Action<float> m_CurrentState;

    public override void _Ready()
    {
        sprite = GetNode<Sprite>(rendererPath);
        lifeComponent = GetNode<LifeComponentCollision>(lifeComponentPath);
        base._Ready();
        lifeComponent.onNoMoreHealth += Death;

        m_CurrentState = Pattern;
    }

    public void SetVoid() { m_CurrentState = null; }
    public void SetMove() { m_CurrentState = Pattern; }

    public override void _Process(float delta)
    {
        base._Process(delta);
        m_CurrentState?.Invoke(delta);
    }

    protected override void CheckBorders()
    {
        if (IsOutOfBounds(out Vector2 lBound))
        {
            // Only destroy the enemy when the out of bounds is in the lower vertical part of the screen
            if (lBound.y == 1)
            {
                QueueFree();
            }
        }
    }

    virtual protected void Pattern(float delta) { }

    private async void Death()
    {
        lifeComponent.DisconnectCollisions();

        GameManager.Instance?.AddScore(m_ScoreOnDeath);

        m_CurrentState = null;

        float deathScale = 1.3f;
        float inTime = .22f;
        SceneTreeTween tween = CreateTween();
        tween.TweenProperty(sprite, "scale", Vector2.One * deathScale, inTime)
            .SetTrans(Tween.TransitionType.Back)
            .SetEase(Tween.EaseType.In);
        await ToSignal(tween, SignalNames.TWEEN_FINISHED);
        QueueFree();
        
    }
}
