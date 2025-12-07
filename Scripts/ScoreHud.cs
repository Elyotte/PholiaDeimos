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
        GameManager.onScoreUpdate += UpdateScoreLabel;
    }

    public void UpdateScoreLabel(int pNewScore)
    {
        scoreLabel.Text = scoreText + pNewScore;
    }
}
