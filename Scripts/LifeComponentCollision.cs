    using Godot;
using System;
using System.Threading.Tasks;

/// <summary>
/// An area2D object that you attach to a parent node that you want to apply damage to.
/// This class is responsible for storing life, managing damage and damage animation
/// BUT NOT DEATH, when the component get to 0 HP an event is sent, it is the responsability
/// to the parent to manage their own death, it ensure correct things happens
/// 
/// WARNING : The parent of this node should be the living object you want implement health
///  to
/// </summary>
public class LifeComponentCollision  : Area2D
{
    [Export] Node2D owner;
    [Export] public int maxLife { get; private set; } = 10;
    public int currentLife { get; private set; }
    public event Action onNoMoreHealth;
    public event Action<int> onHealthChanged; // args is new health
    public event Action onDamage;

    public override void _Ready()
    {
        if (owner == null) owner = GetParent<Node2D>();
        Connect(SignalNames.AREA_ENTERED, this, nameof(OnAreaEntered));
        currentLife = maxLife;
    }

    protected virtual void OnAreaEntered(Area2D area)
    {
        if(area is Bullet pBullet)
        {
            if (GetParent() is Deimos lDeimos && lDeimos.playerState == PlayerState.Splitted)
            {
                 
            }
            else
                Damage(pBullet.GetDamage());
        }
        // Happen when two life component collides
        else if (area is LifeComponentCollision other)
        {
            // Deomos is damaged by the enemies he touch in normal state only
            if (other.GetParent() is Enemy && GetParent() is Deimos lDeimos)
            {
                Damage(1);
            }
            // Pholia damage the enemies she touch
            else if (other.GetParent() is Enemy && GetParent() is Pholia)
            {
                other.Damage(1);
            }
        }
    }

    public virtual void Damage(int pAmount)
    {
        currentLife -= pAmount;
        onHealthChanged?.Invoke(currentLife);
        DamageAnimation();
        IsAlive();
    }

    public async virtual void DamageAnimation()
    {
        SceneTreeTween damageTween = CreateTween();
        float lToRedTime = .2f;
        float lToWhiteTime = .1f;
        damageTween.TweenProperty(owner, "modulate", Colors.Red, lToRedTime)
            .SetTrans(Tween.TransitionType.Bounce)
            .SetEase(Tween.EaseType.InOut);
        damageTween.TweenProperty(owner, "modulate", Colors.White, lToRedTime)
            .SetEase(Tween.EaseType.InOut);
        onDamage?.Invoke();
    }

    private bool IsAlive()
    {
        if (currentLife <= 0)
        {
            onNoMoreHealth?.Invoke();
            return false;
        }

        currentLife = Mathf.Clamp(currentLife, 0, maxLife);
        return true;
    }

    public void DisconnectCollisions() {
        if (IsConnected(SignalNames.AREA_ENTERED, this, nameof(OnAreaEntered)))
        {
            Disconnect(SignalNames.AREA_ENTERED, this, nameof(OnAreaEntered));
        }

    }

    public override void _ExitTree()
    {
        DisconnectCollisions();
        base._ExitTree();
    }
}
