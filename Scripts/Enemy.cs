using Godot;
using System;

public class Enemy : LivingObject
{
    [Export] ShootComponent ShootComponent;
    public override void _Ready()
    {

        base._Ready();
    }


    public override void Dead()
    {

        QueueFree();
    }
}
