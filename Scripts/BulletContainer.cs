using Godot;
using System;

public class BulletContainer : Node
{
    static public BulletContainer instance;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        instance = this;
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
