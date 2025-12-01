using Godot;
using System;

public class Enemy : BorderCheck
{
    [Export] NodePath lifeComponentPath;
    LifeComponentCollision lifeComponent;

    public override void _Ready()
    {
        lifeComponent = GetNode<LifeComponentCollision>(lifeComponentPath);
        base._Ready();
        lifeComponent.onNoMoreHealth += Death;
    }

    private void Death()
    {
        QueueFree();
    }
}
