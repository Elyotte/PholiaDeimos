using Godot;
using System;

public class Pholia : MouseOrKeyboardInputs
{
    [Export] NodePath deimosPath;
    [Export] float moveSpeed = 500f;
    Deimos deimos;
    Action<float> currentAction;
    public bool AskResplit { get; private set; } = false;

    public override void _Ready()
    {
        deimos = GetNode<Deimos>(deimosPath);
        deimos.onSplit += SetModeMove;
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
    }

    private void SetModeMove()
    {
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
        deimos.onSplit -= SetModeMove;
        base._ExitTree();
    }
}
