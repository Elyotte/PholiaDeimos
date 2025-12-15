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
        float scaleFactor = 1.5f;
        float damageAnimDuration = .1f;
        float returnNormal = .1f;

        scaleTween.TweenProperty(m_sprite, "scale", baseScale * scaleFactor, damageAnimDuration)
            .SetTrans(Tween.TransitionType.Cubic)
            .SetEase(Tween.EaseType.OutIn);

        scaleTween.TweenProperty(m_sprite, "scale", baseScale, returnNormal)
            .SetTrans(Tween.TransitionType.Back)
            .SetEase(Tween.EaseType.Out);
    }
}
