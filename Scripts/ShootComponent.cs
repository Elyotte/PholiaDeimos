using Godot;
using System;

public class ShootComponent : Node
{
    [Export] public PackedScene bulletPrefab { get; private set; }

    public Bullet Shoot(Node pContainer, float pSpeed, Vector2 pPos, Vector2 pDirection, int pDamage)
    {
        Bullet lBullet = bulletPrefab.Instance() as Bullet;
        lBullet.SetDamage(pDamage);
        lBullet.direction = pDirection.Normalized(); // Force normalized value
        lBullet.SetSpeed(pSpeed);

        // Adding child to scene
        pContainer.AddChild(lBullet);
        
        lBullet.GlobalPosition = pPos;

        return lBullet;
    }
}
