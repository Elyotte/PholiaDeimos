using Godot;
using System;
using System.ComponentModel;
using System.Drawing.Printing;

/// <summary>
/// Responsible to detect user input and execute logic
/// </summary>
public class Deimos : MouseOrKeyboardInputs
{
    // Player stats
    [Category("Deimos' stats")]
    [Export] float speed = 500f;
    [Export] float reducedSpeed = 200f;

    // nodepaths
    [Category("NodePath")]
    [Export] NodePath ShootFactoryPath, cursorPath, lifePath, pholiaPath, renderPath;

    // Component
    ShootComponent shootComponent;
    Cursor cursorComponent;
    LifeComponentCollision life;
    AnimatedSprite renderer;
    Pholia pholia;

    // Statemachine
    public Action<float> CurrentState; // arg is delta time

    // AnimatedSprite anims names
    string A_Whole = "default";
    string A_Splited = "UnGhosted";

    public override void _Ready()
    {
        shootComponent = GetNode<ShootComponent>(ShootFactoryPath);
        cursorComponent = GetNode<Cursor>(cursorPath);
        life = GetNode<LifeComponentCollision>(lifePath);
        pholia = GetNode<Pholia>(pholiaPath);
        renderer = GetNode<AnimatedSprite>(renderPath);

        base._Ready();

        life.onNoMoreHealth += Death;

        SetModeNormal();
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        CurrentState?.Invoke(delta);
    }
    
    private void Move(float delta, float dynamicSpeed)
    {
        Vector2 lInputPlayer = new Vector2(Input.GetAxis(INPUTS.LEFT, INPUTS.RIGHT), Input.GetAxis(INPUTS.UP, INPUTS.DOWN));
        GlobalPosition += lInputPlayer.Normalized() * dynamicSpeed * delta;
    }

    private void SetModeNormal()
    {
        CurrentState = NormalMode;
    }
    private void NormalMode(float delta)
    {
        renderer.Play(A_Whole);
        cursorComponent?.SetCursorDirection(aimInputAxis);

        Move(delta, speed);

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
        renderer.Play(A_Splited);
        CurrentState = SplittedMode;
        CallPholia();
        cursorComponent.Visible = false;
    }
    private void SplittedMode(float delta)
    {
        Move(delta, reducedSpeed);

    }

    private void Death()
    {
        Visible = false;
        CurrentState = null;
    }

    private async void CallPholia()
    {
        pholia.Visible = true;
        
    }
}
