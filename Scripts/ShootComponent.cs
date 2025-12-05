using Godot;
using System;

public class ShootComponent : Node
{
    [Export] public PackedScene bulletPrefab { get; private set; }

    public Bullet Shoot(Node pContainer, Vector2 pVelocity, Vector2 pStartingPosition, int pDamage = 1)
    {
        Bullet lBullet = bulletPrefab.Instance() as Bullet;
        lBullet.SetDamage(pDamage);
        lBullet.SetSpeed(pVelocity);

        // Adding child to scene
        pContainer.AddChild(lBullet);
        
        lBullet.GlobalPosition = pStartingPosition;

        return lBullet;
    }
}