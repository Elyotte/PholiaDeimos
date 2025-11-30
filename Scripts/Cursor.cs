using Godot;
using System;

public class Cursor : Node2D
{
    [Export] private NodePath cursorPath = "Cursor";
    private Sprite cursor;
    

    [Export] private float distanceToCursor = 70f;

    public override void _Ready()
    {
        base._Ready();
        if (cursorPath != null)
        {
            cursor = GetNode<Sprite>(cursorPath);
        }

    }

    public void SetCursorFromOriginPosition(Vector2 pDirection)
    {
        if (cursor == null) return;

        cursor.Position = pDirection.Normalized() * distanceToCursor;
    }
}
