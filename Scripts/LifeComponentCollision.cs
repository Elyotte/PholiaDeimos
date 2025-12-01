    using Godot;
using System;

public class LifeComponentCollision  : Area2D
{
    [Export] int maxLife = 10;
    public int currentLife { get; private set; }
    public event Action onNoMoreHealth;
    public event Action<int> onHealthChanged; // args is new health

    public override void _Ready()
    {
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
        IsAlive();
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
