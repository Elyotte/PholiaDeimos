using Godot;
using System;

public class Cursor : Node2D
{
    [Export] private NodePath cursorPath = "Cursor";
    private Sprite cursor;
    

    [Export] private float distanceToCursor = 70f;
    private Vector2 direction;

    public Vector2 cursorGlobalPosition => cursor.GlobalPosition;
    public Vector2 originToCursorDirection => (cursor.GlobalPosition - GlobalPosition).Normalized();

    public override void _Ready()
    {
        base._Ready();
        if (cursorPath != null)
        {
            cursor = GetNode<Sprite>(cursorPath);
        }

    }

    public void SetCursorDirection(Vector2 pDirection)
    {
        direction = pDirection.Normalized();
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        SetCursorFromOriginPosition(direction);
    }

    private void SetCursorFromOriginPosition(Vector2 pDirection)
    {
        if (cursor == null) return;

        cursor.Position = pDirection.Normalized() * distanceToCursor;
    }
}
