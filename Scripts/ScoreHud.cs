using Godot;
using System;

public class ScoreHud : Control
{
    [Export] NodePath scoreTextPath;

    Label scoreLabel;
    string scoreText => $"Score : ";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        scoreLabel = GetNode<Label>(scoreTextPath);
        Hide();
        UpdateScoreLabel(0);
        CallDeferred(nameof(Start));
    }

    public void Start()
    {
        if (GameManager.Instance == null) return;
        GameManager.onScoreUpdate += UpdateScoreLabel;
        GameManager.onGameStart += Show;
        GameManager.onGameStop += Hide;
    }

    public void UpdateScoreLabel(int pNewScore =0)
    {
        scoreLabel.Text = scoreText + pNewScore.ToString();
    }

    public override void _ExitTree()
    {
        GameManager.onScoreUpdate -= UpdateScoreLabel;
        GameManager.onGameStart -= Show;    
        GameManager.onGameStop -= Hide;
        base._ExitTree();
    }
}
