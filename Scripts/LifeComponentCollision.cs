    using Godot;
using System;
using System.Threading.Tasks;

/// <summary>
/// An area2D object that you attach to a parent node that you want to apply damage to.
/// This class is responsible for storing life, managing damage and damage animation
/// BUT NOT DEATH, when the component get to 0 HP an event is sent, it is the responsability
/// to the parent to manage their own death, it ensure correct things happens
/// </summary>
public class LifeComponentCollision  : Area2D
{
    [Export] Node2D owner;
    [Export] int maxLife = 10;
    public int currentLife { get; private set; }
    public event Action onNoMoreHealth;
    public event Action<int> onHealthChanged; // args is new health

    public override void _Ready()
    {
        owner = GetParent<Node2D>();
        Connect(SignalNames.AREA_ENTERED, this, nameof(OnAreaEntered));
        currentLife = maxLife;
    }

    protected virtual void OnAreaEntered(Area2D area)
    {
        if(area is Bullet pBullet)
        {
             Damage(pBullet.GetDamage());
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
        owner.Modulate = Colors.Red;
        await ToSignal(GetTree().CreateTimer(.3f), "timeout");
        owner.Modulate = Colors.White;
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

    public override void _ExitTree()
    {
        Disconnect(SignalNames.AREA_ENTERED, this, nameof(OnAreaEntered));
        base._ExitTree();
    }
}
