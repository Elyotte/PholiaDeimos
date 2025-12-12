using Godot;
using System;

public class EnemySpriteTweening : SpriteTweening
{
    Vector2 defaultScale;

    public override void Start()
    {
        base.Start();
        defaultScale = m_sprite.Scale;

    }

    public override void DamageAnimation()
    {
        base.DamageAnimation();
        SceneTreeTween scaleTween = GetTree().CreateTween();
        Vector2 baseScale = defaultScale;
        float scaleFactor = 1.2f;
        float damageAnimDuration = .2f;

        scaleTween.TweenProperty(m_sprite, "scale", baseScale * scaleFactor, damageAnimDuration)
            .SetTrans(Tween.TransitionType.Elastic)
            .SetEase(Tween.EaseType.Out);

        scaleTween.TweenProperty(m_sprite, "scale", baseScale, damageAnimDuration)
            .SetTrans(Tween.TransitionType.Bounce)
            .SetEase(Tween.EaseType.OutIn);
    }
}
