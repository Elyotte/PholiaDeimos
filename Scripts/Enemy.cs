using Godot;
using System;
using System.Drawing.Text;
using System.Threading.Tasks;

public class Enemy : BorderCheck
{
    [Export] NodePath lifeComponentPath, rendererPath;
    LifeComponentCollision lifeComponent;
    Sprite sprite;

    protected Action m_CurrentState;

    public override void _Ready()
    {
        sprite = GetNode<Sprite>(rendererPath);
        lifeComponent = GetNode<LifeComponentCollision>(lifeComponentPath);
        base._Ready();
        lifeComponent.onNoMoreHealth += Death;
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        m_CurrentState?.Invoke();
    }

    private async void Death()
    {
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
