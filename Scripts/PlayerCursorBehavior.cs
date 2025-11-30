using Godot;
using System;

public class PlayerCursorBehavior : LivingObject
{
    // Nodepath
    [Export] NodePath cursorPath = "CursorComponent";

    public Vector2 moveInput { get; private set; }
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
        if (bUseMouse) // when mouse is used
        {
            // touching the keyboard will exit this condition
            bUseMouse = KeyboardInputs() == Vector2.Zero;
            moveInput = MouseInputs();
        }
        else // when keyboard has been touched
        {
            // moving the mouse will exit the loop
            bUseMouse = GetMouseDelta().Length() >= deadzone;
            moveInput = KeyboardInputs();

        }
    }

    public Vector2 GetMouseDelta() => lastFrameMousePos - GetGlobalMousePosition();

    /// <summary>
    /// Compute mouse delta depending on previous mouse cursor position
    /// To ensure this method works, verify that lastFrameMousePos is
    /// correctly set in the class' _Process or _PhysicsProcess
    /// </summary>
    /// <returns></returns>
    public Vector2 MouseInputs() => (GetGlobalMousePosition() - GlobalPosition).Normalized();

    /// <summary>
    /// Getter function to compute player input with gamepad axis
    /// </summary>
    /// <returns></returns>
    public Vector2 KeyboardInputs() => new Vector2(
                                    Input.GetAxis(INPUTS.AIM_LEFT, INPUTS.AIM_RIGHT),
                                    Input.GetAxis(INPUTS.AIM_UP, INPUTS.AIM_DOWN));
}
