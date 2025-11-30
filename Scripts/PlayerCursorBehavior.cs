using Godot;
using System;

public class PlayerCursorBehavior : LivingObject
{
    // Nodepath
    [Export] protected NodePath cursorPath = "CursorComponent";
    const float MOUSE_DELTA_THRESHOLD = .5f;

    // Dynamic 
    protected Cursor m_cursor;

    public bool isCursorEnabled { get; protected set; } = true;
    public Vector2 moveInput { get; protected set; }
    public bool bUseMouse { get; private set; } = false;
    protected Vector2 m_lastFrameMousePos { get; private set; }


    public override void _Ready()
    {
        m_cursor = GetNode<Cursor>(cursorPath);
        base._Ready();
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
       
        if (isCursorEnabled)
        {
            SelectMoveMode();

            m_cursor.SetCursorFromOriginPosition(moveInput);

            m_lastFrameMousePos = GetGlobalMousePosition();
        }
    }

    public void SelectMoveMode()
    {
        if (bUseMouse) // when mouse is used
        {
            // touching the keyboard will exit this condition
            SetUseMouse(KeyboardInputs() == Vector2.Zero);
            moveInput = MouseInputs();
        }
        else // when keyboard has been touched
        {
            // moving the mouse will exit the loop
            SetUseMouse(GetMouseDelta().Length() >= MOUSE_DELTA_THRESHOLD);
            moveInput = KeyboardInputs();
        }
    }

    public Vector2 GetMouseDelta() => m_lastFrameMousePos - GetGlobalMousePosition();

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

    public void SetUseMouse(bool pBool)
    {
        if (bUseMouse == pBool) return; 
        bUseMouse = pBool;
        Input.MouseMode = pBool ? Input.MouseModeEnum.Visible : Input.MouseModeEnum.Hidden ;
    }
}
