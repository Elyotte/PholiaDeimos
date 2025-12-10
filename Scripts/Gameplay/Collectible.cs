using Godot;
using System;

public class Collectible : Area2D
{
    [Export] public int HealAmount = 2;

    public override void _Ready()
    {
        Connect(SignalNames.AREA_ENTERED, this, nameof(OnAreaEntered));
    }

    void OnAreaEntered(Area2D area)
    {
        if (area is Pholia || area is Deimos)
        {
            Disapear();
        }
    }

    void Disapear()
    {
        QueueFree();
    }

    public override void _ExitTree()
    {
        Disconnect(SignalNames.AREA_ENTERED, this, nameof(OnAreaEntered));
        base._ExitTree();
    }

}
