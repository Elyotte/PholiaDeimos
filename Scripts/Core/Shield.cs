using Godot;
using System;

public class Shield : Area2D
{
    public override void _Ready()
    {
        Connect(SignalNames.AREA_ENTERED, this, nameof(OnAreaEntered));
    }

    void OnAreaEntered(Area2D area)
    {
        if (area is Bullet lBullet)
        {
            
        }
    }

    public override void _ExitTree()
    {
        Disconnect(SignalNames.AREA_ENTERED,this, nameof(OnAreaEntered));
        base._ExitTree();
    }
}
