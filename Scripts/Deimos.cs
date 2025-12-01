using Godot;
using System;
using System.Drawing.Printing;

/// <summary>
/// Responsible to detect user input and execute logic
/// </summary>
public class Deimos : MouseOrKeyboardInputs
{
    // nodepaths
    [Export] NodePath ShootFactoryPath, cursorPath, lifePTH;

    [Export] float speed = 500f;
    ShootComponent shootComponent;
    Cursor cursorComponent;
    LifeComponentCollision life;

    public Action<float> CurrentState; // arg is delta time

    public override void _Ready()
    {
        shootComponent = GetNode<ShootComponent>(ShootFactoryPath);
        cursorComponent = GetNode<Cursor>(cursorPath);
        life = GetNode<LifeComponentCollision>(lifePTH);

        base._Ready();

        life.onNoMoreHealth += Death;

        SetModeNormal();
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        CurrentState?.Invoke(delta);
    }
    
    private void SetModeNormal()
    {
        CurrentState = NormalMode;
    }
    private void NormalMode(float delta)
    {
        cursorComponent?.SetCursorDirection(aimInputAxis);

        Vector2 lInputPlayer = new Vector2(Input.GetAxis(INPUTS.LEFT, INPUTS.RIGHT), Input.GetAxis(INPUTS.UP, INPUTS.DOWN));

        GlobalPosition += lInputPlayer.Normalized() * speed * delta;

        if (Input.IsActionJustPressed(INPUTS.FIRE))
        {
            // change hard coded value later
            shootComponent.Shoot(BulletContainer.instance, 200f, cursorComponent.cursorGlobalPosition,
                cursorComponent.originToCursorDirection);
        }

        if (Input.IsActionJustPressed(INPUTS.SPLIT))
        {
            SetModeSplit();
        }
    }

    private void SetModeSplit()
    {
        CurrentState = SplittedMode;
    }
    private void SplittedMode(float delta)
    {

    }

    private void Death()
    {
        Visible = false;
        CurrentState = null;
    }
}

