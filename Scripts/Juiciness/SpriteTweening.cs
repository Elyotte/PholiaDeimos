using Godot;
using System;

public class SpriteTweening : Node
{
    [Export] public NodePath spriteToAnimatepth, lifecomppth;
    protected Sprite m_sprite;
    protected LifeComponentCollision m_life;

    public override void _Ready()
    {
        CallDeferred(nameof(Start));    
    }

    public virtual void Start()
    {
        m_life = GetNode<LifeComponentCollision>(lifecomppth);
        m_sprite = GetNode<Sprite>(spriteToAnimatepth);

        m_life.onDamage += DamageAnimation;
        m_life.onHeal += HealAnimation;
    }

    public async virtual void HealAnimation()
    {
        SceneTreeTween healTween = CreateTween();
        float lToGreenTime = .3f;
        float lToWhiteModulate = .7f;
        healTween.TweenProperty(m_sprite, "modulate", Colors.Green, lToGreenTime)
            .SetTrans(Tween.TransitionType.Quint)
            .SetEase(Tween.EaseType.InOut);
        healTween.TweenProperty(m_sprite, "modulate", Colors.White, lToWhiteModulate)
            .SetEase(Tween.EaseType.InOut);

    }

    public virtual void DamageAnimation(DamageInfo pDamage)
    {
        SceneTreeTween damageTween = CreateTween();
        float lToRedTime = .2f;
        float lToWhiteTime = .1f;

        damageTween.TweenProperty(m_sprite, "modulate", Colors.Red, lToRedTime)
            .SetTrans(Tween.TransitionType.Bounce)
            .SetEase(Tween.EaseType.InOut);
        damageTween.TweenProperty(m_sprite, "modulate", Colors.White, lToWhiteTime)
            .SetEase(Tween.EaseType.InOut);
    }

}
