using Godot;
using System;

public class LifeBar : ProgressBar
{
    [Export] NodePath characterToMonitorCollision;
    LifeComponentCollision characterLife;

    public override void _Ready()
    {
        if (characterToMonitorCollision == null)
            return;
        characterLife = GetNode<LifeComponentCollision>(characterToMonitorCollision);
        CallDeferred(nameof(Start));
    }

    void Start()
    {
        characterLife.onHealthChanged += UpdateLife;
        MaxValue = characterLife.maxLife;
        Value = characterLife.currentLife;
    }

    public void UpdateLife(int pNewVal)
    {
        Value = Mathf.Clamp(pNewVal, 0, (int)MaxValue);
    }



}
