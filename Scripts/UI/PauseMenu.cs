using Godot;
using System;
using System.Reflection;
using System.Threading;

public class PauseMenu : Control
{
    [Export] NodePath unPauseBtnPath;
    Button resumeButton;

    public override void _Ready()
    {

        resumeButton = GetNode<Button>(unPauseBtnPath);
        resumeButton.Connect(SignalNames.BUTTON_CLICKED, this, nameof(Resume));
        CallDeferred(nameof(Start)); 
    }

    void Start()
    {
        if (GameManager.Instance == null) return;

        Hide();
        GameManager.onPause += Show;
        GameManager.onResume += Hide;
    }

    public void Resume()
    {
        GameManager.onResumeByButton?.Invoke();
    }

    public override void _ExitTree()
    {
        GameManager.onPause -= Show;
        GameManager.onResume -= Hide;
        resumeButton.Disconnect(SignalNames.BUTTON_CLICKED, this,nameof(Resume));
        base._ExitTree();
    }
}
