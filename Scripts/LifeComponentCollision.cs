using Godot;
using System;

public class LifeComponentCollision  : Area2D
{
    [Export] int maxLife = 10;
    int currentLife;
    public event Action onNoMoreHealth;
    public event Action<int> onHealthChanged; // args is new health

    public override void _Ready()
    {
        Connect(SignalNames.AREA_ENTERED, this, nameof(OnAreaEntered));
        currentLife = maxLife;
    }

    void OnAreaEntered(Area2D area)
    {
        if(area is Bullet pBullet)
        {
             Damage(pBullet.GetDamage());
        }
    }

    private void Damage(int pAmount)
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
}
