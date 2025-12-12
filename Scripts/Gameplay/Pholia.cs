using Godot;
using System;

public class Pholia : MouseOrKeyboardInputs
{
    [Export] NodePath deimosPath, hpCollisionPath;
    [Export] float moveSpeed = 500f;
    Deimos deimos;
    CollisionShape2D collision;
    Action<float> currentAction;
    public bool AskResplit { get; private set; } = false;

    public override void _Ready()
    {
        collision = GetNode<CollisionShape2D>(hpCollisionPath);
        deimos = GetNode<Deimos>(deimosPath);
        deimos.onSplitAnimFinished += SetModeMove;
        deimos.onResplit += SetModeDeactivated;

        base._Ready();
        Visible = false;
        SetModeDeactivated();
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        currentAction?.Invoke(delta);
    }

    private void SetModeDeactivated() { 
        currentAction = null; 
        AskResplit = false;
        collision.Disabled = true;
    }

    private void SetModeMove()
    {
        collision.Disabled = false;
        currentAction = Move;
    }
    private void Move(float delta)
    {
        GlobalPosition += aimInputAxis.Normalized() * delta * moveSpeed;
        AskResplit = Input.IsActionPressed(INPUTS.FIRE);
    }

    public override void _ExitTree()
    {
        deimos.onResplit -= SetModeDeactivated;
        deimos.onSplitAnimFinished -= SetModeMove;
        base._ExitTree();
    }
}
