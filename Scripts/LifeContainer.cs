using Godot;
using System;

/// <summary>
/// Used to store life amount and kill enemies/entities when needed
/// </summary>
public class LifeContainer
{
    int maxLife = 10;
    int currentLife;
    public event Action onNoMoreHealth;
    public event Action<int> onHealthChanged; // args is new health

    public LifeContainer(int maxLife = 10)
    {
        this.maxLife = maxLife;
        this.currentLife = maxLife;
    }

    public void Damage(int pAmount)
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
