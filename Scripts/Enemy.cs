using Godot;
using System;

public class Enemy : BorderCheck
{
    [Export] ShootComponent ShootComponent;
    public override void _Ready()
    {

        base._Ready();
    }
}
