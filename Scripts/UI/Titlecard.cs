using Godot;
using System;

public class Titlecard : Control
{
    [Export] NodePath buttonPath;
    Button button;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        button = GetNode<Button>(buttonPath);
        button.Connect(SignalNames.BUTTON_CLICKED,this,nameof(OnStartGameClicked));
    }

    void OnStartGameClicked()
    {
        GameManager.Instance?.StartGame();
        Hide();
    }

    public override void _ExitTree()
    {
        button.Disconnect(SignalNames.BUTTON_CLICKED,this,nameof(OnStartGameClicked));
        base._ExitTree();
    }
}
