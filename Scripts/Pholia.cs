using Godot;
using System;

public class Pholia : LivingObject
{
    [Export] NodePath cursorPath;
    private Sprite cursor;
    private Vector2 moveInput;

    private float distanceToCursot = 100f;
   
    public override void _Ready()
    {
        if (cursorPath != null)
        {
            cursor = GetNode<Sprite>(cursorPath);
        }

    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        ComputeInputs();

        if (cursor != null) {
            cursor.Position = moveInput * distanceToCursot;
        }

    }

    public void ComputeInputs()
    {
        // Mouse Inputs detection
        Vector2 lDirectionToCursor = (GetGlobalMousePosition() - GlobalPosition).Normalized();
        moveInput = lDirectionToCursor;

        // Keyboard/gamepad inputs detections
        Vector2 lAxis = new Vector2(
                                    Input.GetAxis(INPUTS.AIM_LEFT, INPUTS.AIM_RIGHT),
                                    Input.GetAxis(INPUTS.AIM_UP, INPUTS.AIM_DOWN));

        moveInput = lAxis.Normalized();
    }
}
