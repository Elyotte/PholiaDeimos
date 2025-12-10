using Godot;
using System;

public class ShootingPopCorn : PopCorn
{
    [Export] NodePath shootPath = "ShootComponent", shootPosPath = "Position2D";
    ShootComponent shoot;
    Position2D pos;
    [Export] float fireRate = .3f;
    float elapsed;

    [Export] float shootSpeed = 50f;
    [Export] int shootDamage = 3;

    public override void _Ready()
    {
        pos = GetNode<Position2D>(shootPosPath);
        shoot = GetNode<ShootComponent>(shootPath);
        base._Ready();

        m_CurrentState += Shoot;
    }

    protected override void Pattern(float delta)
    {
        DotCrossedSine(delta);
    }

    virtual protected void Shoot(float delta)
    {
        if (elapsed <= 0)
        {
            shoot.Shoot(GameManager.bulletContainer, Vector2.Down * (m_MoveSpeed + shootSpeed), pos.GlobalPosition, shootDamage);
            elapsed += fireRate;
        } 
        elapsed -= delta; 
    }
}
