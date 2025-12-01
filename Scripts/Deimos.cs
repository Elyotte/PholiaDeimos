using Godot;
using System;

/// <summary>
/// Responsible to detect user input and execute logic
/// </summary>
public class Deimos : MouseOrKeyboardInputs
{
    [Export] float speed = 500f;
    ShootComponent shootComponent;
    [Export] NodePath ShootFactoryPath;

    public override void _Ready()
    {
        if (ShootFactoryPath != null)
        {
            shootComponent = GetNode<ShootComponent>(ShootFactoryPath);
        }

        base._Ready();

    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        Vector2 lInputPlayer = new Vector2(Input.GetAxis(INPUTS.LEFT,INPUTS.RIGHT),Input.GetAxis(INPUTS.UP, INPUTS.DOWN));

        GlobalPosition += lInputPlayer.Normalized() * speed * delta;

        if (Input.IsActionJustPressed(INPUTS.FIRE))
        {
            shootComponent.Shoot(BulletContainer.instance, 200f, GlobalPosition + new Vector2(0, -100), Vector2.Up, 2);
        }
    }
    

}

