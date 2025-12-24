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
    public event Action<DamageInfo> onDamage;
    public event Action onHeal;

    public override void _Ready()
    {
        if (owner == null) owner = GetParent<Node2D>();
        Connect(SignalNames.AREA_ENTERED, this, nameof(OnAreaEntered));
        currentLife = maxLife;
    }

    protected virtual void OnAreaEntered(Area2D area)
    {
        // Happen when two life component collides
        if (area is LifeComponentCollision other)
        {
            // Deomos is damaged by the enemies he touch in normal state only
            if (other.GetParent() is Enemy && GetParent() is Deimos lDeimos)
            {
                Damage(1);
            }
            // Pholia damage the enemies she touch
            else if (other.GetParent() is Pholia && GetParent() is Enemy)
            {
                Damage(1);
            }
        }
        else if (area is Collectible lCollectible)
        {
            Heal(lCollectible.HealAmount);
        }
    }

    public virtual void Heal(int pHealAmount)
    {
        currentLife += pHealAmount;
        onHealthChanged?.Invoke(currentLife);
        onHeal?.Invoke();
        IsAlive();
    }

    public virtual void Damage(DamageInfo pInfo)
    {
        currentLife -= pInfo.amount;
        onHealthChanged?.Invoke(currentLife);
        onDamage?.Invoke(pInfo);
        IsAlive();
    }

    public virtual void Damage(int pAmount)
    {
        Damage(new DamageInfo(pAmount, Vector2.Zero));
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

public struct DamageInfo
{
    public DamageInfo(int amount, Vector2 velocity)
    {
        this.amount = amount;
        this.bulletVelocity = velocity;
    }

    public int amount;
    public Vector2 bulletVelocity;
}