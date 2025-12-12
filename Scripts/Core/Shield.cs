using Godot;
using System;

public class Shield : Area2D
{
    CollisionShape2D shape;
    NodePath path = "CollisionShape2D";

    public override void _Ready()
    {
        shape = GetNode<CollisionShape2D>(path);
        Connect(SignalNames.AREA_ENTERED, this, nameof(OnAreaEntered));
        CallDeferred(nameof(Start));
    }

    void Start() {
        if(GameManager.deimos != null)
        {
            GameManager.deimos.onSplit += OnSplit;
            GameManager.deimos.onResplit += OnResplit;
        }
    }

    void OnSplit()
    {
        shape.Disabled = true;
        Visible = false;
    }

    void OnResplit()
    {
        shape.Disabled = false;
        Visible = true;
    }

    void OnAreaEntered(Area2D area)
    {
        if (area is Bullet lBullet)
        {
            
        }
    }

    public override void _ExitTree()
    {
        if (GameManager.Instance != null)
        {
            GameManager.deimos.onSplit -= OnSplit;
            GameManager.deimos.onResplit -= OnResplit;
        }
        Disconnect(SignalNames.AREA_ENTERED,this, nameof(OnAreaEntered));
        base._ExitTree();
    }
}
