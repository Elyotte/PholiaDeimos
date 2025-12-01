using Godot;
using System;

/// <summary>
/// Responsible to detect user input and execute logic
/// </summary>
public class Deimos : MouseOrKeyboardInputs
{
    // nodepaths
    [Export] NodePath ShootFactoryPath, cursorPath;

    [Export] float speed = 500f;
    ShootComponent shootComponent;
    Cursor cursorComponent;

    public override void _Ready()
    {
        shootComponent = GetNode<ShootComponent>(ShootFactoryPath);
        cursorComponent = GetNode<Cursor>(cursorPath);

        base._Ready();

    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        cursorComponent?.SetCursorDirection(aimInputAxis);

        Vector2 lInputPlayer = new Vector2(Input.GetAxis(INPUTS.LEFT,INPUTS.RIGHT),Input.GetAxis(INPUTS.UP, INPUTS.DOWN));

        GlobalPosition += lInputPlayer.Normalized() * speed * delta;

        if (Input.IsActionJustPressed(INPUTS.FIRE))
        {
            // change hard coded value later
            shootComponent.Shoot(BulletContainer.instance, 200f, cursorComponent.cursorGlobalPosition, 
                cursorComponent.originToCursorDirection);
        }
    }
    

}

