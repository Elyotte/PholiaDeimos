using Godot;
using System;

public class PholiaDeimosFlowManager : Node
{
    CanvasLayer HUD;
    NodePath HudPath = "CanvasLayer";
    public override void _Ready()
    {
        HUD = GetNode<CanvasLayer>(HudPath);
        CallDeferred(nameof(Start));
    }

    void Start()
    {
        if (GameManager.Instance == null) return;

        HUD.Hide();
        GameManager.onGameStart += HUD.Show;
        GameManager.onGameStop += HUD.Hide;
    }

    public override void _ExitTree()
    {
        GameManager.onGameStop -= HUD.Hide;
        GameManager.onGameStart -= HUD.Show;
        base._ExitTree();
    }
}
