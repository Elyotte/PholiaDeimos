using Godot;
using System;

public class PlayerCursorBehavior : LivingObject
{
    // Nodepath
    [Export] NodePath cursorPath = "CursorComponent";

    private Vector2 moveInput;
    private Cursor cursor;

    bool bUseMouse = false;

    Vector2 lastFrameMousePos;
    float deadzone = .5f;

    public override void _Ready()
    {
        cursor = GetNode<Cursor>(cursorPath);
        base._Ready();
    }

    public override void _Process(float delta)
    {
        SelectMoveMode();

        base._Process(delta);

        SelectMoveMode();
        cursor.SetCursorFromOriginPosition(moveInput);

        lastFrameMousePos = GetGlobalMousePosition();
    }

    public void SelectMoveMode()
    {
        // keyboard
        if (bUseMouse)
        {
            // if keyboard inputs are detected then just pass to keyboard and hide cursor
            bUseMouse = KeyboardInputs() == Vector2.Zero;
            moveInput = MouseInputs();
        }
        else
        {
            bUseMouse = GetMouseDelta().Length() >= deadzone;
            moveInput = KeyboardInputs();

        }
    }

    public Vector2 GetMouseDelta() => lastFrameMousePos - GetGlobalMousePosition();

    public Vector2 MouseInputs()
    {
        // Mouse Inputs detection
        Vector2 lDirectionToCursor = (GetGlobalMousePosition() - GlobalPosition).Normalized();

        return lDirectionToCursor;
    }

    public Vector2 KeyboardInputs()
    {
        // Keyboard/gamepad inputs detections
        Vector2 lAxis = new Vector2(
                                    Input.GetAxis(INPUTS.AIM_LEFT, INPUTS.AIM_RIGHT),
                                    Input.GetAxis(INPUTS.AIM_UP, INPUTS.AIM_DOWN));

        return lAxis;
    }
}
