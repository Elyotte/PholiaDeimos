using Godot;
using System.Collections.Generic;
using System.Data;

public class EnemySpawner : Node
{
    [Export] List<PackedScene> possibleEnemies = new List<PackedScene>();
    [Export] List<NodePath> spawnPointsPath = new List<NodePath>();
    [Export] float spawnRate = 2f;
    [Export] int spawnCount = 3;
    [Export] float spawnRadiusAroundSpawnPoints = 30f;

    float elapsed;
    RandomNumberGenerator rng = new RandomNumberGenerator();
    List<Node2D> spawnPoints = new List<Node2D>();

    public override void _Ready()
    {
        elapsed = spawnRate;
        Init();
    }

    public override void _Process(float delta)
    {
        elapsed -= delta;
        if (elapsed <= 0)
        {
            elapsed += spawnRate;
            SpawnEnemyAtRandomPosInPositionList();
        }
    }

    void Init()
    {
        spawnPoints.Clear();
        int length = spawnPointsPath.Count;
        for (int i = 0; i < length; i++)
        {
            spawnPoints.Add(GetNode<Node2D>(spawnPointsPath[i]));
        }
        GD.Print(spawnPoints.Count);
    }

    void SpawnEnemyAtRandomPosInPositionList()
    {
        int positionCount = spawnPoints.Count;
        int enemyCounts = possibleEnemies.Count;
        if (positionCount == 0 || enemyCounts == 0)
        {
            GD.PrintErr("No enemies or spawn point populated");
            return;
        }

        rng.Randomize();
        Enemy lEnemy = possibleEnemies[rng.RandiRange(0, enemyCounts - 1)].Instance() as Enemy;

        GetTree().Root.AddChild(lEnemy);
        lEnemy.GlobalPosition = spawnPoints[rng.RandiRange(0, positionCount - 1)].GlobalPosition;
    }


}
