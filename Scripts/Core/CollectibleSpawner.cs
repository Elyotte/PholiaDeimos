using Godot;
using System;

public class CollectibleSpawner : Node
{
    [Export] float collectibleSpawnRate = .35f;
    RandomNumberGenerator rng = new RandomNumberGenerator();
    [Export] PackedScene collectiblePacked;

    public override void _Ready()
    {
        Enemy.onDeath += SpawnCollectible;
        rng.Randomize();
    }
    private void SpawnCollectible(Vector2 pPos)
    {
        if (rng.Randf() >= collectibleSpawnRate)
        {
            Collectible lCol = collectiblePacked.Instance() as Collectible;
            lCol.GlobalPosition = pPos;
            GetTree().CurrentScene.AddChild(lCol);
        }
    }

    public override void _ExitTree()
    {
        Enemy.onDeath -= SpawnCollectible;

        base._ExitTree();
    }
}
